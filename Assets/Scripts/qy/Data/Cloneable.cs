using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

namespace qy
{
    public class Cloneable<T>
    {

        public T Clone()
        {
            string json = JsonMapper.ToJson(this);
            T c = JsonMapper.ToObject<T>(json);
            return c;
        }
    }
}

