using System;
using Assets.Scripts.Levels;
using Assets.Scripts.Map;
using Assets.Scripts.Map.Items;
using UnityEngine;

namespace Assets.Scripts.Camera
{
    public class CameraPunch : LevelElement
    {

        public float PunchVelocity = 0.1f;
        public float PuchTime = 1f;

        protected override void OnPlayerMoveEnd(Player player, MapItemMove move)
        {


            Vector3 shakeVector;
            var len = move.MoveLenght;

            switch (move.Direction)
            {
                case Direction.Left:
                    shakeVector = new Vector3(-PunchVelocity * len, 0);
                    break;
                case Direction.Right:
                    shakeVector = new Vector3(PunchVelocity * len, 0);
                    break;
                case Direction.Top:
                    shakeVector = new Vector3(0, -PunchVelocity * len);
                    break;
                case Direction.Bottom:
                    shakeVector = new Vector3(0, PunchVelocity * len);
                    break;
                default:
                    shakeVector = Vector3.zero;
                    break;
            }


            iTween.PunchPosition(gameObject, shakeVector, PuchTime);
            
        }
    }
}
