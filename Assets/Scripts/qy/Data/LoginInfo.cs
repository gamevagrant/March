using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace qy
{
    public class LoginInfo
    {

        public string newDeviceId = "";
        public string gameUid = "";
        public string appVersion = Application.version;
        public string gcmRegisterId = "";
        public string referrer = "";
        public string platform = PltformManager.instance.getPlatform();
        public string lang = "";
        public string afUID = "";
        public string pf = Application.platform.ToString();
        public string pfId = "";
        public string fromCountry = "";
        public string gaid = "";
        public int gmLogin = 1;
        public string terminal = "";
        public string SecurityCode = "";
        public string packageName = "";
        public string isHDLogin = "1";
        public string pfSeeeion = "";
        public string recallId = "";
    }
}

