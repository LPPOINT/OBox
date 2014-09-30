using System;
using Assets.Scripts.Map.Collision;
using UnityEngine;

namespace Assets.Scripts.Map.Items
{
    public class Turn : MapItem
    {

        public enum TurnAction
        {
            TurnLeft,
            TurnRight,
            TurnTop,
            TurnBottom,
            DoNothing
        }

        public TurnAction PlayerLeft;
        public TurnAction PlayerRight;
        public TurnAction PlayerTop;
        public TurnAction PlayerBottom;

        public TurnAction GetActionByPlayerDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return PlayerLeft;
                case Direction.Right:
                    return PlayerRight;
                case Direction.Top:
                    return PlayerTop;
                case Direction.Bottom:
                    return PlayerBottom;
                default:
                    return PlayerLeft;
            }
        }


        public override MapItemColliderType GetCollider(MapItem other)
        {
            if (other is Player)
            {
                var direction = GameMap.CalculateDirection(Index, other.Index);
                var action = GetActionByPlayerDirection(direction);

                return action != TurnAction.DoNothing ? MapItemColliderType.GoInside : MapItemColliderType.StopNear;
            }
            return MapItemColliderType.StopNear;
        }

        public override void OnItemCollisionEnter(MapItemCollisionType collisionType, MapItem item)
        {
            if (collisionType == MapItemCollisionType.Inside && item is Player)
            {
                var newDirection = Direction.Left;
                var oldDirection = item.GetLastMove().Direction;
                var shouldActivate = true;

                var action = GetActionByPlayerDirection(GameMap.SwapDirection(oldDirection));

                switch (action)
                {
                    case TurnAction.TurnLeft:
                        newDirection = Direction.Left;
                        break;
                    case TurnAction.TurnRight:
                        newDirection = Direction.Right;
                        break;
                    case TurnAction.TurnTop:
                        newDirection = Direction.Top;
                        break;
                    case TurnAction.TurnBottom:
                        newDirection = Direction.Bottom;
                        break;
                    default:
                        shouldActivate = false;
                        break;
                }


                if (!shouldActivate)
                {
                    Debug.Log("DirectionChanger.OnItemEnter(): shouldActivate = false");
                    return;
                }

                (item as Player).Move(newDirection, MoveSource.DirectionChanger);
            }
        }
    }
}
