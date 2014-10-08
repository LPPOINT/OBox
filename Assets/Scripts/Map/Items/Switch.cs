using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Levels;
using Assets.Scripts.Map.Collision;
using UnityEngine;

namespace Assets.Scripts.Map.Items
{
    public class Switch : MapItem
    {

        private const string horizontalIdle = "SwitchHIdle";
        private const string verticalIdle = "SwitchVIdle";

        private const string horizontalToVertical = "SwitchHToV";
        private const string verticalToHorizontal = "SwitchVToH";


        public class SwitchEvent : LevelEvent
        {
            public SwitchEvent(MapItem activator, SwitchPosition newPosition)
            {
                NewPosition = newPosition;
                OldPosition = newPosition == SwitchPosition.Horizontal ? SwitchPosition.Vertical : SwitchPosition.Horizontal;
                Activator = activator;
            }

            public MapItem Activator { get; private set; }

            public SwitchPosition OldPosition { get; private set; }
            public SwitchPosition NewPosition { get; private set; }
        }

        public override MapItemCollisionDetectionMode CollisionDetectionMode
        {
            get { return MapItemCollisionDetectionMode.AllTime; }
        }

        protected override void Start()
        {
            base.Start();
            ResetPosition();
        }

        private void ResetPosition()
        {
            if (StartPosition == SwitchPosition.Horizontal)
            {
                GetComponent<Animator>().Play(horizontalIdle);
            }
            else
            {
                GetComponent<Animator>().Play(verticalIdle);
            }
            CurrentPosition = StartPosition;
        }

        public enum SwitchPosition
        {
            Horizontal,
            Vertical
        }

        public SwitchPosition StartPosition;

        public SwitchPosition CurrentPosition { get; private set; }

        public override void OnDebugClick(IEnumerable<DebugClickModifers> modifers)
        {
            base.OnDebugClick(modifers);
            if (!modifers.Any())
            {
                SwapPosition();
            }
        }

        protected override void OnCollisionExit2D(Collision2D collision)
        {

            if (collision.gameObject.GetComponent<Player>() != null)
            {
                var player = collision.gameObject.GetComponent<Player>();


                if (CurrentPosition == SwitchPosition.Horizontal && player.Index.row == Index.row
                    || CurrentPosition == SwitchPosition.Vertical && player.Index.column == Index.column)
                {
                    SwapPosition(player);
                }

            }
        }

        public void ApplyStartPosition()
        {


        }

        private void SwapPosition(MapItem activator = null)
        {

            var animator = GetComponent<Animator>();

            if (CurrentPosition == SwitchPosition.Horizontal)
            {
                animator.Play(horizontalToVertical);
            }
            else if (CurrentPosition == SwitchPosition.Vertical)
            {
                animator.Play(verticalToHorizontal);
            }

            if(CurrentPosition == SwitchPosition.Horizontal) CurrentPosition = SwitchPosition.Vertical;
            else if(CurrentPosition == SwitchPosition.Vertical) CurrentPosition = SwitchPosition.Horizontal;

            FireEvent(new SwitchEvent(activator, CurrentPosition));


        }
        public override MapItemColliderType GetCollider(MapItem other)
        {
            if (!(other is Player))
            {
                return MapItemColliderType.StopNear;
            }
            var player = other as Player;
            var playerDirection = player.GetLastDirection();

            if((CurrentPosition == SwitchPosition.Horizontal && (playerDirection == Direction.Left || playerDirection == Direction.Right))
                || (CurrentPosition == SwitchPosition.Vertical && (playerDirection == Direction.Top || playerDirection == Direction.Bottom)))
            {
                return MapItemColliderType.MoveThrow;
            }

            return MapItemColliderType.StopNear;

        }

        protected override void OnLevelReset()
        {
            base.OnLevelReset();
            ResetPosition();
        }
    }
}
