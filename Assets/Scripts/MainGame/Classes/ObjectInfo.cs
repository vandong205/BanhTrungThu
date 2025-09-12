using System.Runtime.CompilerServices;
using UnityEngine;

public class ObjectInfo:MonoBehaviour
{
    public ObjectType Type;
    public int ID;
    public string Name;
    public string RoleName;
    public void SetProp(ObjectType type, int id,string name,string rolename)
    {
        Type = type;
        ID = id;
        Name = name;
        RoleName = rolename;
    }
}