using Assets.Scripts.Meta.Stats;

namespace Assets.Scripts.Meta.Model
{
    public class LevelModel
    {

        public LevelModel()
        {
            
        }

        public LevelModel(int number, int world, LevelStatus status)
        {
            Number = number;
            World = world;
            Status = status;
        }

        public int Number { get; set; }
        public int World { get; set; }

        public LevelStatus Status { get; set; }

    }
}
