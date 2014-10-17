using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.LevelSelection
{
    public sealed class LSTrigger : MonoBehaviour
    {

        public enum LSTriggerMode
        {
            Single,
            Smart,
            EveryFrame
        }

        public LSTriggerMode Mode;

        public event EventHandler TriggerInvoked;

        private void OnTriggerInvoked()
        {
            var handler = TriggerInvoked;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        private GameObject cameraObject;

        private void Start()
        {
            cameraObject = UnityEngine.Camera.main.gameObject;
        }

        private void Update()
        {
            
        }

    }
}
