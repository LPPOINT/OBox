using System;
using Assets.Scripts.Model.Numeration;
using Assets.Scripts.Model.Statuses;
using UnityEngine;

namespace Assets.Scripts.Model.Storage
{
    public  class PlayerPrefsStorage : IModelStorage
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

        private static string GetCurrentGameSessionKey()
        {
            return "CurrentGameSession";
        }

        private static string GetLatestGameSessionKey()
        {
            return "LatestGameSession";
        }

        public  LevelStatus GetLevelStatus(int levelNumber, WorldNumber worldNumber)
        {
            var key = GetLevelStatusKey(levelNumber, worldNumber);
            if(!PlayerPrefs.HasKey(key))
                return LevelStatus.NotCompleted;
            return (LevelStatus) PlayerPrefs.GetInt(key, (int) LevelStatus.NotCompleted);
        }

        public  void SetLevelStatus(int levelNumber, WorldNumber worldNumber, LevelStatus status)
        {
            var key = GetLevelStatusKey(levelNumber, worldNumber);
            PlayerPrefs.SetInt(key, (int)status);
        }

        public  int GetSkipsCount()
        {
            return PlayerPrefs.GetInt("Skips");
        }

        public  void SetSkipsCount(int count)
        {
            PlayerPrefs.SetInt("Skips", count);
          
        }

        public  void SetCurrentLevel(int level, WorldNumber targetWorldNumber)
        {
            var key = GetCurrentLevelKey(targetWorldNumber);
            PlayerPrefs.SetInt(key, level);
        }

        public  int GetCurrentLevel(WorldNumber targetWorldNumber)
        {
            return PlayerPrefs.GetInt(GetCurrentLevelKey(targetWorldNumber));
        }

        public  void SetCurrentWorld(WorldNumber worldNumber)
        {
            PlayerPrefs.SetInt(GetCurrentWorldKey(), (int)worldNumber);
        }

        public  WorldNumber GetCurrentWorld()
        {
            return (WorldNumber) PlayerPrefs.GetInt(GetCurrentWorldKey());
        }

        public  void SetWorldStatus(WorldNumber worldNumber, WorldStatus status)
        {
            PlayerPrefs.SetInt(GetWorldStatusKey(worldNumber), (int)status);
        }

        public  WorldStatus GetWorldStatus(WorldNumber worldNumber)
        {
            var key = GetWorldStatusKey(worldNumber);
            if(!PlayerPrefs.HasKey(key))
                return WorldStatus.Locked;
            return (WorldStatus)PlayerPrefs.GetInt(key);
        }

        public  void Save()
        {
            PlayerPrefs.Save();
        }

        public  void RegisterGameSession(DateTime sessionStartTime)
        {

            if (IsCurrentGameSessionExist)
            {
                Debug.Log("Latest game session was found (" + GetCurrentGameSessionTime() + ")");
                var currentGameSessionTime = GetCurrentGameSessionTime();
                if (currentGameSessionTime != null)
                    PlayerPrefs.SetFloat(GetLatestGameSessionKey(), currentGameSessionTime.Value.ToBinary());
            }

            PlayerPrefs.SetFloat(GetCurrentGameSessionKey(), sessionStartTime.ToBinary());
        }
        public  void RegisterGameSession()
        {
            RegisterGameSession(DateTime.Now);
        }

        public  void UnregisterAllSessions()
        {
            PlayerPrefs.DeleteKey(GetLatestGameSessionKey());
            PlayerPrefs.DeleteKey(GetCurrentGameSessionKey());
        }

        private  DateTime? GetSessionTime(bool isSessionExist, string sessionKey)
        {
            if (!isSessionExist)
            {
                return null;
            }
            return DateTime.FromBinary((long)PlayerPrefs.GetFloat(sessionKey));
        }

        public  DateTime? GetCurrentGameSessionTime()
        {
            return GetSessionTime(IsCurrentGameSessionExist, GetCurrentGameSessionKey());
        }

        public  DateTime? GetLatestGameSessionTime()
        {
            return GetSessionTime(IsLatestGameSessionExist, GetLatestGameSessionKey());
        }
        public  bool IsCurrentGameSessionExist
        {
            get { return PlayerPrefs.HasKey(GetCurrentGameSessionKey()); }
        }
        public  bool IsLatestGameSessionExist
        {
            get { return PlayerPrefs.HasKey(GetLatestGameSessionKey()); }
        }

        public  void Clear()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
