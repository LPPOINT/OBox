using System;
using Rotorz.Tile;

namespace Assets.Scripts.Map
{
    public abstract class MapItemMove
    {
        protected MapItemMove()
        {
            Speed = 40;
            EaseType = iTween.EaseType.easeInCirc;
        }

        public MoveSource Source { get; set; }

        public MapItem Item { get; set; }
        public TileIndex From { get; set; }

        public float Speed { get; set; }
        public iTween.EaseType EaseType { get; set; }

        public Direction Direction { get; set; }


        public static ToCellItemMove ToCell(MapItem item, TileIndex to)
        {
            var move = new ToCellItemMove();
            move.Item = item;
            move.From = item.Index;
            move.To = to;
            move.Direction = item.GameMap.CalculateDirection(move.From, move.To);
            move.Speed = 40;
            move.EaseType = iTween.EaseType.easeInCirc;

            return move;
        }

        public static OutsideItemMove Outside(MapItem item, Direction direction)
        {
            var move = new OutsideItemMove();
            move.Item = item;
            move.From = item.Index;
            move.Direction = direction;
            move.Speed = 40;
            move.EaseType = iTween.EaseType.easeInCirc;
            move.TimeInOutside = new TimeSpan(0, 0, 0, 0, 200);

            return move;
        }

    }

    public class ToCellItemMove : MapItemMove
    {
        public TileIndex To { get; set; }
    }

    public class OutsideItemMove : MapItemMove
    {
        public TimeSpan TimeInOutside { get; set; }
    }

    public class EmptyItemMove : MapItemMove
    {
        public EmptyItemMove(ToCellItemMove basedOn)
        {
            Direction = basedOn.Direction;
            Source = basedOn.Source;
            Speed = basedOn.Speed;
            Item = basedOn.Item;
        }
    }

}
