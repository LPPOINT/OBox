using UnityEngine;

namespace Assets.Scripts.GameGUI.Pages.LevelSelection
{
    public class LSCameraKeyboardController : LSCameraControllerBase
    {
        public float Offset = 0.3f;

        private void Update()
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                Camera.transform.position = new Vector3(Camera.transform.position.x + Offset, Camera.transform.position.y, Camera.transform.position.z);
                SetDirty();
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                Camera.transform.position = new Vector3(Camera.transform.position.x - Offset, Camera.transform.position.y, Camera.transform.position.z);
                SetDirty();
            }
        }
    }
}
