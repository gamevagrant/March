using March.Core.Network;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.MiniJSON;

namespace March.Core.Pay
{
    public class UIPaymentController : MonoBehaviour
    {
        private PayHandler handler = new PayHandler();

        public void OnPurchaseSuccess(PurchaseEventArgs args)
        {
            var recieptDict = args.purchasedProduct.receipt.HashtableFromJson();
            var payload = recieptDict["Payload"].ToString();
            var payloadJson = payload.HashtableFromJson();

            if (payloadJson.ContainsKey("json"))
            {
                var json = payloadJson["json"].ToString();
                var jsonJson = json.HashtableFromJson();
                handler.PayData.signData = json;
                handler.PayData.orderId = jsonJson["orderId"].ToString();
                handler.PayData.itemId = jsonJson["productId"].ToString();
                handler.PayData.purchaseTime = jsonJson["purchaseTime"].ToString();
                handler.PayData.exchangeId = jsonJson["productId"].ToString();
            }

            if (payloadJson.ContainsKey("signature"))
            {
                handler.PayData.signature = payloadJson["signature"].ToString();
            }

            Debug.LogWarning("OnPurchaseSuccess: pay data is: " + handler.PayData);

            NetManager.instance.SendRequest(handler);
        }

        public void OnPurchaseFail(Product product, PurchaseFailureReason failureReason)
        {
            var error = string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}",
                product.definition.storeSpecificId, failureReason);
            Debug.Log(error);
#if GAME_DEBUG
            Toolkit.MessageBox.Show(error, "Purchase Failed");
#endif
        }
    }
}
