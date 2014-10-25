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
            return string.Format("Level{0}_{2}{4}{3}",
                (int) WorldNumber,
                openBuildLevel ? "Build/" : "Editor/"
                , LevelNumber,
                withExtension ? ".unity" : string.Empty,
                openBuildLevel ? "B" : "E");
        }

        public static string GetScenePath(int levelNumber, WorldNumber worldNumber, bool withExtension = false, bool openBuildLevel = false)
        {
            var i = new LevelName(levelNumber, worldNumber);
            return i.GetScenePath(withExtension, openBuildLevel);
        }

    }
}
