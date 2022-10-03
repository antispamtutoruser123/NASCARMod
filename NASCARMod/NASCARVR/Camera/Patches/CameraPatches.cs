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

        public static GameObject DummyCamera, VRCamera;

        private static readonly string[] canvasesToIgnore =
{
        "com.sinai.unityexplorer_Root", // UnityExplorer.
        "com.sinai.unityexplorer.MouseInspector_Root", // UnityExplorer.
        "IntroCanvas"
    };

      /*  [HarmonyPostfix]
        [HarmonyPatch(typeof(RenderHeads.Media.AVProVideo.MediaPlayer), "Update")]
        private static void MoveIntroCanvas(RenderHeads.Media.AVProVideo.MediaPlayer __instance)
        {
            __instance.Stop();
        }
        */

        [HarmonyPostfix]
        [HarmonyPatch(typeof(CanvasScaler), "OnEnable")]
        private static void MoveIntroCanvas(CanvasScaler __instance)
        {
            if (IsCanvasToIgnore(__instance.name)) return;

            Logs.WriteInfo($"Hiding Canvas:  {__instance.name}");
            var canvas = __instance.GetComponent<Canvas>();

          //  if (__instance.name == "OverlayCanvas") 
           //  AttachedUi.Create<StaticUi>(canvas, 0.00145f);
        }


        [HarmonyPrefix]
        [HarmonyPatch(typeof(MGI.CarManager.CarInstancePlayer), "Start")]
        private static void fixlighting(MGI.CarManager.CarInstancePlayer __instance)
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

                // Third Person
                // DummyCamera.transform.localPosition = new Vector3(0,1f,0);
                // first person
                 DummyCamera.transform.localPosition = new Vector3(0, -1f, 0);

                VRCamera.transform.parent = DummyCamera.transform;
                startpos = new Vector3(.2f, 1.8f, -.5f);
                offset = startpos - VRCamera.transform.localPosition;
                Logs.WriteInfo($"RRRRRR: In Car position: startpos startrot offset {VRCamera.transform.localPosition} {VRCamera.transform.localEulerAngles} {offset}");


            }

        }


        [HarmonyPostfix]
        [HarmonyPatch(typeof(MGI.CarManager.CarInstancePlayer), "Update")]
        private static void MoveCamera(MGI.CarManager.CarInstancePlayer __instance)
        {

            float newx, newz;

            float speed = 4f * Time.deltaTime;

            //DummyCamera.transform.localPosition += new Vector3(0, .1f*Time.deltaTime, 0);

            if (Input.GetKey("1") || Input.GetKey("2"))
                    speed = 40f * Time.deltaTime;
            offset = startpos - VRCamera.transform.localPosition;
//Logs.WriteInfo($"LLLL: In Car position: startpos startrot offset {VRCamera.transform.localPosition} {VRCamera.transform.localEulerAngles} {offset}");

            if (Input.GetKey(KeyCode.Return))
            {
                Logs.WriteInfo($"LLLL: ENTER KEY PRESSED");
                CameraManager.Recenter();

            }

            if (Input.GetKey(KeyCode.Mouse0))
            {
                Logs.WriteInfo($"LLLL: Moving VRCamera Mouse0 ");
                newx = (VRCamera.transform.localRotation * Vector3.right * -speed).x;
                    newz = (VRCamera.transform.localRotation * Vector3.right * -speed).z;
                    DummyCamera.transform.Translate(newx, 0, newz);
                }

                if (Input.GetKey(KeyCode.Joystick1Button0))
            {
                Logs.WriteInfo($"LLLL: Moving VRCamera Joystick1Button0");
                newx = (VRCamera.transform.localRotation * Vector3.right * speed).x;
                    newz = (VRCamera.transform.localRotation * Vector3.right * speed).z;
                    DummyCamera.transform.Translate(newx, 0, newz);
                }
                if (Input.GetKey(KeyCode.W))
                {
                Logs.WriteInfo($"LLLL: Moving VRCamera W");
                newx = (VRCamera.transform.localRotation * Vector3.forward * speed).x;
                    newz = (VRCamera.transform.localRotation * Vector3.forward * speed).z;
                 //   DummyCamera.transform.Translate(newx, 0, newz);
                DummyCamera.transform.localPosition += new Vector3(0, 0, -speed * Time.deltaTime);
            }
                
                if (Input.GetKey("s"))
                {
                    newx = (VRCamera.transform.localRotation * Vector3.forward * -speed).x;
                    newz = (VRCamera.transform.localRotation * Vector3.forward * -speed).z;
                  //  DummyCamera.transform.Translate(newx, 0, newz);
                DummyCamera.transform.localPosition += new Vector3(0, 0, speed * Time.deltaTime);
                }

                if (Input.GetKey("r"))
                    DummyCamera.transform.Translate(0f, speed * .7f, 0f);
                if (Input.GetKey("f"))
                    DummyCamera.transform.Translate(0f, -speed * .7f, 0f);
                /*   if (Input.GetKey("g"))
                       DummyCamera.transform.parent.RotateAround(VRCamera.transform.position, Vector3.up, -30f * Time.deltaTime);
                   if (Input.GetKey("h"))
                       DummyCamera.transform.parent.RotateAround(VRCamera.transform.position, Vector3.up, 30f * Time.deltaTime);
               */
                // snap turn
                if (Input.GetKeyDown("g"))
                    DummyCamera.transform.parent.RotateAround(VRCamera.transform.position, Vector3.up, curangle - 30f);
                if (Input.GetKeyDown("h"))
                    DummyCamera.transform.parent.RotateAround(VRCamera.transform.position, Vector3.up, curangle + 30f);
            
        }

        private static bool IsCanvasToIgnore(string canvasName)
        {
            foreach (var s in canvasesToIgnore)
                if (Equals(s, canvasName))
                    return true;
            return false;
        }

    }
}

