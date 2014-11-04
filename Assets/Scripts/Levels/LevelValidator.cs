using System.Linq;
using Assets.Scripts.Missions;
using UnityEngine;

namespace Assets.Scripts.Levels
{
    public static class LevelValidator
    {

        public static void Validate(Level level)
        {

            ValidateMission(level);
            ValidateSteps(level);
            ValidateLevelIndex(level);
        }

        private static void ValidateLevel(Level level)
        {
#if UNITY_EDITOR


            if (level.LevelMap == null)
            {
                Debug.LogWarning("Level.Map == null");
            }

#endif
        }
        private static void ValidateSteps(Level level)
        {

        }

        private static void ValidateLevelIndex(Level level)
        {
            if (level.Index.LevelNumber == 0)
            {
                Debug.LogWarning("Levent number or level world not found");
            }
        }

        private static void ValidateMission(Level level)
        {

            if (level.Mission == null)
            {

                Debug.LogWarning("ValidateMission(): Mission for level not found. Initializing mission by temp value (" + level.Mission.GetType().Name + ")");
            }

            if (level.Mission is DestroyAllWallsMission && level.LevelMap.FindItemsOfType<Map.Items.Target>().Any())
            {
                Debug.LogWarning("ValidateMission(): Unexpected target found.");
            }
            else if (level.Mission is EnterTargetMission && !level.LevelMap.FindItemsOfType<Map.Items.Target>().Any())
            {
                Debug.LogWarning("ValidateMission(): For EnterTargetMission target not found");
            }
        }
    }
}
