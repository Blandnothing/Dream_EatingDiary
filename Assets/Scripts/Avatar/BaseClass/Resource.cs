using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

//资源大类
public enum ResourceCategory
{
    Null,
    Nightmare,        //梦魇自身持有的资源，即饥饿度和意识开放度
    Mine,             //矿物，从关卡内获得的基础资源
    Item              //道具，
}
//资源小类
[System.Serializable]
public enum ResourceType{
    Null,
    
    //Nightmare
    Hunger, //饥饿度
    Openness,//意识开放度
    
    Nightmare,  //结束标识符,不应该被使用
    
    //Mine
    Gold,
    Silver,
    Copper,
    Gem,
    
    Mine,
    
    //Item
    Dichotomy, //虚实切换  
    Attract,//灵能吸引 
    Accelerate,//加速
    
    Item,
}


public class Resource
{
   
}

public static class ResourceIndex
{
    public static Dictionary<ResourceType,int> TypeToIndex = new()
        {
            {ResourceType.Gold,0},
            { ResourceType.Silver,1},
            { ResourceType.Copper ,2},
            { ResourceType.Gem ,3}
        };
}


 