﻿using BepInEx;
using BepInEx.Bootstrap;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using ControlCompanyDetector.Patches;
using static UnityEngine.Scripting.GarbageCollector;
using System;
using System.Linq;
using Unity.Netcode;

namespace ControlCompanyDetector.Logic
{
    internal static class Detector
    {
        // private static Dictionary<string, PluginInfo> Mods = new Dictionary<string, PluginInfo>();
        // private static int previousLastCCLine;


        public static IEnumerator StartDetection()
        {
            //HUDManagerPatch.displayTip = false;
            //if (Player.LocalPlayer == null)
            //{
            //    Mods = Chainloader.PluginInfos;
            //    Plugin.LogInfoMLS("Starting detection...");
            //    Network.Broadcast("LC_API_ReqGUID");
            //    CoroutineManager.StartCoroutine(ReadBepinLog());
            //}

            Plugin.LogInfoMLS("Checking if host and client are friends...");

            yield return new WaitForSeconds(3.5f);

            if (!StartOfRound.Instance.IsClientFriendsWithHost())
            {
                Detector.Detect();
            }
            else if (!Plugin.ignoreFriendlyLobbies.Value)
            {
                Detector.Detect();
            }
            else if (Plugin.showInfoMessage.Value)
            {
                Detector.SendUITip("Control Company Detector:", "Lobbies created by friends are currently being ignored. Check the mod config for more info", false);
            }
        }

        internal static void Detect()
        {
            Plugin.LogInfoMLS("Detection started");
            Plugin.LogInfoMLS("Lobby name: " + GameNetworkManager.Instance.steamLobbyName);
            if (GameNetworkManager.Instance != null)
            {
                if (GameNetworkManager.Instance.steamLobbyName.Contains('\u200b'))
                {
                    Plugin.LogWarnMLS("Control Company has been detected");
                    Detector.SendUITip("WARNING:", "The host is using Control Company", true);
                }
            }
        }

        //[NetworkMessage("CCD_SendMods")]
        //internal static void SendUITip(/*ulong senderId*/ string header, string message)
        //{
        // Player player = Player.Get(senderId);
        // HUDManagerPatch.displayTip = true;
        // Player.LocalPlayer.QueueTip(header, message, 5f, 0, true, false, "LC_Tip1");
        //}

        internal static void SendUITip(/*ulong senderId*/ string header, string message, bool warning)
        {
            // Player player = Player.Get(senderId);
            // HUDManagerPatch.displayTip = true;
            HUDManager.Instance.DisplayTip(header, message, warning, false, "LC_Tip1");
        }

        // [NetworkMessage("LC_APISendMods")]
        //public static IEnumerator ReadBepinLog()
        //{
        //    Plugin.LogInfoMLS("Waiting to read log file...");

        //    yield return new WaitForSeconds(4.5f);

        //    bool shouldContinue = Detector.CheckLocalPlugins();

        //    if (!Player.LocalPlayer.IsHost && shouldContinue)
        //    {
        //        Plugin.LogInfoMLS("Reading log file");
        //        string bepinexPath = Plugin.bepinexPathEntry.Value;
        //        DirectoryInfo bepinexDirectory = new DirectoryInfo(bepinexPath);
        //        Plugin.LogInfoMLS("BepInEx folder: " + bepinexDirectory);
        //        FileInfo[] ogFiles = bepinexDirectory.GetFiles("LogOutput.log");
        //        Plugin.LogInfoMLS("Log file: " + ogFiles[0].FullName);
        //        Plugin.LogInfoMLS("Copying file...");
        //        File.Copy(ogFiles[0].FullName, bepinexPath + "/LogCopy.log", true);
        //        FileInfo[] filesToEdit = bepinexDirectory.GetFiles("LogCopy.log");
        //        if (filesToEdit.Length > 0)
        //        {
        //            // int lastLC_APIline = FindLastLineOccurrence("responded with these mods", filesToEdit);
        //            int lastCCline = FindLastLineOccurrence("ControlCompany.ControlCompany", filesToEdit);
        //            if (previousLastCCLine != lastCCline)
        //            {
        //                Plugin.LogWarnMLS("Control Company has been detected");
        //                Detector.SendUITip("WARNING:", $"{Player.HostPlayer.Username} is using Control Company");
        //            }
        //            previousLastCCLine = lastCCline;
        //        }
        //    }
        //}

        //internal static bool CheckLocalPlugins()
        //{
        //    bool shouldContinue = true;
        //    foreach (PluginInfo info in Mods.Values)
        //    {
        //        if (info.Metadata.GUID.Equals("ControlCompany.ControlCompany"))
        //        {
        //            shouldContinue = false;
        //            Plugin.LogWarnMLS("Control Company detected on client, stopping corroutine. Please do not use Control Company and this mod together");
        //            Detector.SendUITip("ERROR:", "Please do not use Control Company and Control Company Detector together");
        //        }
        //    }
        //    return shouldContinue;
        //}

        //internal static int FindLastLineOccurrence(string targetLine, FileInfo[] filesToEdit)
        //{
        //    int lastLineOccurrence = 0;
        //    using (StreamReader reader = filesToEdit[0].OpenText())
        //    {
        //        string line;
        //        while ((line = reader.ReadLine()) != null)
        //        {
        //            if (line.Contains(targetLine))
        //            {
        //                lastLineOccurrence++;
        //            }
        //        }
        //    }
        //    return lastLineOccurrence;
        //}
    }
}
