using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP;

public class NetMessage {

    public string err;    // 错误信息

    public bool isOK
    {
        get
        {
            return string.IsNullOrEmpty(err);
        }
    }


}
