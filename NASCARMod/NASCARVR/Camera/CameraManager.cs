using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NASCARVR
{
    class CameraManager
    {
        static int cameramode;
        public static void Recenter()
        {
            Logs.WriteInfo($"LLLL: RECENTERING");
            if (!CameraPatches.VRCamera) return;

            Vector3 offset = CameraPatches.startpos - CameraPatches.VRCamera.transform.position;
            Vector3 roffset = CameraPatches.startrot - CameraPatches.VRCamera.transform.eulerAngles;

            CameraPatches.DummyCamera.transform.localPosition = new Vector3(0, -1f, 0) - offset;
            CameraPatches.DummyCamera.transform.localEulerAngles = Vector3.zero - roffset;

            switch (cameramode)
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
        }
        private Vector3 GetCameraOffset()
        {
            try
            {
                return CameraPatches.DummyCamera.transform.localPosition - CameraPatches.VRCamera.transform.localPosition;
            }
            catch
            {
                return Vector3.zero;
            }
        }
    }
}
