using System;
using Assets.Scripts.Model.Numeration;
using Assets.Scripts.Model.Statuses;

namespace Assets.Scripts.Model.Storage
{
    public interface IModelStorage
    {
        LevelStatus GetLevelStatus(int levelNumber, WorldNumber worldNumber);
        void SetLevelStatus(int levelNumber, WorldNumber worldNumber, LevelStatus status);

        int GetSkipsCount();
        void SetSkipsCount(int count);

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