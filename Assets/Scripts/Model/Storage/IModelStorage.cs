using System;
using Assets.Scripts.Model.Constants;
using Assets.Scripts.Model.Numeration;
using Assets.Scripts.Model.Statuses;

namespace Assets.Scripts.Model.Storage
{
    public interface IModelStorage
    {
        LevelStatus GetLevelStatus(int levelNumber, WorldNumber worldNumber);
        void SetLevelStatus(int levelNumber, WorldNumber worldNumber, LevelStatus status);

        int GetGameCurrency();
        void SetGameCurrency(int currency);

        void SetLatestCurrencyIncrementationDate(DateTime date, CurrencyIncrementation.CurrencyIncrementationSource source);
        DateTime? GetLatestCurrencyIncrementationDate(CurrencyIncrementation.CurrencyIncrementationSource source);

        void SetCurrentLevel(int level, WorldNumber targetWorldNumber);
        int GetCurrentLevel(WorldNumber targetWorldNumber);

        void SetCurrentWorld(WorldNumber worldNumber);
        WorldNumber GetCurrentWorld();

        void SetWorldStatus(WorldNumber worldNumber, WorldStatus status);
        WorldStatus GetWorldStatus(WorldNumber worldNumber);

        void Save();
        void Clear();

        void RegisterGameSession(DateTime sessionStartTime);
        void RegisterGameSession();
        void UnregisterAllSessions();

        DateTime? GetCurrentGameSessionTime();
        DateTime? GetLatestGameSessionTime();

        bool IsCurrentGameSessionExist { get; }
        bool IsLatestGameSessionExist { get; }

    }
}