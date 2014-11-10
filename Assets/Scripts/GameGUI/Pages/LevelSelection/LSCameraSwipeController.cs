using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.GameGUI.Pages.LevelSelection
{
    public class LSCameraSwipeController : LSCameraControllerBase
    {
        public float Speed = 10f;
        private void Update()
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
                Camera.transform.Translate(-touchDeltaPosition.x * Speed, 0, 0);
                SetDirty();
            }
        }
    }
}
