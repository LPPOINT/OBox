using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Camera.Effects;
using UnityEngine;

namespace Assets.Scripts.GameGUI
{
    public class GUIWorldUnlockPopup : MonoBehaviour
    {

        public static GUIWorldUnlockPopup Current { get; private set; }

        public GUIWorldData Data;
        public IGUIWorldUnlockHandler UnlockHandler;

        private void Start()
        {
            Current = this;
        }

        public void Close()
        {
            CameraBlurEffect.BlurOut();
            Destroy(gameObject);
        }


    }
}
