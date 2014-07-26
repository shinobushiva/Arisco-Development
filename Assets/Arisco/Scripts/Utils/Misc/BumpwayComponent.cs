using UnityEngine;
using System.Collections;

public class BumpwayComponent : AComponent
{
	
	public float speedMultiplier = 0.5f;
	
	void OnDrawGizmos ()
	{
		BoxCollider bc = GetComponent<BoxCollider> ();
		if (bc) {
			Bounds b = bc.bounds;
			Gizmos.DrawWireCube (b.center, b.size);
		}
	}

}
