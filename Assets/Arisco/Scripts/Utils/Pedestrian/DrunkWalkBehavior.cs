using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AAgent))]
public class DrunkWalkBehavior : SpeedDirectionBehavior
{

	public override void Step ()
	{
		Direction = 
			new Vector3 ((Random.value - .5f) * Speed, 0, (Random.value - .5f) * Speed);
	}
	
	
}
