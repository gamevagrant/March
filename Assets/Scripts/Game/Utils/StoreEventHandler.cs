using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using Soomla;
using Soomla.Store;


/// <summary>
/// This class contains functions that receive events that they are subscribed to.
///
/// THIS IS JUST AN EXAMPLE. IF YOU WANT TO USE IT YOU NEED TO INSTANTIATE IT SOMEWHERE.
/// </summary>
public class StoreEventHandler : MonoBehaviour
{
    public static StoreEventHandler instance = null;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Initialize potential events.
    /// </summary>
    public void InitializeEventHandler()
    {
        StoreEvents.OnMarketPurchase += onMarketPurchase;
        StoreEvents.OnMarketRefund += onMarketRefund;
        StoreEvents.OnItemPurchased += onItemPurchased;
        StoreEvents.OnGoodEquipped += onGoodEquipped;
        StoreEvents.OnGoodUnEquipped += onGoodUnequipped;
        StoreEvents.OnGoodUpgrade += onGoodUpgrade;
        StoreEvents.OnBillingSupported += onBillingSupported;
        StoreEvents.OnBillingNotSupported += onBillingNotSupported;
        StoreEvents.OnMarketPurchaseStarted += onMarketPurchaseStarted;
        StoreEvents.OnItemPurchaseStarted += onItemPurchaseStarted;
        StoreEvents.OnCurrencyBalanceChanged += onCurrencyBalanceChanged;
        StoreEvents.OnGoodBalanceChanged += onGoodBalanceChanged;
        StoreEvents.OnMarketPurchaseCancelled += onMarketPurchaseCancelled;
        StoreEvents.OnRestoreTransactionsStarted += onRestoreTransactionsStarted;
        StoreEvents.OnRestoreTransactionsFinished += onRestoreTransactionsFinished;
        StoreEvents.OnSoomlaStoreInitialized += onSoomlaStoreInitialized;
        StoreEvents.OnUnexpectedStoreError += onUnexpectedStoreError;

#if UNITY_ANDROID && !UNITY_EDITOR
		StoreEvents.OnIabServiceStarted += onIabServiceStarted;
		StoreEvents.OnIabServiceStopped += onIabServiceStopped;
#endif
    }

    /// <summary>
    /// Handles unexpected errors with error code.
    /// </summary>
    /// <param name="errorCode">The error code.</param>
    public void onUnexpectedStoreError(int errorCode)
    {
        SoomlaUtils.LogError("SOOMLA StoreEventHandler", "error with code: " + errorCode);
    }

    /// <summary>
    /// Handles a market purchase event.
    /// </summary>
    /// <param name="pvi">Purchasable virtual item.</param>
    /// <param name="purchaseToken">Purchase token.</param>
    public void onMarketPurchase(PurchasableVirtualItem pvi, string payload, Dictionary<string, string> extra)
    {
        //Debug.Log("SOOMLA purchase operation has completed successfully (market purchase event)");

        if (pvi.ItemId == "no_ads")
        {
        }
        else if (pvi.ItemId == "product_1")
        {
            // plus coin
            //GameData.instance.SavePlayerCoin(GameData.instance.GetPlayerCoin() + Configure.instance.product1Coin);
        }
        else if (pvi.ItemId == "product_2")
        {
            // plus coin
            //GameData.instance.SavePlayerCoin(GameData.instance.GetPlayerCoin() + Configure.instance.product2Coin);
        }
        else if (pvi.ItemId == "product_3")
        {
            // plus coin
            //GameData.instance.SavePlayerCoin(GameData.instance.GetPlayerCoin() + Configure.instance.product3Coin);
        }
        else if (pvi.ItemId == "product_4")
        {
            // plus coin
            //GameData.instance.SavePlayerCoin(GameData.instance.GetPlayerCoin() + Configure.instance.product4Coin);
        }
        else if (pvi.ItemId == "product_5")
        {
            // plus coin
            //GameData.instance.SavePlayerCoin(GameData.instance.GetPlayerCoin() + Configure.instance.product5Coin);
        }

        if (pvi.ItemId == "product_1" || pvi.ItemId == "product_2" || pvi.ItemId == "product_3" || pvi.ItemId == "product_4" || pvi.ItemId == "product_5")
        {
            // play add coin sound
            AudioManager.instance.CoinAddAudio();

            // update text label
            ShopManager.instance.UpdateCoinAmountLabel();
        }
        
    }

