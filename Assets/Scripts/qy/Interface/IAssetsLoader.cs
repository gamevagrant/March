using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

public interface IAssetsLoader
{

    IEnumerator LoadAssetAsync<T>(string path, System.Action<T> callback) where T : Object;


    T LoadAsset<T>(string path) where T : Object;
}
