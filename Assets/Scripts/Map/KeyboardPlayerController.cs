using UnityEngine;

namespace Assets.Scripts.Map
{
    public class KeyboardPlayerController : PlayerController
    {
        protected override void Update()
        {

            if(!CanControl)
                return;

            if (Input.GetKeyDown(KeyCode.W)) Player.Move(Direction.Top);
            else if (Input.GetKeyDown(KeyCode.S)) Player.Move(Direction.Bottom);
            else if (Input.GetKeyDown(KeyCode.A)) Player.Move(Direction.Left);
            else if (Input.GetKeyDown(KeyCode.D)) Player.Move(Direction.Right);
        }
    }
}
