namespace Assets.Scripts.Model
{
    public static class Prices
    {
        public static int StarsForWorld2 = 12;
        public static int StarsForWorld3 = 22;
        public static int StarsForWorld4 = 32;
        public static int StarsForWorld5 = 42;

        public static int GetStarsForWorld(WorldNumber world)
        {
            if (world == WorldNumber.World1) return 0;
            if (world == WorldNumber.World2) return StarsForWorld2;
            if (world == WorldNumber.World3) return StarsForWorld3;
            if (world == WorldNumber.World4) return StarsForWorld4;
            if (world == WorldNumber.World5) return StarsForWorld5;
            return 0;
        }

    }
}
