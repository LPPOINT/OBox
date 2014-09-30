using Assets.Scripts.Map;
using Assets.Scripts.Map.Items;
using UnityEngine;

namespace Assets.Scripts.Levels
{

    [ExecuteInEditMode]
    public class LevelSolutionRecorder : MonoBehaviour
    {
        public LevelSolution Solution;
        public Player Player;

        private void Start()
        {
            Player.MoveStarted += (sender, args) =>
                                  {
                                      var move = Player.GetCurrentMove();
                                      if (move.Source == MoveSource.User)
                                      {
                                          Solution.Nodes.Add(new LevelSolution.LevelSolutionNode(move.From,
                                              move.Direction));
                                      }
                                  };
        }

    }
}
