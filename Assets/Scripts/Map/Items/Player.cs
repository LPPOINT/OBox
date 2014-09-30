
using System;
using Assets.Scripts.Levels;
using Assets.Scripts.Map.Collision;
using Rotorz.Tile;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Map.Items
{
    public class Player : MapItem
    {



        private Direction lastDirection;

        public Direction GetLastDirection()
        {
            return lastDirection;
        }

        public float Speed;
        public ParticleSystem Shape;
        public bool EnableShape;

        private TileIndex startIndex;

        public event EventHandler MoveStarted;
        public event EventHandler MoveDone;

        protected override void Start()
        {
            base.Start();
            startIndex = TileSystem.ClosestTileIndexFromWorld(transform.position);
            if (!EnableShape)
            {
                Shape.gameObject.SetActive(false);
            }
        }

        protected override void Update()
        {



        }

        public override void OnMoveStart(MapItemMove move)
        {
            base.OnMoveStart(move);
            Level.Current.RegesterPlayerMoveBegin(move);
            if (EnableShape)
            {
            var newShapePos = Vector3.zero;
            var offset = 5f;

            switch (move.Direction)
            {
                case Direction.Left:
                    newShapePos = new Vector3(offset, 0, Shape.transform.localPosition.z);
                    break;
                case Direction.Right:
                    newShapePos = new Vector3(-offset, 0, Shape.transform.localPosition.z);
                    break;
                case Direction.Top:
                    newShapePos = new Vector3(0, -offset, Shape.transform.localPosition.z);
                    break;
                case Direction.Bottom:
                    newShapePos = new Vector3(0, offset, Shape.transform.localPosition.z);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

                Shape.transform.localPosition = newShapePos;
                Shape.Play();
            }

            var handler = MoveStarted;
            if (handler != null) handler(this, EventArgs.Empty);

        }

        public override void OnMoveDone(MapItemMove move)
        {
            var currentLevel = Level.Current;
            Level.Current.RegisterPlayerMoveEnd(move);
            if (currentLevel == null)
            {
                Debug.LogWarning("OnMoveDone(): cant register step: current level not found.");
                return;
            }

            if (move is ToCellItemMove)
            {
            }
            else if (move is OutsideItemMove)
            {
                currentLevel.RegisterPlayerOutside();
            }

            if (EnableShape)
            {
                Shape.transform.localPosition = new Vector3(0, 0, Shape.transform.localPosition.z);
                Shape.Stop();
            }

            var handler = MoveDone;
            if (handler != null) handler(this, EventArgs.Empty);

        }

        public override void OnLevelReset()
        {
            base.OnLevelReset();
            SetIndex(startIndex);
        }

        public void Move(Direction direction, MoveSource source = MoveSource.User)
        {

            if (IsMoving)
                return;


            lastDirection = direction;
            var wall = GameMap.GetFirstWallOfDirection(this, direction);
            if (wall != null)
            {
                var newIndex = wall.GetCollider(this) != MapItemColliderType.GoInside ? GameMap.GetItemStop(this, wall) : wall.Index;


                var move = Move(newIndex, Speed, source);

                if (newIndex != Index)
                {
                    Level.Current.RegisterPlayerStep();

                }
                else if(newIndex == Index)
                {

                    GameMap.ProcessCollisionEnter(this);

                }

            }
            else
            {
                MoveOutside(direction, source);
            }
        }

    }
}
