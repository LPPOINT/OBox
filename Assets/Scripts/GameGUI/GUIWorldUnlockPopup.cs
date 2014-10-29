using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Camera.Effects;
using UnityEngine;

namespace Assets.Scripts.GameGUI
{
    public class GUIWorldUnlockPopup : MonoBehaviour
    {
        public static GUIWorldUnlockPopup Current { get; private set; }

        public static void Show(GUIWorldModel model)
        {
            CloseIfExist();
            var go = new GameObject("WorldUnlockPopup");
            Current = go.AddComponent<GUIWorldUnlockPopup>();
            Current.Model = model;
        }

        public static void CloseIfExist()
        {
            if (Current != null)
            {
                Current.CloseWithoutBlur();
                Current = null;
            }
        }


        public GUIWorldModel Model;

        public void Close()
        {
            CameraBlurEffect.BlurOut();
            Destroy(gameObject);
        }

        private void CloseWithoutBlur()
        {
            Destroy(gameObject);
        }

        private IEnumerator Start()
        {
            var canvas = GetComponent<Canvas>();
            canvas.enabled = false;
            CameraBlurEffect.BlurIn();
            yield return new WaitForSeconds(0.3f);
            canvas.enabled = true;
        }

    }
}
