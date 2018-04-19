using BestHTTP;
using LitJson;
using March.Core.Network.Data;
using UnityEngine;
using qy;

namespace March.Core.Network
{
    public delegate void OnSendCallback();
    public delegate void OnRecieveCallback(bool success);

    public interface INetHandler
    {
        string GetCommand();
        string GetData();
        void OnRecieve(HTTPRequest request, HTTPResponse response);
        OnSendCallback OnSendComplete { get; set; }
        OnRecieveCallback OnRecieveComplete { get; set; }
    }

    public abstract class AbstractNetHandler : INetHandler
    {
        public abstract string GetCommand();
        public abstract string GetData();

        public virtual void OnRecieve(HTTPRequest request, HTTPResponse response)
        {
            Debug.Log(string.Format("Server response hander is {0}, status is {1}, data is {2}", GetCommand(), response.IsSuccess ? "Success" : "Fail", response.DataAsText));

            if (OnRecieveComplete != null)
            {
                OnRecieveComplete(response.IsSuccess);
            }
        }

        public OnSendCallback OnSendComplete { get; set; }

        public OnRecieveCallback OnRecieveComplete { get; set; }
    }

    public class PayActionHandler : AbstractNetHandler
    {
        public PayActionData PayData = new PayActionData();

        public override string GetCommand()
        {
            return "pay.action";
        }

        public override string GetData()
        {
            return JsonMapper.ToJson(PayData);
        }

        public override void OnRecieve(HTTPRequest request, HTTPResponse response)
        {
            base.OnRecieve(request, response);

        }
    }

    public class PayHandler : AbstractNetHandler
    {
        public PayData PayData = new PayData();
        public override string GetCommand()
        {
            return "pay";
            //return "dev.pay.test";
        }

        public override string GetData()
        {
            return JsonMapper.ToJson(PayData);
        }

        public override void OnRecieve(HTTPRequest request, HTTPResponse response)
        {
            base.OnRecieve(request, response);

            var jsonData = JsonMapper.ToObject(response.DataAsText);
            var payRec = JsonMapper.ToObject<PayDataRec>(jsonData["thisP"].ToJson());

            if (payRec.status != 0)
            {
#if UNITY_EDITOR
                Toolkit.MessageBox.Show("Status: " + payRec.status, "Pay error");
#endif
                return;
            }

            GameMainManager.Instance.playerData.coinNum = payRec.gold;
            MainScene.Instance.RefreshPlayerData();
        }
    }
}
