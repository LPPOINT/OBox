namespace Assets.Scripts.Purchases
{
    public interface IPurchaseBuilder
    {
        void Perform(ShopItemType type, IPurchaseHandler handler);
    }
}
