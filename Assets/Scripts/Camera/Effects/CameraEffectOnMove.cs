﻿using System;
using Assets.Scripts.Levels;
using Assets.Scripts.Map;
using Assets.Scripts.Map.Items;
using UnityEngine;

namespace Assets.Scripts.Camera.Effects
{
    public class CameraEffectOnMove : LevelElement
    {

        public float PunchVelocity = 0.1f;
        public float PuchTime = 1f;

        public bool BlurOnMove;
        public float BlurAmout = 0.3f;

        private bool hasPreviousPunch = false;
        private Vector3 startPunchPosition;

        protected override void OnPlayerMoveEnd(Player player, MapItemMove move)
        {


            if (hasPreviousPunch && transform.position != startPunchPosition)
            {
                transform.position = startPunchPosition;
            }

            startPunchPosition = transform.position;



            hasPreviousPunch = true;


            var blur = GetComponent<MotionBlur>();
            if (blur != null) Destroy(blur);

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

        protected override void OnPlayerMoveBegin(Player player, MapItemMove move)
        {


            if (BlurOnMove)
            {
                var blur = GetComponent<MotionBlur>();

                if (blur != null)
                {
                    Destroy(blur);
                }

                var newBlur = gameObject.AddComponent<MotionBlur>();
                newBlur.extraBlur = true;
                newBlur.blurAmount = BlurAmout;
                newBlur.shader = Shader.Find("Hidden/MotionBlur");
            }

            base.OnPlayerMoveBegin(player, move);
        }

        protected override void OnLevelReset()
        {


            var blur = GetComponent<MotionBlur>();
            if (blur != null) Destroy(blur);
            base.OnLevelReset();
        }
    }
}
