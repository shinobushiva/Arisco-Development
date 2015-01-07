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
public class AAgentCollider : AComponent
{
	void Start ()
	{
		Collider c = GetComponent<Collider> ();
        Collider2D c2d = GetComponent<Collider2D> ();
		if (!c && !c2d) {
			Debug.Log ("No Collider is attached!");
			Destroy (this);
		}
	}

}
