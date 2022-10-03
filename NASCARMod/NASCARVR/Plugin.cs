using BepInEx;
using HarmonyLib;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace NASCARVR
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "com.NoSmoke.VRMods.UEBSVR";
        public const string PLUGIN_NAME = "UEBSVR";
        public const string PLUGIN_VERSION = "0.0.1";

        public static string gameExePath = Process.GetCurrentProcess().MainModule.FileName;
        public static string gamePath = Path.GetDirectoryName(gameExePath);

        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            //new AssetLoader();

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
            Logs.WriteInfo($"LLLL: Calling InitSteamVR");
            InitSteamVR();
        }
        private static void InitSteamVR()
        {
            Logs.WriteInfo($"LLLL: Calling SteamVR_Actions.PreInitialize");
            SteamVR_Actions.PreInitialize();
            Logs.WriteInfo($"LLLL: Calling SteamVR.Initialize");
            SteamVR.Initialize();

            SteamVR_Settings.instance.pauseGameWhenDashboardVisible = true;


            // INPUT TEST
            SteamVR_Actions._default.InteractUI.AddOnStateDownListener(OnInteractUI2Down, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.InteractUI2.AddOnStateDownListener(OnInteractUIDown, SteamVR_Input_Sources.Any);
         
        }

        // INPUT TEST

        public static void OnInteractUI2Down(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            Logs.WriteInfo("InteractUI2 is Down");
            CameraManager.Recenter();
        }
        public static void OnInteractUIDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            Logs.WriteInfo("InteractUI is Down");
            CameraManager.Recenter();
        }

       
    }
}
