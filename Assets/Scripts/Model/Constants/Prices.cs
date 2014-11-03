using Assets.Scripts.Model.Numeration;

namespace Assets.Scripts.Model.Constants
{
    public static class Prices
    {
        public static int StarsForWorld2 = 12;
        public static int StarsForWorld3 = 22;
        public static int StarsForWorld4 = 32;
        public static int StarsForWorld5 = 42;

        public static int CurrencyForWorld2 = 30;
        public static int CurrencyForWorld3 = 40;
        public static int CurrencyForWorld4 = 50;
        public static int CurrencyForWorld5 = 60;

        public static int CurrencyForSkipLevel = 4;

        public static int GetCurrencyForWorld(WorldNumber world)
        {
            if (world == WorldNumber.World1) return 0;
            if (world == WorldNumber.World2) return CurrencyForWorld2;
            if (world == WorldNumber.World3) return CurrencyForWorld3;
            if (world == WorldNumber.World4) return CurrencyForWorld4;
            if (world == WorldNumber.World5) return CurrencyForWorld5;
            return 0;
        }
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
