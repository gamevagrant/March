namespace March.Core.Network.Data
{
    /// <summary>
    /// pay.action called when purchase button is called.
    /// </summary>
    public class PayActionData
    {
        public string exchangeId;
    }


    /// <summary>
    /// pay called when purchase successful.
    /// </summary>
    public class PayData
    {
        public string exchangeId;
        public string signData;
        public string itemId;
        public string orderId;
        public string purchaseTime;
        public string signature;

        public override string ToString()
        {
            return string.Format(
                "exchangeId-{0}, signData-{1}, itemId-{2}, orderId-{3}, purchaseTime-{4}, signature-{5}", exchangeId,
                signData, itemId, orderId, purchaseTime, signature);
        }
    }

    public class PayDataRec
    {
        public int gold;
        public string itemId;
        public int payTotal;
        public string cost;
        public string orderId;
        public int status;
    }

    /// <summary>
    /// Purchase reciept.
    /// </summary>
    /// <remarks>https://docs.unity3d.com/Manual/UnityIAPPurchaseReceipts.html</remarks>
    public class PurchaseReciept
    {
        public string Store;
        public string TransactionID;
        public Payload Payload;
    }

    /// <summary>
    /// Purchase reciept payload.
    /// </summary>
    /// <remarks>https://docs.unity3d.com/Manual/UnityIAPPurchaseReceipts.html</remarks>
    public class Payload
    {
        public PayloadJson json;
        public string signature;
    }

    public class PayloadJson
    {
        //public bool autoRenewing;
        public string orderId;
        public string packageName;
        public string productId;
        public long purchaseTime;
        public int purchaseState;
        //public string developerPayload;
        public string purchaseToken;
    }
}