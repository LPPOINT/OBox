using System;

namespace Assets.Scripts.Map.Collision
{
    public class MapItemCollisionHelper
    {
        public bool IsExtendsCollision(MapItemCollisionType collision)
        {
            return collision == MapItemCollisionType.ExtendsBottom
                   || collision == MapItemCollisionType.ExtendsLeft
                   || collision == MapItemCollisionType.ExtendsRight
                   || collision == MapItemCollisionType.ExtendsThrough
                   || collision == MapItemCollisionType.ExtendsTop;
        }

        public bool IsTouchCollision(MapItemCollisionType collision)
        {
            return collision == MapItemCollisionType.TouchBottom
                   || collision == MapItemCollisionType.TouchLeft
                   || collision == MapItemCollisionType.TouchRight
                   || collision == MapItemCollisionType.TouchTop;
        }

        public Direction GetCollisionDirection(MapItemCollisionType collision)
        {

            if(collision == MapItemCollisionType.ExtendsBottom || collision == MapItemCollisionType.TouchBottom) return Direction.Bottom;
            if (collision == MapItemCollisionType.ExtendsTop || collision == MapItemCollisionType.TouchTop) return Direction.Top;
            if (collision == MapItemCollisionType.ExtendsLeft || collision == MapItemCollisionType.TouchLeft) return Direction.Left;
            if (collision == MapItemCollisionType.ExtendsRight || collision == MapItemCollisionType.TouchRight) return Direction.Right;

            throw new InvalidOperationException();
        }
    }
}
