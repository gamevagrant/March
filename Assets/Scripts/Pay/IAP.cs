using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

namespace March.Core.Pay
{
    // Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
    public class IAP : MonoBehaviour, IStoreListener
    {
        public string PublicKey;

        private static IStoreController m_Controller; // Reference to the Purchasing system.
        private static IExtensionProvider m_StoreExtensionProvider; // Reference to store-specific Purchasing

        private bool m_IsGooglePlayStoreSelected;
        private string m_LastTransactionID;
        private bool m_PurchaseInProgress;
        private bool m_IsUnityChannelSelected;

        public UIPaymentController PayHandler;
        public List<string> ProductItems;

        public void InitializePurchasing()
        {
            // If we have already connected to Purchasing ...
            if (IsInitialized())
            {
                // ... we are done here.
                return;
            }

            var module = StandardPurchasingModule.Instance();
            module.useFakeStoreUIMode = FakeStoreUIMode.StandardUser;

            // Create a builder, first passing in a suite of Unity provided stores.
            var builder = ConfigurationBuilder.Instance(module);
            builder.Configure<IGooglePlayConfiguration>().SetPublicKey(PublicKey);

            m_IsGooglePlayStoreSelected =
                Application.platform == RuntimePlatform.Android && module.appStore == AppStore.GooglePlay;

            ProductItems.ForEach(product =>
            {
                // Add a product to sell / restore by way of its identifier, associating the general identifier with its store-specific identifiers.
                Debug.LogWarning("[===IAP===] adding product: " + product);
                builder.AddProduct(product, ProductType.Consumable,
                    new IDs {{product, GooglePlay.Name}}); // Continue adding the non-consumable product.
            });

            try
            {
                UnityPurchasing.Initialize(this, builder);
            }
            catch (Exception e)
            {
                Debug.LogError("Initialize exception: " + e.Message);
            }
        }


        private bool IsInitialized()
        {
            // Only say we are initialized if both the Purchasing references are set.
            return m_Controller != null && m_StoreExtensionProvider != null;
        }

        public void BuyProductID(string productId)
        {
            // If the stores throw an unexpected exception, use try..catch to protect my logic here.
            try
            {
                // If Purchasing has been initialized ...
                if (IsInitialized())
                {
                    // ... look up the Product reference with the general product identifier and the Purchasing system's products collection.
                    var product = m_Controller.products.WithID(productId);

                    // If the look up found a product for this device's store and that product is ready to be sold ...
                    if (product != null && product.availableToPurchase)
                    {
                        Debug.Log(string.Format("Purchasing product asychronously: '{0}'",
                            product.definition.id)); // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.
                        m_Controller.InitiatePurchase(product);
                    }
                    // Otherwise ...
                    else
                    {
                        // ... report the product look-up failure situation
                        Debug.Log(
                            "BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                    }
                }
                // Otherwise ...
                else
                {
                    // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or retrying initiailization.
                    Debug.Log("BuyProductID FAIL. Not initialized.");
                }
            }
            // Complete the unexpected exception handling ...
            catch (Exception e)
            {
                // ... by reporting any unexpected exception for later diagnosis.
                Debug.Log("BuyProductID: FAIL. Exception during purchase. " + e);
            }
        }


        // Restore purchases previously made by this customer. Some platforms automatically restore purchases. Apple currently requires explicit purchase restoration for IAP.
        public void RestorePurchases()
        {
            // If Purchasing has not yet been set up ...
            if (!IsInitialized())
            {
                // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
                Debug.Log("RestorePurchases FAIL. Not initialized.");
                return;
            }

            // If we are running on an Apple device ...
            if (Application.platform == RuntimePlatform.IPhonePlayer ||
                Application.platform == RuntimePlatform.OSXPlayer)
            {
                // ... begin restoring purchases
                Debug.Log("RestorePurchases started ...");

                // Fetch the Apple store-specific subsystem.
                var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
                // Begin the asynchronous process of restoring purchases. Expect a confirmation response in the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
                apple.RestoreTransactions((result) =>
                {
                    // The first phase of restoration. If no more responses are received on ProcessPurchase then no purchases are available to be restored.
                    Debug.Log("RestorePurchases continuing: " + result +
                              ". If no further messages, no purchases available to restore.");
                });
            }
            // Otherwise ...
            else
            {
                // We are not running on an Apple device. No work is necessary to restore purchases.
                Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
            }
        }


        //
        // --- IStoreListener
        //

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            // Purchasing has succeeded initializing. Collect our Purchasing references.
            Debug.Log("OnInitialized: PASS");

            // Overall Purchasing system, configured with products for this application.
            m_Controller = controller;
            // Store specific subsystem, for accessing device-specific store features.
            m_StoreExtensionProvider = extensions;

            LogProductDefinitions();
        }

        private void LogProductDefinitions()
        {
            var products = m_Controller.products.all;
            foreach (var product in products)
            {
#if UNITY_5_6_OR_NEWER
                Debug.Log(string.Format("id: {0}\nstore-specific id: {1}\ntype: {2}\nenabled: {3}\n", product.definition.id, product.definition.storeSpecificId, product.definition.type.ToString(), product.definition.enabled ? "enabled" : "disabled"));
#else
            Debug.Log(string.Format("id: {0}\nstore-specific id: {1}\ntype: {2}\n", product.definition.id,
                product.definition.storeSpecificId, product.definition.type.ToString()));
#endif
            }
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
            Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
        }


        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
        {
            m_LastTransactionID = e.purchasedProduct.transactionID;
            m_PurchaseInProgress = false;

            var productID = e.purchasedProduct.definition.id;

            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", productID));
            //If the item has been successfully purchased, store the item for later use!
            PlayerPrefs.SetInt(productID, 1);

            var information = string.Format("receipt:\n{0}\ndefinition:\nid - {1} type - {2}",
                e.purchasedProduct.receipt, e.purchasedProduct.definition.id,
                e.purchasedProduct.definition.type);
#if UNITY_EDITOR
            Toolkit.MessageBox.Show(information, "Purchase Information");
#endif
            Debug.LogWarning(information);

            PayHandler.OnPurchaseSuccess(e);

            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing this reason with the user.
            Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}",
                product.definition.storeSpecificId, failureReason));

            PayHandler.OnPurchaseFail(product, failureReason);
        }
    }
}