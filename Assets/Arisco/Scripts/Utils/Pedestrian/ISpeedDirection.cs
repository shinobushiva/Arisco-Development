using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface ISpeedDirection
{
	 // Property declaration:
    float Speed
    {
        get;
        set;
    }
	
	// Property declaration:
    Vector3 Direction
    {
        get;
        set;
    }
}
