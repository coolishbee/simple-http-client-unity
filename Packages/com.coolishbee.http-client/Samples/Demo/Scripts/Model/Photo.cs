using System;
using UnityEngine;

namespace CoolishDemo
{
    [Serializable]
    public class Photo
    {
        public Urls urls;        

        public override string ToString()
        {
            return JsonUtility.ToJson(this, true);
        }
    }

    [Serializable]
    public class Urls
    {
        public string regular;

        public override string ToString()
        {
            return JsonUtility.ToJson(this, true);
        }
    }    
}