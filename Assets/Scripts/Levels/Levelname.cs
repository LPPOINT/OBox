using System;
using Assets.Scripts.Model;

namespace Assets.Scripts.Levels
{

    [Serializable]
    public class LevelName
    {
        public LevelName(int levelNumber, WorldNumber worldNumber)
        {
            LevelNumber = levelNumber;
            WorldNumber = worldNumber;
        }

        public int LevelNumber;
        public WorldNumber WorldNumber;

        public string GetScenePath(bool withExtension = false, bool openBuildLevel = false)
        {
            return string.Format("Scenes/Levels/World{0}/{1}Level{2}{3}",
                (int) WorldNumber,
                openBuildLevel ? "Build/" : string.Empty
                , LevelNumber,
                withExtension ? ".unity" : string.Empty);
        }

        public static string GetScenePath(int levelNumber, WorldNumber worldNumber, bool withExtension = false, bool openBuildLevel = false)
        {
            var i = new LevelName(levelNumber, worldNumber);
            return i.GetScenePath(withExtension, openBuildLevel);
        }

    }
}