    /// <summary>
    /// Handles a market refund event.
    /// </summary>
    /// <param name="pvi">Purchasable virtual item.</param>
    public void onMarketRefund(PurchasableVirtualItem pvi)
    {
        //Debug.Log("SOOMLA refund operation has been completed successfully (market refund event)");

        if (pvi.ItemId == "no_ads")
        {
            StoreInventory.TakeItem("no_ads", 1);
        }
        else if (pvi.ItemId == "product_1")
        {

        }
        else if (pvi.ItemId == "product_2")
        {

        }
        else if (pvi.ItemId == "product_3")
        {

        }
        else if (pvi.ItemId == "product_4")
        {

        }
        else if (pvi.ItemId == "product_5")
        {

        }
    }

    /// <summary>
    /// Handles an item purchase event.
    /// </summary>
    /// <param name="pvi">Purchasable virtual item.</param>
    public void onItemPurchased(PurchasableVirtualItem pvi, string payload)
    {
        
    }

    /// <summary>
    /// Handles a good equipped event.
    /// </summary>
    /// <param name="good">Equippable virtual good.</param>
    public void onGoodEquipped(EquippableVG good)
    {

    }

    /// <summary>
    /// Handles a good unequipped event.
    /// </summary>
    /// <param name="good">Equippable virtual good.</param>
    public void onGoodUnequipped(EquippableVG good)
    {

    }

    /// <summary>
    /// Handles a good upgraded event.
    /// </summary>
    /// <param name="good">Virtual good that is being upgraded.</param>
    /// <param name="currentUpgrade">The current upgrade that the given virtual
    /// good is being upgraded to.</param>
    public void onGoodUpgrade(VirtualGood good, UpgradeVG currentUpgrade)
    {

    }

    /// <summary>
    /// Handles a billing supported event.
    /// </summary>
    public void onBillingSupported()
    {
        
    }

    /// <summary>
    /// Handles a billing NOT supported event.
    /// </summary>
    public void onBillingNotSupported()
    {
        
    }

    /// <summary>
    /// Handles a market purchase started event.
    /// </summary>
    /// <param name="pvi">Purchasable virtual item.</param>
    public void onMarketPurchaseStarted(PurchasableVirtualItem pvi)
    {
        
    }

    /// <summary>
    /// Handles an item purchase started event.
    /// </summary>
    /// <param name="pvi">Purchasable virtual item.</param>
    public void onItemPurchaseStarted(PurchasableVirtualItem pvi)
    {
        
    }

    /// <summary>
    /// Handles an item purchase cancelled event.
    /// </summary>
    /// <param name="pvi">Purchasable virtual item.</param>
    public void onMarketPurchaseCancelled(PurchasableVirtualItem pvi)
    {
        
    }

    /// <summary>
    /// Handles a currency balance changed event.
    /// </summary>
    /// <param name="virtualCurrency">Virtual currency whose balance has changed.</param>
    /// <param name="balance">Balance of the given virtual currency.</param>
    /// <param name="amountAdded">Amount added to the balance.</param>
    public void onCurrencyBalanceChanged(VirtualCurrency virtualCurrency, int balance, int amountAdded)
    {

    }

    /// <summary>
    /// Handles a good balance changed event.
    /// </summary>
    /// <param name="good">Virtual good whose balance has changed.</param>
    /// <param name="balance">Balance.</param>
    /// <param name="amountAdded">Amount added.</param>
    public void onGoodBalanceChanged(VirtualGood good, int balance, int amountAdded)
    {

    }

    /// <summary>
    /// Handles a restore Transactions process started event.
    /// </summary>
    public void onRestoreTransactionsStarted()
    {
        
    }

    /// <summary>
    /// Handles a restore transactions process finished event.
    /// </summary>
    /// <param name="success">If set to <c>true</c> success.</param>
    public void onRestoreTransactionsFinished(bool success)
    {
        
    }

    /// <summary>
    /// Handles a store controller initialized event.
    /// </summary>
    public void onSoomlaStoreInitialized()
    {
        
    }

#if UNITY_ANDROID && !UNITY_EDITOR
	public void onIabServiceStarted() {

	}
	public void onIabServiceStopped() {

	}
#endif
}
