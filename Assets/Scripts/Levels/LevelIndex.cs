using System;
using Assets.Scripts.Model;

namespace Assets.Scripts.Levels
{

    [Serializable]
    public class LevelIndex
    {
        public LevelIndex(int levelNumber, WorldNumber worldNumber)
        {
            LevelNumber = (LevelNumber)levelNumber;
            WorldNumber = worldNumber;
        }

        public LevelIndex(LevelNumber levelNumber, WorldNumber worldNumber)
        {
            LevelNumber = levelNumber;
            WorldNumber = worldNumber;
        }

        public LevelNumber LevelNumber;
        public WorldNumber WorldNumber;

        protected bool Equals(LevelIndex other)
        {
            return WorldNumber == other.WorldNumber && LevelNumber == other.LevelNumber;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((LevelIndex) obj);
        }


        public string GetScenePath(bool withExtension = false, bool openBuildLevel = false)
        {
            return string.Format("Level{0}_{2}{4}{3}",
                (int) WorldNumber,
                openBuildLevel ? "Build/" : "Editor/"
                , LevelNumber,
                withExtension ? ".unity" : string.Empty,
                openBuildLevel ? "B" : "E");
        }

        public static int GetLevelHash(int levelNumber, WorldNumber worldNumber)
        {
            return 0;
        }


        public static string GetScenePath(int levelNumber, WorldNumber worldNumber, bool withExtension = false, bool openBuildLevel = false)
        {
            var i = new LevelIndex(levelNumber, worldNumber);
            return i.GetScenePath(withExtension, openBuildLevel);
        }

    }
}
