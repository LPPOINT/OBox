using Debug = UnityEngine.Debug;

namespace Assets.Scripts.Purchases.IOS
{
    public class IOSPurchaseBuilder : IPurchaseBuilder
    {



        private  IPurchaseHandler currentHandler;
        private  ShopItemType currentItemType;


        private void PerformInternal(ShopItemType itemType, IPurchaseHandler handler)
        {
            Debug.Log("Performing purchase");
            currentHandler = handler;
            currentItemType = itemType;
            var productId = IOSIntegration.GetProductIdByItemType(itemType);
            if (IOSInAppPurchaseManager.instance.OnTransactionComplete != OnTransactionComplete)
                IOSInAppPurchaseManager.instance.OnTransactionComplete = OnTransactionComplete;
            IOSInAppPurchaseManager.instance.buyProduct(productId);
        }

        public void Perform(ShopItemType itemType, IPurchaseHandler handler)
        {

            if (!IOSInAppPurchaseManager.instance.IsStoreLoaded)
            {
                IOSInAppPurchaseManager.instance.OnStoreKitInitComplete += result => PerformInternal(itemType, handler);
                IOSInAppPurchaseManager.instance.loadStore();
            }
            else
            {
                PerformInternal(itemType, handler);
            }

        }

        private  void OnTransactionComplete(IOSStoreKitResponse r)
        {

            if(r.state == InAppPurchaseState.Failed) currentHandler.OnPurchaseFailed(currentItemType);
            else if(r.state == InAppPurchaseState.Purchased) currentHandler.OnPurchaseCompleted(currentItemType);
            else if (r.state == InAppPurchaseState.Restored) ;//TODO: OnPurchaseRestored()

            if (IOSInAppPurchaseManager.instance.OnTransactionComplete == OnTransactionComplete)
                IOSInAppPurchaseManager.instance.OnTransactionComplete = null;
        }
    }
}
