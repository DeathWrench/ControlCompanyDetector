﻿using ControlCompanyDetector.Logic;
using HarmonyLib;
using LC_API.GameInterfaceAPI.Features;
using UnityEngine;

namespace ControlCompanyDetector.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    internal class StartOfRoundPatch : MonoBehaviour
    {
        [HarmonyPatch("OnPlayerConnectedClientRpc")]
        [HarmonyPostfix]
        static void PatchOnPlayerConnected()
        {
            if (HUDManager.Instance != null && Player.HostPlayer != null)
            {
                Detector.StartDetection();
            }
        }
    }
}
