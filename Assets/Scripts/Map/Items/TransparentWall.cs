
using System.Collections.Generic;
using Assets.Scripts.Map.Collision;
using UnityEngine;

namespace Assets.Scripts.Map.Items
{
    public class TransparentWall : Wall
    {
        public float TransparentAlpha = 0.1f;

        public bool IsTransparent { get; private set; }
        protected override void Start()
        {
            base.Start();
            IsTransparent = true;
            UpdateAlpha();
        }

        public override void OnDebugClick(IEnumerable<DebugClickModifers> modifers)
        {
            IsTransparent = !IsTransparent;
            UpdateAlpha();

            base.OnDebugClick(modifers);
        }

        private void UpdateAlpha()
        {
            var r = GetComponent<SpriteRenderer>();

            if (IsTransparent)
            {
                r.color = new Color(r.color.r, r.color.g, r.color.b, 0.3f);
            }
            else
            {
                r.color = new Color(r.color.r, r.color.g, r.color.b, 1);
            }
        }

        public override MapItemColliderType GetCollider(MapItem other)
        {
            return IsTransparent ? MapItemColliderType.MoveThrow : MapItemColliderType.StopNear;
        }

        protected override void OnCollisionExit2D(Collision2D collision)
        {


            if (collision.gameObject.GetComponent<Player>() != null)
            {


                var player = collision.gameObject.GetComponent<Player>();
                var playerMove = player.GetCurrentMove();

                if (playerMove == null)
                {
                    playerMove = player.GetLastMove();
                }


                if (playerMove != null && (player.Index.row == Index.row &&
                    (playerMove.Direction == Direction.Left || playerMove.Direction == Direction.Right))
                    ||(player.Index.column == Index.column &&
                    (playerMove.Direction == Direction.Top || playerMove.Direction == Direction.Bottom)))
                {
                    IsTransparent = false;
                    UpdateAlpha();
                }
            }
        }

        public override void OnLevelReset()
        {

            base.OnLevelReset();
            IsTransparent = true;
            UpdateAlpha();
        }
    }
}
