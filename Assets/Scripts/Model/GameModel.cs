using System;
using System.Collections.Generic;
using Assets.Scripts.Levels;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class GameModel 
    {

        public static bool IsExist
        {
            get { return Instance != null; }
        }
        public static GameModel Instance { get; private set; }
        public static void Create()
        {
            if(Instance != null)
                return;

            Instance = new GameModel();
        }

        public GameModel()
        {
            InvalidateStars();
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
            return CurrentLevelIndex.LevelNumber == levelNumber && CurrentLevelIndex.WorldNumber == worldNumber;
        }
        public bool IsCurrentLevel(LevelIndex index)
        {
            return IsCurrentLevel(index.LevelNumber, index.WorldNumber);
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

        #region Unlocks

        public void RegisterWorldUnlock(WorldNumber worldNumber)
        {
            if(ModelStorage.GetWorldStatus(worldNumber) == WorldStatus.Locked)
                ModelStorage.SetWorldStatus(worldNumber, WorldStatus.Unlocked);
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

        #region Skips
        public int SkipsCount
        {
            get { return ModelStorage.GetSkipsCount(); }
        }

        public void DecrementSkips()
        {
            DecrementSkips(1);
        }
        public void DecrementSkips(int value)
        {
            ModelStorage.SetSkipsCount(SkipsCount - value);
        }

        public void IncrementSkips()
        {
            IncrementSkips(1);
        }
        public void IncrementSkips(int value)
        {
            ModelStorage.SetSkipsCount(SkipsCount + value);
        }

        #endregion

        #region Enumerators

        public IEnumerable<WorldNumber> EnumerateWorlds()
        {
            yield return WorldNumber.World1;
            yield return WorldNumber.World2;
            yield return WorldNumber.World3;
            yield return WorldNumber.World4;
            yield return WorldNumber.World5;
        }

        public IEnumerable<LevelNumber> EnumerateLevels()
        {
            var names = Enum.GetNames(typeof (LevelNumber));
            for (var i = 0; i < names.Length; i++)
            {
                yield return (LevelNumber)Enum.Parse(typeof(LevelNumber), names[i]);
            }
        }

        public IEnumerable<WorldStatus> EnumerateWorldsStatuses()
        {
            foreach (var w in EnumerateWorlds())
            {
                yield return GetWorldStatus(w);
            }
        }

        public IEnumerable<LevelStatus> EnumerateLevelsStatuses()
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

        public void Save()
        {
            ModelStorage.Save();
        }

    }
}
