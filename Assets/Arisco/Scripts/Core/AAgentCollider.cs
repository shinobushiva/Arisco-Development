using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arisco.Core;

///<summery>
///  The agent which has this componet will be considered a colliding object.
///
///@author shiva
///
///</summery>
[RequireComponent(typeof(AAgent))]
public class AAgentCollider : AComponent
{
	void Start ()
	{
		Collider c = GetComponent<Collider> ();
		if (!c) {
			Debug.Log ("No Collider is attached!");
			Destroy (this);
		}
	}

}
