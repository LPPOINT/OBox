using System;

namespace Assets.Scripts.Model.Constants
{
    public static class CurrencyIncrementation
    {
        public static TimeSpan AutoincrementationTimeout = new TimeSpan(0, 0, 30);
        public static TimeSpan VideobannerTimeout = new TimeSpan(0, 0, 15);

        public static int CurrencyFromAutoincrementation = 10;
        public static int CurrencyFromVideobanner = 5;

        public enum CurrencyIncrementationSource
        {
            Videobanner,
            AutorIncrementation
        }

    }
}
