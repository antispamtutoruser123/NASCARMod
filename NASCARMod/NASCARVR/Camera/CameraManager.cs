using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NASCARVR
{
    class CameraManager
    {
        static int cameramode;
        static Vector3 roffsetprev= Vector3.zero;
        static Vector3 prevpos = Vector3.zero;
        public static void Recenter()
        {
            Logs.WriteInfo($"LLLL: RECENTERING");
            if (!CameraPatches.VRCamera) return;

            Vector3 offset = CameraPatches.startpos - CameraPatches.VRCamera.transform.localPosition;
            Vector3 roffset = CameraPatches.startrot - CameraPatches.VRCamera.transform.localEulerAngles;
    
           // CameraPatches.DummyCamera.transform.RotateAround(CameraPatches.VRCamera.transform.position, Vector3.up, roffset.y - roffsetprev.y);

            CameraPatches.DummyCamera.transform.Translate(offset-prevpos);
    
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
