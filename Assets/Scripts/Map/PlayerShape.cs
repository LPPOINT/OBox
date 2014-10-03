using System;
using System.Collections;
using Assets.Scripts.Levels;
using Assets.Scripts.Map.Items;
using UnityEngine;

namespace Assets.Scripts.Map
{
    public class PlayerShape : LevelElement
    {

        private ParticleSystem shape;
        public int EnableShapeMinMoveLenght = 3;

        private void Start()
        {
            shape = GetComponent<ParticleSystem>();
            shape.Stop();
        }


        private Vector2 GetShapeOriginByPlayerDirection(Direction direction)
        {
            switch (GameMap.SwapDirection(direction))
            {
                case Direction.Left:
                    return new Vector2(-2.5f, 0);
                case Direction.Right:
                    return new Vector2(2.5f, 0);
                case Direction.Top:
                    return new Vector2(0, 2.5f);
                case Direction.Bottom:
                    return new Vector2(0, -2.5f);

            }
            return new Vector2(0, 0);
        }

        public override void OnLevelEvent(LevelEvent e)
        {
            if (e is Player.PlayerOutsideEvent)
            {
                shape.enableEmission = false;
            }
        }

        public override void OnPlayerMoveBegin(Player player, MapItemMove move)
        {
            base.OnPlayerMoveBegin(player, move);

            var moveLen = move.MoveLenght;

            if (moveLen >= EnableShapeMinMoveLenght)
            {
                var origin = GetShapeOriginByPlayerDirection(move.Direction);
                gameObject.transform.localPosition = new Vector3(origin.x, origin.y, gameObject.transform.localPosition.z);
                shape.enableEmission = true;
                shape.Play();

            }

        }


        private IEnumerator StopShape()
        {
            yield return new WaitForSeconds(0.3f);
            shape.Stop();

        }

        public override void OnPlayerMoveEnd(Player player, MapItemMove move)
        {
            base.OnPlayerMoveEnd(player, move);
            //StartCoroutine(StopShape());
            shape.enableEmission = false;
        }

        public override void OnLevelStateChanged(LevelState oldState, LevelState newState)
        {
            base.OnLevelStateChanged(oldState, newState);
        }
    }
}
