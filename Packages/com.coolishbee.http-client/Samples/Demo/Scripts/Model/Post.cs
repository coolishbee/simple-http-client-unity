using System;
using UnityEngine;

namespace CoolishDemo
{
    [Serializable]
    public class Post
    {
        public int id;

        public int userId;

        public string title;

        public string body;

        public override string ToString()
        {
            return JsonUtility.ToJson(this, true);
        }
    }
}