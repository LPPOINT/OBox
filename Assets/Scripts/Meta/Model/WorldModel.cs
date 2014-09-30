using System.Collections.Generic;
using Assets.Scripts.Meta.Stats;

namespace Assets.Scripts.Meta.Model
{
    public class WorldModel
    {
        public List<LevelModel> Levels { get; set; }
        public WorldStatus Status { get; set; }

        public static WorldModel GetWorldByNumber(WorldNumber number)
        {
            return null;
        }

    }
}
