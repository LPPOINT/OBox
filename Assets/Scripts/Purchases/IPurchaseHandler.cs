namespace Assets.Scripts.Purchases
{
    public interface IPurchaseHandler
    {
        void OnPurchaseCompleted(ShopItemType itemType);
        void OnPurchaseFailed(ShopItemType itemType);
    }
}
