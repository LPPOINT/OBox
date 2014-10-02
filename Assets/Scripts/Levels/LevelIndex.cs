using System;
using Assets.Scripts.Meta.Model;

namespace Assets.Scripts.Levels
{

    [Serializable]
    public class LevelIndex
    {
        public LevelIndex(int levelNumber, WorldNumber worldNumber)
        {
            LevelNumber = levelNumber;
            WorldNumber = worldNumber;
        }

        public int LevelNumber;
        public WorldNumber WorldNumber;

        public string GetScenePath(bool withExtension = false)
        {
            var l = "Assets/Scenes/Levels/World" + (int) WorldNumber + "/Level" + LevelNumber;
            if (withExtension)
            {
                l += ".unity";
            }
            return l;
        }

    }
}
