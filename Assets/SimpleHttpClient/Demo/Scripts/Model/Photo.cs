using System;
using UnityEngine;

namespace CoolishDemo
{
    [Serializable]
    public class Photo
    {
        public int albumId;

        public int id;

        public string title;

        public string url;

        public string thumbnailUrl;

        public override string ToString()
        {
            return JsonUtility.ToJson(this, true);
        }
    }
}