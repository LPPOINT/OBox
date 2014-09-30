using System;
using System.Collections.Generic;
using Assets.Scripts.Map;
using Rotorz.Tile;
using UnityEngine;

namespace Assets.Scripts.Levels
{

    [ExecuteInEditMode]
    public class LevelSolution : MonoBehaviour
    {

        [Serializable]
        public class LevelSolutionNode
        {

            public LevelSolutionNode()
            {
                
            }

            public LevelSolutionNode(TileIndex index, Direction direction)
            {
                Column = index.column;
                Row = index.row;
                Direction = direction;
            }

            public TileIndex Index
            {
                get
                {
                    return new TileIndex(Row, Column);
                }
            }
            public int Column;
            public int Row;
            public Direction Direction;
        }


        public List<LevelSolutionNode> Nodes;

    }
}
