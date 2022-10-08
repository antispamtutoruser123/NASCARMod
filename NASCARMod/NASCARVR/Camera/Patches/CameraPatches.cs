using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using Valve.VR;

namespace NASCARVR
{


    [HarmonyPatch]
    public class CameraPatches
    {
        static float curangle = 0;
        public static Vector3 startpos,startrot,offset;

        public static GameObject DummyCamera, VRCamera,VRPlayer;

        private static readonly string[] canvasesToIgnore =
    {
        "com.sinai.unityexplorer_Root", // UnityExplorer.
        "com.sinai.unityexplorer.MouseInspector_Root", // UnityExplorer.
        "IntroCanvas"
    };
        private static readonly string[] canvasesToWorld =
    {
        "OverlayCanvas"
    };

        [HarmonyPostfix]
        [HarmonyPatch(typeof(CanvasScaler), "OnEnable")]
        private static void MoveIntroCanvas(CanvasScaler __instance)
        {
            if (IsCanvasToIgnore(__instance.name)) return;

            Logs.WriteInfo($"Hiding Canvas:  {__instance.name}");
            var canvas = __instance.GetComponent<Canvas>();

           if (IsCanvasToWorld(__instance.name)) 
             AttachedUi.Create<StaticUi>(canvas, 0.00145f);
        }

        static bool buttondown = false;
        [HarmonyPrefix]
        [HarmonyPatch(typeof(MGI.Platform.PC.NormalizedPCControllerState), "SetFromXInputGamepad")]
        private static void GetButtonInput(MGI.Platform.PC.NormalizedPCControllerState __instance) {

            if (__instance.buttons[12] && !buttondown)
            {  // X button
                CameraManager.Recenter();
                buttondown = true;
            }

            if (!__instance.buttons[12])
                buttondown = false;
        }


        [HarmonyPrefix]
        [HarmonyPatch(typeof(MGI.CarManager.CarInstancePlayer), "Start")]
        private static void createcamera(MGI.CarManager.CarInstancePlayer __instance)
        {
            if (!VRCamera) {
                Logs.WriteInfo($"LLLL: Creating VRCamera");
                VRCamera = new GameObject("VRCamera");
            DummyCamera = new GameObject("DummyCamera");
            VRCamera.tag = "MainCamera";
            VRCamera.AddComponent<Camera>();
            VRCamera.GetComponent<Camera>().nearClipPlane = .01f;
            VRCamera.GetComponent<Camera>().farClipPlane = 50000f;

            DummyCamera.transform.parent = __instance.transform;
                VRPlayer = __instance.gameObject;
                // Third Person
                // DummyCamera.transform.localPosition = new Vector3(0,1f,0);
                // first person
                 DummyCamera.transform.localPosition = new Vector3(0, -1.02f, 0);

                VRCamera.transform.parent = DummyCamera.transform;
                startpos = new Vector3(.5f, 1.8f, .3f);
                startrot = new Vector3(356.6f,187.9f,.6f);
                offset = startpos - VRCamera.transform.localPosition;
                Logs.WriteInfo($"RRRRRR: In Car position: startpos startrot offset {VRCamera.transform.localPosition} {VRCamera.transform.localEulerAngles} {offset}");


            }

        }


        private static bool IsCanvasToIgnore(string canvasName)
        {
            foreach (var s in canvasesToIgnore)
                if (Equals(s, canvasName))
                    return true;
            return false;
        }

        private static bool IsCanvasToWorld(string canvasName)
        {
            foreach (var s in canvasesToWorld)
                if (Equals(s, canvasName))
                    return true;
            return false;
        }

    }
}

