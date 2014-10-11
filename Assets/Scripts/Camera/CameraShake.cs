using System;
using Assets.Scripts.Levels;
using Assets.Scripts.Map;
using Assets.Scripts.Map.Items;
using UnityEngine;

namespace Assets.Scripts.Camera
{
    public class CameraShake : LevelElement
    {

        public float ShakeOffset = 1;
        public float ShakeTime = 0.6f;
        public int MinMoveLenght = 0;

        protected override void OnPlayerMoveEnd(Player player, MapItemMove move)
        {

           if(move.MoveLenght < MinMoveLenght) return;

            Vector3 shakeVector;

            switch (move.Direction)
            {
                case Direction.Left:
                    shakeVector = new Vector3(-ShakeOffset, 0);
                    break;
                case Direction.Right:
                    shakeVector = new Vector3(ShakeOffset, 0);
                    break;
                case Direction.Top:
                    shakeVector = new Vector3(0, -ShakeOffset);
                    break;
                case Direction.Bottom:
                    shakeVector = new Vector3(0, ShakeOffset);
                    break;
                default:
                    shakeVector = Vector3.zero;
                    break;
            }


            iTween.PunchPosition(gameObject, shakeVector, ShakeTime);
            
        }
    }
}
