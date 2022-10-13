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
        static bool ingame = false;
        public static Vector3 startpos,startrot,offset;
        public static RenderTexture rt;
        public static GameObject newUI;
        public static GameObject worldcam;

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
        [HarmonyPatch(typeof(CanvasScaler), "OnDisable")]
        private static void ClearSkybox(CanvasScaler __instance)
        {
            if (!ingame)
                foreach (Camera cam in Camera.allCameras)
                {
                    cam.clearFlags = CameraClearFlags.Color;
                   // cam.backgroundColor = Color.blue;
                }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(CanvasScaler), "OnEnable")]
        private static void MoveIntroCanvas(CanvasScaler __instance)
        {
            if (IsCanvasToIgnore(__instance.name)) return;
           
            Logs.WriteInfo($"Hiding Canvas:  {__instance.name}");
            var canvas = __instance.GetComponent<Canvas>();

            if (Equals(canvas.name, "CarSceneRenderCanvas"))
                ingame = false;

            if (IsCanvasToWorld(__instance.name))
                AttachedUi.Create<StaticUi>(canvas, 0.00145f);
            else 
            {

                canvas.renderMode = RenderMode.ScreenSpaceCamera;

                if (!worldcam)
                {
                    worldcam = new GameObject("WorldCam");
                    worldcam.AddComponent<Camera>();

                }

                worldcam.GetComponent<Camera>().enabled = true;
                canvas.worldCamera = worldcam.GetComponent<Camera>();


                if (!rt)
                {
                    rt = new RenderTexture(1280, 720, 24);

                    worldcam.GetComponent<Camera>().targetTexture = rt;

                    newUI = new GameObject("newUI");
                    newUI.AddComponent<Canvas>();
                    newUI.AddComponent<RawImage>();
                    canvas = newUI.GetComponent<Canvas>();
                    canvas.renderMode = RenderMode.WorldSpace;
                    newUI.transform.position =  new Vector3(-130f,0,0);
                    newUI.transform.localScale = new Vector3(.16f, .09f, .1f);
                    newUI.transform.eulerAngles = new Vector3(0,-20f,0);
                    newUI.GetComponent<RawImage>().texture = rt;
               
                }

            }

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
            ingame = true;
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
                startpos = new Vector3(.2f, 1.8f, -.45f);
                startrot = new Vector3(354f,284f,358f);
                offset = startpos - VRCamera.transform.localPosition;
                Logs.WriteInfo($"RRRRRR: In Car position: startpos startrot offset {VRCamera.transform.localPosition} {VRCamera.transform.localEulerAngles} {offset}");
                //CameraManager.Recenter();
               
                foreach (Camera cam in Camera.allCameras)
                    cam.clearFlags = CameraClearFlags.Skybox;

                worldcam.GetComponent<Camera>().enabled = false;

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

