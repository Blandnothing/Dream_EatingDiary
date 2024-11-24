using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public enum ResourceType{
    
}

public class Resource 
{
    
}


//资源集合(封装一下参数)
public struct resourceUnion
{
	public Dictionary<ResourceType,BigInteger> resourceConsumption;
}
