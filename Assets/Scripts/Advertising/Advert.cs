using System;

namespace Assets.Scripts.Advertising
{
    public static class Advert
    {
        public static string GameId = "18140";
        public static float BannerTimeout = 30f;

        private static DateTime bannerOpenTime;
        public static void RegisterBannerOpen()
        {
            bannerOpenTime = DateTime.Now;
        }
        public static bool CanShowBanner
        {
            get
            {
                return bannerOpenTime != null && (DateTime.Now - bannerOpenTime) > TimeSpan.FromSeconds(BannerTimeout);
            }
        }

    }
}
