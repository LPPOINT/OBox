using System;
using System.Collections.Generic;
using Assets.Scripts.GameGUI.Shop;
using Assets.Scripts.Levels;
using Assets.Scripts.Model.Constants;
using Assets.Scripts.Model.Numeration;
using Assets.Scripts.Model.Statuses;
using Assets.Scripts.Model.Storage;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class GameModel 
    {

        public static bool IsExist
        {
            get { return Instance != null; }
        }

        private static GameModel instance;
        public static GameModel Instance
        {
            get
            {
                if(instance == null)
                    Create();
                return instance;
            }
        }

        public static void Create()
        {
            Create(!CustomStorage.IsExist ? new PlayerPrefsStorage() : CustomStorage.Instance.Storage);
        }

        public static void Create(IModelStorage storage)
        {
            instance = new GameModel(storage);
        }

        public IModelStorage ModelStorage { get; private set; }

        public GameModel(IModelStorage modelStorage)
        {
            Debug.Log("Creating GameModel. ModelStorage is " + modelStorage.GetType().Name);
            ModelStorage = modelStorage;
            InvalidateStars();
            ModelStorage.RegisterGameSession();

            if (IsFirstLaunch)
            {
                ProcessFirstLaunch();
            }
            else
            {
                ProcessLaunch();
            }
        }

        public GameModel() : this(new PlayerPrefsStorage())
        {
        }

        #region Constants
        public static int Worlds = Enum.GetValues(typeof(WorldNumber)).Length;
        public static int LevelsInWorld = Enum.GetValues(typeof(LevelNumber)).Length;
        public static int TotalLevels = Worlds*LevelsInWorld;


        #endregion

        #region Current level info
        public LevelIndex CurrentLevelIndex
        {
            get
            {
                return new LevelIndex(CurrentLevel, CurrentWorld);
            }
        }
        public LevelNumber CurrentLevel { get; private set; }
        public WorldNumber CurrentWorld { get; private set; }
        public bool IsCurrentLevel(LevelNumber levelNumber, WorldNumber worldNumber)
        {
            if (CurrentWorld == worldNumber)
            {
                return CurrentLevelIndex.LevelNumber == levelNumber && CurrentLevelIndex.WorldNumber == worldNumber;
            }
            if (levelNumber == LevelNumber.Level1) return true;
            if(GetLevelStatus(levelNumber, worldNumber) == LevelStatus.NotCompleted && GetLevelStatus(levelNumber-1, worldNumber) != LevelStatus.NotCompleted) return true;
            return false;
        }
        public bool IsCurrentLevel(LevelIndex index)
        {
            return IsCurrentLevel(index.LevelNumber, index.WorldNumber);
        }

        public void SaveCurrentLevelIndex()
        {
            ModelStorage.SetCurrentWorld(CurrentWorld);
            ModelStorage.SetCurrentLevel((int)CurrentLevel, CurrentWorld);
        }
        #endregion

        #region Game progress processing
        public void SetNextLevelAsCurrent()
        {
            if (CurrentLevel == LevelNumber.Level16)
            {
                CurrentLevel = LevelNumber.Level1;
                CurrentWorld++;
            }
            else
            {
                CurrentLevel++;
            }
            SaveCurrentLevelIndex();
        }

        public void RegisterCurrentLevelResults(StarsCount stars)
        {

            RegisterLevelResults(CurrentLevel, CurrentWorld, stars);



        }

        public void RegisterLevelResults(LevelNumber levelNumber, WorldNumber worldNumber, StarsCount starsCount)
        {
            var status = GetLevelStatusByStarsCount(starsCount);
            var oldStatus = ModelStorage.GetLevelStatus((int)CurrentLevel, CurrentWorld);

            if (status > oldStatus)
            {
                ModelStorage.SetLevelStatus((int)levelNumber, worldNumber, status);
            }
            InvalidateStars();
        }

        public void ProcessCurrentLevelResults(StarsCount stars)
        {
            RegisterCurrentLevelResults(stars);
            SetNextLevelAsCurrent();
        }

        public void ProcessCurrentLevelSkip()
        {
            const StarsCount starsForSkip = StarsCount.ThreeStar;
            ProcessCurrentLevelResults(starsForSkip);
        }

        #endregion

        #region Level status getting  
        public static LevelStatus GetLevelStatusByStarsCount(StarsCount starsCount)
        {
            switch (starsCount)
            {
                case StarsCount.None:
                    return LevelStatus.NotCompleted;
                case StarsCount.OneStar:
                    return LevelStatus.OneStar;
                case StarsCount.TwoStar:
                    return LevelStatus.TwoStar;
                case StarsCount.ThreeStar:
                    return LevelStatus.ThreeStar;
            }
            return LevelStatus.NotCompleted;
        }

        public bool IsFirstLevelInWorld
        {
            get { return CurrentLevelIndex.LevelNumber == LevelNumber.Level1; }
        }
        public LevelStatus GetLevelStatus(LevelNumber levelNumber, WorldNumber worldNumber)
        {
            return ModelStorage.GetLevelStatus((int)levelNumber, worldNumber);
        }
        #endregion

        #region World status getting

        public WorldStatus GetWorldStatus(WorldNumber worldNumber)
        {
            return ModelStorage.GetWorldStatus(worldNumber);
        }

        #endregion

        #region Stars (ingame money)

        public int TotalStars { get; private set; }

        public void InvalidateStars()
        {

            TotalStars = 0;

            foreach (var s in EnumerateLevelsStatuses())
            {
                TotalStars += (int) GetStarsCountByLevelStatus(s);
            }
        }

        public static StarsCount GetStarsCountByLevelStatus(LevelStatus status)
        {
            switch (status)
            {
                case LevelStatus.OneStar:
                    return StarsCount.OneStar;
                case LevelStatus.TwoStar:
                    return StarsCount.TwoStar;
                case LevelStatus.ThreeStar:
                    return StarsCount.ThreeStar;
                case LevelStatus.NotCompleted:
                    return StarsCount.None;
                default:
                    return StarsCount.None;
            }
        }

        #endregion

        #region Currency

        public void SetCurrency(int currency)
        {
            if(IsInfiniteCurrency()) return;

            ModelStorage.SetGameCurrency(currency);
        }

        public int GetCurrency()
        {
            return ModelStorage.GetGameCurrency();
        }

        public void AddCurrency(int currency)
        {

            SetCurrency(GetCurrency() + currency);
        }

        public void RemoveCurrency(int currency)
        {
            SetCurrency(GetCurrency() - currency);
        }

        public bool IsEnoughCurrency(int targetCurrency)
        {
            return IsInfiniteCurrency() || GetCurrency() >= targetCurrency;
        }

        public bool IsInfiniteCurrency()
        {
            var c = GetCurrency();
            return c < 0 || c > 999;
        }

        public void SetInfiniteCurrency()
        {
            SetCurrency(-1);
        }

        public DateTime? GetLatestCurrencyIncrementationDate(CurrencyIncrementation.CurrencyIncrementationSource source)
        {
            return ModelStorage.GetLatestCurrencyIncrementationDate(source);
        }

        public bool CanIncerementCurrency(CurrencyIncrementation.CurrencyIncrementationSource source)
        {
            if (source == CurrencyIncrementation.CurrencyIncrementationSource.AutorIncrementation)
            {
                var latest =
                    GetLatestCurrencyIncrementationDate(
                        CurrencyIncrementation.CurrencyIncrementationSource.AutorIncrementation);
                return (!latest.HasValue &&
                        (DateTime.Now - latest.Value) > CurrencyIncrementation.AutoincrementationTimeout);
            }
            if (source == CurrencyIncrementation.CurrencyIncrementationSource.Videobanner)
            {
                var latest =
                    GetLatestCurrencyIncrementationDate(
                    CurrencyIncrementation.CurrencyIncrementationSource.Videobanner);
                return (!latest.HasValue &&
                        (DateTime.Now - latest.Value) > CurrencyIncrementation.VideobannerTimeout);
            }
            return false;
        }

        public void AutoincrementCurrency(CurrencyIncrementation.CurrencyIncrementationSource source)
        {
            AddCurrency(source == CurrencyIncrementation.CurrencyIncrementationSource.AutorIncrementation
                ? CurrencyIncrementation.CurrencyFromAutoincrementation
                : CurrencyIncrementation.CurrencyFromVideobanner);

            RegisterCurrencyIncrementationDate(source);
        }

        public bool TryAutoincrementCurrency()
        {
            if (!CanIncerementCurrency(CurrencyIncrementation.CurrencyIncrementationSource.AutorIncrementation))
                return false;
            AutoincrementCurrency(CurrencyIncrementation.CurrencyIncrementationSource.AutorIncrementation);
            return true;
        }

        private void RegisterCurrencyIncrementationDate(CurrencyIncrementation.CurrencyIncrementationSource source)
        {
            ModelStorage.SetLatestCurrencyIncrementationDate(DateTime.Now, source);
        }



        #endregion

        #region Ads

        public bool IsAdsRemoved
        {
            get { return ModelStorage.GetAdsRemoveStatus(); }
        }

        public void RemoveAds()
        {
            ModelStorage.SetAdsRemoveStatus(true);
        }

        #endregion

        #region Unlocks

        public void RegisterWorldUnlock(WorldNumber worldNumber)
        {
            if(ModelStorage.GetWorldStatus(worldNumber) == WorldStatus.Locked)
                ModelStorage.SetWorldStatus(worldNumber, WorldStatus.Unlocked);
            SaveCurrentLevelIndex();
        }

        public void RegisterWorldUnlockAndAllInnerLevels(WorldNumber worldNumber)
        {
            RegisterWorldUnlock(worldNumber);
            for (var i = 1; i < 17; i++)
            {
                var levelNumber = (LevelNumber) i;
                var status = GetLevelStatus(levelNumber, worldNumber);

                if (status == LevelStatus.NotCompleted)
                {
                    RegisterLevelResults(levelNumber, worldNumber, StarsCount.ThreeStar);
                }
            }
        }

        public void RegisterWorldUnlockAndMoveInto(WorldNumber worldNumber)
        {
            RegisterWorldUnlock(worldNumber);
            CurrentLevel = LevelNumber.Level1;
            CurrentWorld = worldNumber;
        }

        public void UnlockAll()
        {
            RegisterWorldUnlockAndAllInnerLevels(WorldNumber.World1);
            RegisterWorldUnlockAndAllInnerLevels(WorldNumber.World2);
            RegisterWorldUnlockAndAllInnerLevels(WorldNumber.World3);
            RegisterWorldUnlockAndAllInnerLevels(WorldNumber.World4);
            RegisterWorldUnlockAndAllInnerLevels(WorldNumber.World5);
        }

        public bool CanUnlockWorldByStars(WorldNumber world)
        {
            InvalidateStars();
            return Prices.GetStarsForWorld(world) <= TotalStars;
        }

        #endregion

        #region Enumerators

        public static IEnumerable<WorldNumber> EnumerateWorlds()
        {
            yield return WorldNumber.World1;
            yield return WorldNumber.World2;
            yield return WorldNumber.World3;
            yield return WorldNumber.World4;
            yield return WorldNumber.World5;
        }

        public static IEnumerable<LevelNumber> EnumerateLevels()
        {
            var names = Enum.GetNames(typeof (LevelNumber));
            for (var i = 0; i < names.Length; i++)
            {
                yield return (LevelNumber)Enum.Parse(typeof(LevelNumber), names[i]);
            }
        }

        public  IEnumerable<WorldStatus> EnumerateWorldsStatuses()
        {
            foreach (var w in EnumerateWorlds())
            {
                yield return GetWorldStatus(w);
            }
        }

        public  IEnumerable<LevelStatus> EnumerateLevelsStatuses()
        {
            foreach (var w in EnumerateWorlds())
            {

                foreach (var l in EnumerateLevels())
                {
                    yield return GetLevelStatus(l, w);
                }
            }
        }

        #endregion

        #region launch management
        public bool IsFirstLaunch
        {
            get { return !ModelStorage.IsLatestGameSessionExist; }
        }

        private void ProcessFirstLaunch()
        {
            SetCurrency(StartValues.StartCurrency);
            RegisterWorldUnlock(WorldNumber.World1);
            CurrentLevel = LevelNumber.Level1;
            CurrentWorld = WorldNumber.World1;
            SaveCurrentLevelIndex();
        }

        private void ProcessLaunch()
        {
            CurrentWorld = ModelStorage.GetCurrentWorld();
            CurrentLevel = (LevelNumber)ModelStorage.GetCurrentLevel(CurrentWorld);
        }

        public void UnregisterAllSessions()
        {
            ModelStorage.UnregisterAllSessions();
        }

        #endregion

        public void Save()
        {
            ModelStorage.Save();
        }

    }
}
