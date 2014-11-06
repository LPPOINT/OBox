using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Purchases.IOS
{
    public static class IOSIntegration
    {

        private static Dictionary<ShopItemType, string> productsMap = new Dictionary<ShopItemType, string>()
                                                                      {
                                                                          {
                                                                              ShopItemType.GameCurrency1, "rose.currency.1"
                                                                          },
                                                                          {
                                                                              ShopItemType.GameCurrency2, "rose.currency.2"
                                                                          },
                                                                          {
                                                                              ShopItemType.GameCurrency3, "rose.currency.3"
                                                                          },
                                                                          {
                                                                              ShopItemType.InfinityGameCurrency, "rose.currency.infinity"
                                                                          },
                                                                      };

        public static string GetProductIdByItemType(ShopItemType item)
        {
            if (!productsMap.ContainsKey(item)) return string.Empty;

            return productsMap[item];

        }

        public static ShopItemType GetItemTypeByProductKey(string productKey)
        {
            var res = productsMap.FirstOrDefault(pair => pair.Value == productKey);
            return res.Key;
        }

        public static IEnumerable<string> EnumerateAllProducts()
        {
            yield return GetProductIdByItemType(ShopItemType.GameCurrency1);
            yield return GetProductIdByItemType(ShopItemType.GameCurrency2);
            yield return GetProductIdByItemType(ShopItemType.GameCurrency3);
            yield return GetProductIdByItemType(ShopItemType.InfinityGameCurrency);
        }

    }
}
