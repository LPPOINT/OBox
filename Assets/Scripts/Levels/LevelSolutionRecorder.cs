using Assets.Scripts.Map;
using Assets.Scripts.Map.Items;
using UnityEngine;

namespace Assets.Scripts.Levels
{

    [ExecuteInEditMode]
    public class LevelSolutionRecorder : LevelElement
    {
        public LevelSolution Solution;

        private void Start()
        {

        }

        [LevelEventFilter(typeof(MapItem.MapItemMoveEvent), typeof(Player))]
        private void OnPlayerMove(MapItem.MapItemMoveEvent playerMove)
        {

                var move = playerMove.Move;
                if (move.Source == MoveSource.User)
                {
                    Solution.Nodes.Add(new LevelSolution.LevelSolutionNode(move.From,
                        move.Direction));
                }

        }


    }
}
