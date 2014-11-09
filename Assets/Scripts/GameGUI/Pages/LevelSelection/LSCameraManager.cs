using UnityEngine;

namespace Assets.Scripts.GameGUI.Pages.LevelSelection
{
    public static class LSCameraManager
    {
        private static UnityEngine.Camera staticCamera;
        private static UnityEngine.Camera dynamicCamera;

        public static UnityEngine.Camera StaticCamera
        {
            get
            {
                if (staticCamera == null)
                {
                    staticCamera = GameObject.FindGameObjectWithTag("StaticCamera").GetComponent<UnityEngine.Camera>();
                }
                return staticCamera;
            }
        }

        public static UnityEngine.Camera DynamicCamera
        {
            get
            {
                if (dynamicCamera == null)
                {
                    dynamicCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<UnityEngine.Camera>();
                }
                return dynamicCamera;
            }
        }

    }
}
