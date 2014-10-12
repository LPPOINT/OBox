using Assets.Scripts.Map.Collision;
using UnityEngine;

namespace Assets.Scripts.Map.Items
{
    public class Sphere : MapItem
    {
        protected override void Start()
        {
            base.Start();
            IsDisposed = false;
            ps = GetComponent<ParticleSystem>();
        }

        private bool IsDisposed;
        private ParticleSystem ps;

        public override void OnItemCollisionEnter(MapItemCollisionType collisionType, MapItem other)
        {
            if (other is Player)
            {
                IsDisposed = true;
                ps.Stop();
            }
        }

        public override MapItemColliderType GetCollider(MapItem other)
        {
            return MapItemColliderType.MoveThrow;
        }

        protected override void OnLevelReset()
        {

            IsDisposed = false;
            ps.Play();

            base.OnLevelReset();
        }
    }
}
