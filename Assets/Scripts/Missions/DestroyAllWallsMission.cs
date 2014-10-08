using System.Linq;
using Assets.Scripts.Levels;
using Assets.Scripts.Map.Items;
using UnityEngine;

namespace Assets.Scripts.Missions
{
    public class DestroyAllWallsMission : LevelMission
    {
        public int WallsCount { get; private set; }
        public int CurrentDestroyedWallsCount { get; private set; }

        public int RemainingWalls
        {
            get { return WallsCount - CurrentDestroyedWallsCount; }
        }

        protected override void OnLevelStarted()
        {
            CurrentDestroyedWallsCount = 0;
            WallsCount = Level.LevelMap.FindItemsOfType<DisposeableWall>().Count();
        }

        [LevelEventFilter(typeof(DisposeableWall.DisposeableWallDestroyedEvent))]
        public void OnWallDestroyed(LevelEvent e)
        {

            CurrentDestroyedWallsCount++;
            if (CurrentDestroyedWallsCount == WallsCount)
            {
                RegisterMissionDone();
            }
        }

    }
}
