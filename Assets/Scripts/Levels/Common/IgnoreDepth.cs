using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Levels.Common
{
    public class IgnoreDepth : MonoBehaviour
    {


        private static List<IgnoreDepth> elements = null;

        public static void Initialize()
        {
            elements = new List<IgnoreDepth>(FindObjectsOfType<IgnoreDepth>());
        }

        private static void CheckInitialized()
        {
            if (elements == null)
            {
                Initialize();
            }
        }

        public static void OnLevelHiding()
        {
            CheckInitialized();

            foreach (var ignoreDepth in elements)
            {
                ignoreDepth.ToFrontAnchor();
            }
        }

        public static void OnLevelUnhiding()
        {
            CheckInitialized();


            foreach (var ignoreDepth in elements)
            {
                ignoreDepth.ToStartAnchor();
            }
        }

        private float StartDepth;

        private void Start()
        {
            StartDepth = transform.position.z;
        }

        public void ToFrontAnchor()
        {
            if (LevelDepth.IsExist)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, LevelDepth.FrontDepth - 1);
            }
        }

        public void ToStartAnchor()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, StartDepth);
        }

        
        private void OnDisable()
        {
            elements = null;
        }

    }
}
