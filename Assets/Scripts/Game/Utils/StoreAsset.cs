using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla.Store;

public class StoreAsset : IStoreAssets
{
    public int GetVersion()
    {
        return 3;
    }

    public VirtualCurrency[] GetCurrencies()
    {
        return new VirtualCurrency[] { };
    }

    public VirtualGood[] GetGoods()
    {
        return new VirtualGood[] { NO_ADS_LTVG, PRODUCT_1, PRODUCT_2, PRODUCT_3, PRODUCT_4, PRODUCT_5 };
    }

    public VirtualCurrencyPack[] GetCurrencyPacks()
    {
        return new VirtualCurrencyPack[] { };
    }

    public VirtualCategory[] GetCategories()
    {
        return new VirtualCategory[] { };
    }

    public const string NO_ADS_ID = "no_ads";
    public const string PRODUCT_1_ID = "product_1";
    public const string PRODUCT_2_ID = "product_2";
    public const string PRODUCT_3_ID = "product_3";
    public const string PRODUCT_4_ID = "product_4";
    public const string PRODUCT_5_ID = "product_5";

    //public const string REFUNDED = "android.test.refunded";
    //public const string CANCELED = "android.test.canceled";
    //public const string PURCHASED = "android.test.purchased";

    /** LifeTimeVGs **/

    public static VirtualGood NO_ADS_LTVG = new LifetimeVG(
        "No Ads", // name
        "No Ads", // description
        NO_ADS_ID, // item id
        new PurchaseWithMarket(NO_ADS_ID, 0.99)); // the way this virtual good is purchased

    /** Virtual Goods **/

    public static VirtualGood PRODUCT_1 = new SingleUseVG(
        "Stack of coins", // name
        "Stack of coins", // description
        PRODUCT_1_ID, // item id
        new PurchaseWithMarket(PRODUCT_1_ID, 0.99)); // the way this virtual good is purchased

    public static VirtualGood PRODUCT_2 = new SingleUseVG(
        "Jar of coins", // name
        "Jar of coins", // description
        PRODUCT_2_ID, // item id
        new PurchaseWithMarket(PRODUCT_2_ID, 1.99)); // the way this virtual good is purchased

    public static VirtualGood PRODUCT_3 = new SingleUseVG(
        "Bag of coins", // name
        "Bag of coins", // description
        PRODUCT_3_ID, // item id
        new PurchaseWithMarket(PRODUCT_3_ID, 2.99)); // the way this virtual good is purchased

    public static VirtualGood PRODUCT_4 = new SingleUseVG(
        "Chest of coins", // name
        "Chest of coins", // description
        PRODUCT_4_ID, // item id
        new PurchaseWithMarket(PRODUCT_4_ID, 3.99)); // the way this virtual good is purchased

    public static VirtualGood PRODUCT_5 = new SingleUseVG(
        "Barrel of coins", // name
        "Barrel of coins", // description
        PRODUCT_5_ID, // item id
        new PurchaseWithMarket(PRODUCT_5_ID, 4.99)); // the way this virtual good is purchased
}
