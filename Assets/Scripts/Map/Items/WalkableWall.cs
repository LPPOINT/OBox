using Assets.Scripts.Map.Collision;
using Assets.Scripts.Map.Serialization;
using UnityEngine;

namespace Assets.Scripts.Map.Items
{
    public class WalkableWall : Wall
    {

        public bool DestroyAfterLeave;

        public bool IsDestroyed { get; private set; }

        public override MapItemColliderType GetCollider(MapItem other)
        {
            return IsDestroyed ? MapItemColliderType.MoveThrow : MapItemColliderType.GoInside;
        }

        private bool isShowed = false;

        protected override void Update()
        {
            base.Update();

        }

        private void LateUpdate()
        {
            if (!isShowed)
            {
                isShowed = true;
                IsDestroyed = false;
                GetComponent<Animator>().Play("WalkableWallShowing");
            }
        }

        protected override void Start()
        {
            base.Start();
            IsDestroyed = false;
        }

        public void Destroy()
        {
            IsDestroyed = true;
            GetComponent<Animator>().Play("WalkableWallDisposing");
        }


        public override void OnItemCollisionLeave(MapItemCollisionType collisionType, MapItem other)
        {
            base.OnItemCollisionLeave(collisionType, other);
            if (collisionType == MapItemCollisionType.Inside && DestroyAfterLeave)
            {
                Destroy();
            }
        }

        public override void OnLevelReset()
        {
            base.OnLevelReset();
            IsDestroyed = false;
            GetComponent<Animator>().Play("WalkableWallShowing");
        }
    }
}
