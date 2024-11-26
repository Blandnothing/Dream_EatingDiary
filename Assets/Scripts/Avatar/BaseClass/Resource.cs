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
public enum ResourceType{
    Null,
    
    //Nightmare
    Hunger,         //饥饿度
    Openness,       //意识开放度
    Nightmare,      //结束标识符,不应该被使用
    
    //Mine
    Gold,
    Silver,
    Copper,
    Gem,
    Mine,
    
    //Item
    Item
}


public class Resource 
{
    
}


//资源集合(封装一下参数)
public class resourceUnion
{
	public Dictionary<ResourceType,int> resourceConsumption;
}

