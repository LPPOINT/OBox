using UnityEngine;

namespace Assets.Scripts.Model
{
    public static class ModelStorage
    {


        private static string GetLevelStatusKey(int levelNumber, WorldNumber worldNumber)
        {
            return "Level" + levelNumber + "W" + (int)worldNumber + "Staus";
        }

        private static string GetWorldStatusKey(WorldNumber worldNumber)
        {
            return "World" + (int) worldNumber + "Status";
        }

        private static string GetCurrentLevelKey( WorldNumber worldNumber)
        {
            return "CurrentLevelFor" + (int) worldNumber;
        }

        private static string GetCurrentWorldKey()
        {
            return "CurrentWorld"; 
        }

        public static LevelStatus GetLevelStatus(int levelNumber, WorldNumber worldNumber)
        {
            var key = GetLevelStatusKey(levelNumber, worldNumber);
            if(!PlayerPrefs.HasKey(key))
                return LevelStatus.NotCompleted;
            return (LevelStatus) PlayerPrefs.GetInt(key, (int) LevelStatus.NotCompleted);
        }

        public static void SetLevelStatus(int levelNumber, WorldNumber worldNumber, LevelStatus status)
        {
            var key = GetLevelStatusKey(levelNumber, worldNumber);
            PlayerPrefs.SetInt(key, (int)status);
        }

        public static int GetSkipsCount()
        {
            return PlayerPrefs.GetInt("Skips");
        }

        public static void SetSkipsCount(int count)
        {
            PlayerPrefs.SetInt("Skips", count);
          
        }

        public static void SetCurrentLevel(int level, WorldNumber targetWorldNumber)
        {
            var key = GetCurrentLevelKey(targetWorldNumber);
            PlayerPrefs.SetInt(key, level);
        }

        public static int GetCurrentLevel(WorldNumber targetWorldNumber)
        {
            return PlayerPrefs.GetInt(GetCurrentLevelKey(targetWorldNumber));
        }

        public static void SetCurrentWorld(WorldNumber worldNumber)
        {
            PlayerPrefs.SetInt(GetCurrentWorldKey(), (int)worldNumber);
        }

        public static WorldNumber GetCurrentWorld()
        {
            return (WorldNumber) PlayerPrefs.GetInt(GetCurrentWorldKey());
        }

        public static void SetWorldStatus(WorldNumber worldNumber, WorldStatus status)
        {
            PlayerPrefs.SetInt(GetWorldStatusKey(worldNumber), (int)status);
        }

        public static WorldStatus GetWorldStatus(WorldNumber worldNumber)
        {
            var key = GetWorldStatusKey(worldNumber);
            if(!PlayerPrefs.HasKey(key))
                return WorldStatus.Locked;
            return (WorldStatus)PlayerPrefs.GetInt(key);
        }

        public static void Save()
        {
            PlayerPrefs.Save();
        }
    }
}
