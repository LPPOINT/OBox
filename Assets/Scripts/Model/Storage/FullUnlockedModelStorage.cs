using Assets.Scripts.Model.Statuses;

namespace Assets.Scripts.Model.Storage
{
    public class FullUnlockedModelStorage : RAMModelStorage
    {
        public FullUnlockedModelStorage()
        {
            foreach (var world in GameModel.EnumerateWorlds())
            {
                SetWorldStatus(world, WorldStatus.Unlocked);
                foreach (var level in GameModel.EnumerateLevels())
                {
                    SetLevelStatus((int)level, world, LevelStatus.ThreeStar);
                }
            }

            SetSkipsCount(99);
        }
    }
}