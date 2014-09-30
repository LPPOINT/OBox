using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

namespace Assets.Scripts.Meta.Stats
{
    public static class UserStats
    {


        private static string GetLevelBestStepsKey(int levelNumber)
        {
            return "Level" + levelNumber + "BestSteps";
        }

        private static string GetLevelStatusKey(int levelNumber)
        {
            return "Level" + levelNumber + "Staus";
        }

        public static LevelStatus GetLevelState(int levelNumber)
        {
            var key = GetLevelStatusKey(levelNumber);
            return (LevelStatus) PlayerPrefs.GetInt(key, (int) LevelStatus.NotCompleted);
        }

        public static void SetLevelState(int levelNumber, LevelStatus status)
        {
            var key = GetLevelStatusKey(levelNumber);
            PlayerPrefs.SetInt(key, (int)status);
        }

        public static int GetLevelBestSteps(int levelNumber)
        {
            return PlayerPrefs.GetInt(GetLevelBestStepsKey(levelNumber));
        }

        public static void SetLevelBestSteps(int levelNumber, int steps)
        {
            PlayerPrefs.SetInt(GetLevelBestStepsKey(levelNumber), steps);
        }

        public static int GetTipsCount()
        {
            return PlayerPrefs.GetInt("TipsCount");
        }

        public static void SetTipsCount(int count)
        {
            PlayerPrefs.SetInt("TipsCount", count);
          
        }

    }
}
