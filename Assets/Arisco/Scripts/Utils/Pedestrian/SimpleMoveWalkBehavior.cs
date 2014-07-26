using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AAgent))]
[RequireComponent(typeof(CharacterController))]
public class SimpleMoveWalkBehavior : SpeedDirectionBehavior
{

	public float speedMultiplier = 1;
	
	protected CharacterController cc;
	void Initialize ()
	{
		cc = GetComponent<CharacterController> ();
	}

	void Step ()
	{
		cc.SimpleMove(transform.forward * Speed * speedMultiplier);
	}
	
	void End ()
	{
	
	}

}
