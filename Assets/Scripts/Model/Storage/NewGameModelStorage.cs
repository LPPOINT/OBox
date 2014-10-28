using Assets.Scripts.Model.Numeration;
using Assets.Scripts.Model.Statuses;

namespace Assets.Scripts.Model.Storage
{
    public class NewGameModelStorage : RAMModelStorage
    {
        public NewGameModelStorage()
        {
            SetCurrentLevel(1, WorldNumber.World1);
            SetCurrentWorld(WorldNumber.World1);
            SetLevelStatus(1, WorldNumber.World1, LevelStatus.NotCompleted);
            SetWorldStatus(WorldNumber.World1, WorldStatus.Unlocked);
        }
    }
}