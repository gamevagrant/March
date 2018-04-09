using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using UnityEngine;

public class PltformManager : Singleton<PltformManager>
{
    private  string platform = "";

    public void setPlatform(string value)
    {
        platform = value;
    }
    public string getPlatform()
    {
        return platform;
    }

    private string pf = "";
    public string Pf
    {
        get { return pf; }
        set { pf = value; }
    }

    private string pfId = "";
    public string PfId
    {
        get { return pfId; }
        set { pfId = value; }
    }

    private string phoneDevice = "";
    public string PhoneDevice
    {
        get { return pfId; }
        set { pfId = value; }
    }
    private string pfSession = "";
    public string PfSession
    {
        get { return pfSession; }
        set { pfSession = value; }
    }
    private string newDeviceId = "";
    public string NewDeviceId
    {
        get { return newDeviceId; }
        set { newDeviceId = value; }
    }

    private string fromCountry = "";
    public string FromCountry
    {
        get { return fromCountry; }
        set { fromCountry = value; }
    }

    private string appVersion = "";
    public string AppVersion

    {
        get { return appVersion; }
        set { appVersion = value; }
    }

}
