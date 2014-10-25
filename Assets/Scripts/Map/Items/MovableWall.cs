using Assets.Scripts.Map.Collision;
using Rotorz.Tile;
using UnityEngine;

namespace Assets.Scripts.Map.Items
{
    public class MovableWall : Wall
    {
        public bool CanMoveTo(Direction direction)
        {
            var next = GameMap.GetNextItem(this, direction);
            return next == null;
        }

        public SpriteRenderer TopArrow;
        public SpriteRenderer BottomArrow;
        public SpriteRenderer RightArrow;
        public SpriteRenderer LeftArrow;

        public bool LockInputWhileMoving = true;

        private void UpdateArrow(SpriteRenderer arrowRenderer, Direction arrowDirection)
        {
            if (CanMoveTo(arrowDirection) && GameMap.GetNextItem(this, GameMap.SwapDirection(arrowDirection)) == null) arrowRenderer.enabled = true;
            else arrowRenderer.enabled = false;
        }

        private void UpdateArrows()
        {
           UpdateArrow(TopArrow, Direction.Top);
           UpdateArrow(BottomArrow, Direction.Bottom);
           UpdateArrow(RightArrow, Direction.Right);
           UpdateArrow(LeftArrow, Direction.Left);
        }

        private void Move(Direction direction)
        {
            if(LockInputWhileMoving) Level.LockInput();
            var wall = GameMap.GetFirstWallOfDirection(this, direction);
            if (wall != null)
            {

                var newIndex = GameMap.GetItemStop(this, wall);

                Move(newIndex, 30);

                if (newIndex == Index)
                {
                    GameMap.ProcessCollisionEnter(this);
                }

            }
            else
            {
                MoveOutside(direction);
            }
        }

        public override void OnMoveDone(MapItemMove move)
        {
            base.OnMoveDone(move);
            if(LockInputWhileMoving) Level.Play();
            UpdateArrows();
        }

        public override void OnItemCollisionEnter(MapItemCollisionType collisionType, MapItem other)
        {
            if (other is Player)
            {
                var player = other as Player;
                var direction = player.GetLastMove().Direction;
                if (CanMoveTo(direction))
                {
                    Move(direction);
                }
            }
        }

        private TileIndex startIndex;

        protected override void OnLevelInitialized()
        {
            UpdateArrows();
        }

        protected override void Start()
        {

            base.Start();
            startIndex = TileSystem.ClosestTileIndexFromWorld(transform.position);
        }

        protected override void OnLevelStarted()
        {
            base.OnLevelStarted();
            SetIndex(startIndex);
            UpdateArrows();
        }
    }
}
