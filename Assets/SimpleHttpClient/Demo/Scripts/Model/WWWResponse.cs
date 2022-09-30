using System;

[Serializable]
public class WWWResponse<T>
{
    public int code { get; set; }
    public string msg { get; set; }
    public T data { get; set; }
}

[Serializable]
public class GetAllTeams_Response
{
    public TeamList data = null;
    public int code;
    public string msg;    
}

[Serializable]
public class TeamList
{
    public TeamInfo[] list = null;
}