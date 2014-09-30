using Assets.Scripts.Levels;
using Assets.Scripts.Map.Collision;

namespace Assets.Scripts.Map.Items
{
    public class Target : MapItem
    {

        public override MapItemColliderType GetCollider(MapItem other)
        {
            return MapItemColliderType.GoInside;
        }

        public override void OnItemCollisionEnter(MapItemCollisionType collisionType, MapItem other)
        {
            if (collisionType == MapItemCollisionType.Inside)
            {
                Level.Current.EndLevel();
            }
        }
    }
}
