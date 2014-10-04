using System;
using System.Diagnostics;
using Assets.Scripts.Map;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.Levels
{
    public class BySolutionPlayerController : PlayerController
    {
        public int CurrentNodeIndex { get; private set; }


        private bool isDone;
        public LevelSolution Solution;

        private bool isStarted;

        protected override void Update()
        {
            if (!isStarted)
            {
                isStarted = true;
                FollowSolution();
            }
        }

        private void FollowSolution()
        {

            if (CurrentNodeIndex > Solution.Nodes.Count - 1)
            {
                isDone = true;
                return;
            }

            var node = Solution.Nodes[CurrentNodeIndex];
            Player.Move(node.Direction);
        }

        private void Start()
        {
        }

        public override void OnLevelReset()
        {
            CurrentNodeIndex = 0;
            isDone = false;
            isStarted = false;
        }


        public override void OnLevelEvent(LevelEvent e)
        {
            if (e is MapItem.MapItemMoveEvent && e.IsPlayer)
            {
                if (isDone)
                    return;
                CurrentNodeIndex++;

                if (CurrentNodeIndex > Solution.Nodes.Count - 1)
                {
                    isDone = true;
                    return;
                }


                var node = Solution.Nodes[CurrentNodeIndex];

                if (Player.Index != node.Index)
                {
                    CurrentNodeIndex--;
                    return;
                }

                FollowSolution();
            }
        }
    }
}
