using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Levels;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Map.InteractiveUI
{
    public class MapItemsMover : LevelElement
    {
        public enum MoverOrientation
        {
            Horizontal,
            Vertical
        }

        public MoverOrientation Orientation;
        public Graphic UIBorder;
        public MapItemsGroup Group;

        public void Move(Vector2 offset)
        {
            if(Orientation == MoverOrientation.Horizontal) Group.Offset(new Vector2(offset.x, 0));
            else Group.Offset(new Vector2(0, offset.y));
        }


        public bool CanMove(Direction direction)
        {
            var edgeItems = Group.GetEdgeItems(direction);

            

            foreach (var edgeItem in edgeItems)
            {
                var wall = Level.LevelMap.GetFirstWallOfDirection(edgeItem, direction);
                if (wall == null)
                {
                    return false;
                }
            }
            return true;

        }


    }
}
