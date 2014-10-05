using Assets.Scripts.Map.Collision;
using UnityEngine;

namespace Assets.Scripts.Map.Items
{
    public class WalkableWall : Wall
    {
        public bool DestroyAfterLeave;

        private const string ShowingAnimation = "WalkableWallShowing";
        private const string IdleAnimation = "WalkableWallIdle";
        private const string DisposingAnimation = "WalkableWallDisposing";

        public bool IsDisposed { get; private set; }

        private Animator animator;


        protected override void Start()
        {
            base.Start();
            animator = GetComponent<Animator>();
        }

        public override MapItemColliderType GetCollider(MapItem other)
        {
            return IsDisposed ? MapItemColliderType.MoveThrow : MapItemColliderType.GoInside;
        }

        public override void OnItemCollisionLeave(MapItemCollisionType collisionType, MapItem other)
        {
            if (collisionType == MapItemCollisionType.Inside && DestroyAfterLeave && !IsDisposed)
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            if (IsDisposed)
            {
                Debug.LogWarning("WalkableWall: Wall already disposed");
                return;
            }
            IsDisposed = true;
            animator.Play(DisposingAnimation);
        }

        public override void OnLevelStarted()
        {
            IsDisposed = false;
            animator.Play(IdleAnimation);
        }
    }
}
