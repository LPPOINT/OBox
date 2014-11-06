using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Levels;
using Assets.Scripts.Model.Constants;
using Assets.Scripts.Model.Numeration;
using Assets.Scripts.Model.Statuses;

namespace Assets.Scripts.Model.Storage
{
    public class RAMModelStorage : IModelStorage
    {

        private readonly Dictionary<LevelIndex, LevelStatus> levelStatuses = new Dictionary<LevelIndex, LevelStatus>();
        private readonly Dictionary<WorldNumber, WorldStatus> worldStatuses = new Dictionary<WorldNumber, WorldStatus>();
        private int currency = 0;
        private DateTime currentSessionTime;
        private readonly Dictionary<WorldNumber, int> currentLevels = new Dictionary<WorldNumber, int>();
        private WorldNumber currentWorld;
        private bool isAdsRemoved;

        private Dictionary<string, bool> purchases = new Dictionary<string, bool>(); 
        private Dictionary<CurrencyIncrementation.CurrencyIncrementationSource, DateTime> incrementationTimes = new Dictionary<CurrencyIncrementation.CurrencyIncrementationSource, DateTime>(); 

        public LevelStatus GetLevelStatus(int levelNumber, WorldNumber worldNumber)
        {
            try
            {
                return
                    levelStatuses.FirstOrDefault(
                        pair => pair.Key.LevelNumber == (LevelNumber) levelNumber && pair.Key.WorldNumber == worldNumber)
                        .Value;
            }
            catch 
            {
                return LevelStatus.NotCompleted;
            }
        }

        public void SetLevelStatus(int levelNumber, WorldNumber worldNumber, LevelStatus status)
        {
            var index = new LevelIndex(levelNumber, worldNumber);
            if (levelStatuses.ContainsKey(index))
            {
                levelStatuses[index] = status;
            }
            levelStatuses.Add(index, status);
        }

        public int GetGameCurrency()
        {
            return currency;
        }

        public void SetGameCurrency(int c)
        {
            currency = c;
        }

        public void SetAdsRemoveStatus(bool status)
        {
            isAdsRemoved = status;
        }

        public bool GetAdsRemoveStatus()
        {
            return isAdsRemoved;
        }

        public void SetPurchaseStatus(string purchaseId, bool status)
        {
            if (purchases.ContainsKey(purchaseId))
            {
                purchases[purchaseId] = status;
            }
            else
            {
                purchases.Add(purchaseId, status);
            }
        }

        public bool GetPurchaseStatus(string purchaseId)
        {
            bool value;
            if (purchases.TryGetValue(purchaseId, out value))
            {
                return value;
            }
            else
            {
                return false;
            }
        }

        public void SetLatestCurrencyIncrementationDate(DateTime date, CurrencyIncrementation.CurrencyIncrementationSource source)
        {
            if(incrementationTimes.ContainsKey(source))
                incrementationTimes[source] = date;
            else incrementationTimes.Add(source, date);
        }

        public DateTime? GetLatestCurrencyIncrementationDate(CurrencyIncrementation.CurrencyIncrementationSource source)
        {
            if (!incrementationTimes.ContainsKey(source)) return null;
            return incrementationTimes[source];
        }


        public void SetCurrentLevel(int level, WorldNumber targetWorldNumber)
        {
            if (!currentLevels.ContainsKey(targetWorldNumber))
            {
                currentLevels.Add(targetWorldNumber, level);
            }
            else
            {
                currentLevels[targetWorldNumber] = level;
            }
        }

        public int GetCurrentLevel(WorldNumber targetWorldNumber)
        {
            try
            {
                return (int) currentLevels[targetWorldNumber];
            }
            catch
            {
                return 1;
            }
        }

        public void SetCurrentWorld(WorldNumber worldNumber)
        {
            currentWorld = worldNumber;
        }

        public WorldNumber GetCurrentWorld()
        {
            return currentWorld;
        }

        public void SetWorldStatus(WorldNumber worldNumber, WorldStatus status)
        {
            if(worldStatuses.ContainsKey(worldNumber)) worldStatuses.Add(worldNumber, status);
            else worldStatuses.Add(worldNumber, status);
        }

        public WorldStatus GetWorldStatus(WorldNumber worldNumber)
        {
            try
            {
                return worldStatuses[worldNumber];
            }
            catch 
            {
                return WorldStatus.Locked;
            }
        }

        public void Save()
        {
            
        }

        public void Clear()
        {
            levelStatuses.Clear();
            worldStatuses.Clear();
            currentWorld = WorldNumber.World1;
            currentLevels.Clear();
        }

        public void RegisterGameSession(DateTime sessionStartTime)
        {
            currentSessionTime = sessionStartTime;
        }

        public void RegisterGameSession()
        {
            RegisterGameSession(DateTime.Now);
        }

        public void UnregisterAllSessions()
        {
            
        }

        public DateTime? GetCurrentGameSessionTime()
        {
            return currentSessionTime;
        }

        public DateTime? GetLatestGameSessionTime()
        {
            return null;
        }

        public bool IsCurrentGameSessionExist { get { return currentSessionTime != null; } }
        public bool IsLatestGameSessionExist { get { return false; } }
    }
}