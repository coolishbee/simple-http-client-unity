using System;
using UnityEngine;

namespace CoolishDemo.WWWResponse
{
    [Serializable]
    public class GetPosts_Res
    {
        [SerializeField]
        public Post[] Items = null;
    }         
    
    [Serializable]
    public class GetTodos_Res
    {
        [SerializeField]
        public Todo[] Items = null;
    }

    [Serializable]
    public class GetUsers_Res
    {
        [SerializeField]
        public User[] Items = null;
    }


    
}