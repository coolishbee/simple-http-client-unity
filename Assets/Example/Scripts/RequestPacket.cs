using System;
using UnityEngine;

namespace Example.RequestPacket
{
    [Serializable]
    public class TeamInfo_Req
    {
        public string team_name;
        public string manager_name;
        public string formation;

        public TeamInfo_Req(string team_name, string manager_name, string formation)
        {
            this.team_name = team_name;
            this.manager_name = manager_name;
            this.formation = formation;
        }

        public override string ToString()
        {
            return JsonUtility.ToJson(this, true);
        }
    }
}