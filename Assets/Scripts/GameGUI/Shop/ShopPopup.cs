using System;
using Assets.Scripts.Camera.Effects;
using Assets.Scripts.Common;
using Assets.Scripts.Model;
using Assets.Scripts.Model.Constants;
using Assets.Scripts.Purchases;
using UnityEngine;

namespace Assets.Scripts.GameGUI.Shop
{
    public class ShopPopup : MonoBehaviour, IPurchaseHandler
    {
        public static event EventHandler Opened;
        private static void OnOpened()
        {
            var handler = Opened;
            if (handler != null) handler(null, EventArgs.Empty);
        }

        public static event EventHandler<GenericEventArgs<ShopItemType>> PurchasePerformed;
        private static void OnPurchasePerformed(ShopItemType itemType)
        {
            var handler = PurchasePerformed;
            if (handler != null) handler(null, new GenericEventArgs<ShopItemType>(itemType));
        }


        public static event EventHandler Closed;
        private static void OnClosed()
        {
            var handler = Closed;
            if (handler != null) handler(null, EventArgs.Empty);
        }

        public static bool IsOpened { get { return Current != null; } }
        public static ShopPopup Current { get; private set; }



        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                ExecutePurchase(ShopItemType.GameCurrency1);
            }
        }

        private void ExecutePurchase(ShopItemType item)
        {
            var builder = PurchaseBuilderFactory.CreateForCurrentPlatform();
            builder.Perform(item, this);
        }

        private void Start()
        {
            Current = this;
            OnOpened();
        }

        public void Close()
        {
            CameraBlurEffect.BlurOut();
            Destroy(gameObject);
            OnClosed();
        }

        public void OnPurchaseCompleted(ShopItemType itemType)
        {
            Debug.Log("OnPurchaseCompleted(" + itemType + ")");
            var currencyToAdd = Prices.GetCurrencyCountByShopItem(itemType);
            GameModel.Instance.AddCurrency(currencyToAdd);
            OnPurchasePerformed(itemType);
        }

        public void OnPurchaseFailed(ShopItemType itemType)
        {
            
        }
    }
}
