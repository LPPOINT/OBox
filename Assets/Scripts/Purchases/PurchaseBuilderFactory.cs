using Assets.Scripts.Purchases.IOS;

namespace Assets.Scripts.Purchases
{
    public static class PurchaseBuilderFactory
    {
        public static IPurchaseBuilder CreateForCurrentPlatform()
        {
            return new IOSPurchaseBuilder();
        }
    }
}
