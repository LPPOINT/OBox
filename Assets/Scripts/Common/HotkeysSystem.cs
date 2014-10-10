using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Common
{
    public class HotkeysSystem : MonoBehaviour
    {

        [Serializable]
        public class HotkeyAction
        {
            public KeyCode Key;
            public GameObject Object;
            public string MethodName;
        }


        public List<HotkeyAction> Actions; 

        private void Start()
        {
            
        }

        private void Update()
        {
            foreach (var action in Actions)
            {
                if (Input.GetKeyDown(action.Key))
                {
                    if (action.Object != null && !string.IsNullOrEmpty(action.MethodName))
                    {
                        action.Object.SendMessage(action.MethodName);
                    }
                }
            }
        }

    }
}
