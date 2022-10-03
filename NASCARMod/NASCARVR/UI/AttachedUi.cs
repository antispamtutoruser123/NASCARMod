using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace NASCARVR
{
    class AttachedUi : MonoBehaviour
    {
        private Transform targetTransform;

        public static void Create<TAttachedUi>(Canvas canvas, float scale = 0)
            where TAttachedUi : AttachedUi
        {
            var instance = canvas.gameObject.AddComponent<TAttachedUi>();
            if (scale > 0) canvas.transform.localScale = new Vector3(scale, scale, scale);

            canvas.renderMode = RenderMode.WorldSpace;
            for (int i = 0; i < canvas.gameObject.transform.childCount; i++)
            {
                Logs.WriteInfo($"canvas Child{i}:  {canvas.gameObject.transform.GetChild(i).name}");
                if (canvas.gameObject.transform.GetChild(i).name == "MediaPlayer")
                    foreach (var component in canvas.gameObject.transform.GetChild(i).GetComponents<Component>())
                    {
                        Logs.WriteInfo($"MediaPlayer components:  {component.name} {component.GetType()}");
                       

                    }
            }
        }
        protected virtual void Start()
        {
            transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2f;
            transform.rotation = Camera.main.transform.rotation;
        }
        protected virtual void Update()
        {
            UpdateTransform();
        }

        public void SetTargetTransform(Transform target)
        {
            targetTransform = target;
        }

        private void UpdateTransform()
        {
            transform.position = Camera.main.transform.position + Camera.main.transform.forward*2f;
            transform.rotation = Camera.main.transform.rotation;
        }
    }
}
