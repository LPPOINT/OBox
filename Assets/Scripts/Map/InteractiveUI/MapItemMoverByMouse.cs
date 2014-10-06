using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Map.InteractiveUI
{
    public class MapItemMoverByMouse : MapItemsMover
    {

        private Vector2 lastMousePosition;
        private bool lastMousePressed;



        private void Update()
        {


            if (Input.GetMouseButton(0))
            {

                var mousePos = Input.mousePosition;

                var corners = new Vector3[4];
                UIBorder.rectTransform.GetWorldCorners(corners);

                var worldRect = new Rect(corners[1].x, corners[1].y, Math.Abs(corners[2].x - corners[1].x),
                    corners[1].y - corners[0].y);
                worldRect.Set(worldRect.xMin, worldRect.yMin - worldRect.height, worldRect.width, worldRect.height);
                var worldMouse = UnityEngine.Camera.main.ScreenToWorldPoint(mousePos);

                if (lastMousePressed && worldRect.Contains(worldMouse))
                {
                    var offset = new Vector2( worldMouse.x - lastMousePosition.x,  worldMouse.y - lastMousePosition.y);
                    Move(offset);
                }

                lastMousePressed = true;
                lastMousePosition = worldMouse;

            }
            else
            {
                if(lastMousePressed) Group.NormalizePositions();
                lastMousePressed = false;
            }

        }
    }
}
