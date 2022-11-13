using System;
using UnityEngine;

namespace CoolishDemo
{
    [Serializable]
    public class User
    {
        public int id;

        public string name;

        public string username;

        public string email;

        public string phone;

        public string website;

        public override string ToString()
        {
            return JsonUtility.ToJson(this, true);
        }
    }
}