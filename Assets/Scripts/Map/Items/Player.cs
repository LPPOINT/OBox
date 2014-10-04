
using System;
using Assets.Scripts.Levels;
using Assets.Scripts.Map.Collision;
using Rotorz.Tile;
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

        private TileIndex startIndex;

        public class PlayerOutsideEvent : LevelEvent
        {
            public PlayerOutsideEvent(OutsideItemMove move)
            {
                Move = move;
            }

            public OutsideItemMove Move { get; private set; }
        }
        public class PlayerStepEvent : LevelEvent
        {
            
        }

        protected override void Start()
        {
            base.Start();
            startIndex = TileSystem.ClosestTileIndexFromWorld(transform.position);
        }

        protected override void Update()
        {



        }

        public override void OnMoveDone(MapItemMove move)
        {
            var currentLevel = Level.Current;
            if (currentLevel == null)
            {
                Debug.LogWarning("OnMoveDone(): cant register step: current level not found.");
                return;
            }

            if (move is ToCellItemMove)
            {
                base.OnMoveDone(move);
            }
            else if (move is OutsideItemMove)
            {
                FireEvent(new PlayerOutsideEvent(move as OutsideItemMove));
            }



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
                    FireEvent(new PlayerStepEvent());

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
