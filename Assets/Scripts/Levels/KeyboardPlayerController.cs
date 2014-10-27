using UnityEngine;

namespace Assets.Scripts.Map
{
    public class KeyboardPlayerController : PlayerController
    {


        public bool LockTopDirection;
        public bool LockBottomDirection;
        public bool LockLeftDirection;
        public bool LockRightDirection;

        public bool ShakeOnWrongDirection = true;
        public float ShakeAmount = 0.3f;

        private void TryMove(Direction direction)
        {
            if ((direction == Direction.Bottom && LockBottomDirection)
                || (direction == Direction.Left && LockLeftDirection)
                || (direction == Direction.Top && LockTopDirection)
                || (direction == Direction.Right && LockRightDirection))
            {
                if(ShakeOnWrongDirection) Shake();
                return;
            }

            Player.Move(direction);
        }

        private void Shake()
        {
            iTween.ShakePosition(Level.LevelMap.gameObject, new Vector3(ShakeAmount, ShakeAmount, 0), 0.3f);
        }

        protected override void Update()
        {

            if (!CanControl)
                return;

            if (Input.GetKeyDown(KeyCode.W)) TryMove(Direction.Top);
            else if (Input.GetKeyDown(KeyCode.S)) TryMove(Direction.Bottom);
            else if (Input.GetKeyDown(KeyCode.A)) TryMove(Direction.Left);
            else if (Input.GetKeyDown(KeyCode.D)) TryMove(Direction.Right);
        }
    }
}
