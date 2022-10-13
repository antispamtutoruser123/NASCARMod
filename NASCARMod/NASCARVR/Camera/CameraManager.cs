using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NASCARVR
{
    class CameraManager : MonoBehaviour
    {
        static int cameramode;
        static Vector3 roffsetprev= Vector3.zero;
        static Vector3 prevpos = Vector3.zero;

        public static void RecenterRotation()
        {
            if (!CameraPatches.VRCamera) return;

            Vector3 offset = CameraPatches.startpos - CameraPatches.VRCamera.transform.localPosition;
            var angleOffset = CameraPatches.VRPlayer.transform.eulerAngles.y - CameraPatches.VRCamera.transform.eulerAngles.y;

         //   CameraPatches.DummyCamera.transform.Rotate(0, angleOffset, 0);
            CameraPatches.DummyCamera.transform.RotateAround(CameraPatches.VRCamera.transform.position, Vector3.up, angleOffset);
         //   CameraPatches.DummyCamera.transform.eulerAngles = Vector3.up * CameraPatches.DummyCamera.transform.eulerAngles.y;

        }

        public static void Recenter()
        {
            Logs.WriteInfo($"LLLL: RECENTERING");
            if (!CameraPatches.VRCamera) return;

            Vector3 offset = CameraPatches.startpos - CameraPatches.VRCamera.transform.localPosition;
            Vector3 roffset = CameraPatches.startrot - CameraPatches.VRCamera.transform.localEulerAngles;

            

            CameraPatches.DummyCamera.transform.Translate(offset - prevpos);

            RecenterRotation();

            roffsetprev = roffset;
            prevpos = offset;

         /*   switch (cameramode)
            {
                case 0:  // recenter
                    cameramode = 1;
                    break;
                case 1:  // switch to 3rd person
                    CameraPatches.DummyCamera.transform.localPosition = new Vector3(0, 1f, 0) - offset;
                    cameramode = 0;
                    break;
                default:
                    break;
            }
         */
        }
     
    }
}
