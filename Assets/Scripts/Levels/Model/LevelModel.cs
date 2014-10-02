using Assets.Scripts.Meta.Model;

namespace Assets.Scripts.Levels.Model
{

    [System.Serializable]
    public class LevelModel
    {

        public LevelModel()
        {
            
        }

        public LevelModel(string levelPath, LevelMissionModel missionModel, WorldNumber worldNumber, int levelNumber)
        {
            MissionModel = missionModel;
            WorldNumber = worldNumber;
            LevelNumber = levelNumber;
        }

        public LevelMissionModel MissionModel;
        public WorldNumber WorldNumber;
        public int LevelNumber;

        public string LevelPath
        {
            get { return new LevelIndex(LevelNumber, WorldNumber).GetScenePath(true); }
        }

    }
}
