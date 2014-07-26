using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AAgent))]
public class CarBehavior : SpeedDirectionBehavior
{

	public float maxSpeed = 0;
	
	//
	//Vector3 lv = (Vector3.forward + Vector3.left).normalized;
	//Vector3 rv = (Vector3.forward + Vector3.right).normalized;

	
	//
	private float range = 1f;
	public float viewDist = 1f;
	
	public override void Initialize ()
	{
		Direction = transform.forward;
		transform.LookAt (transform.position + Direction);
	}

	public override void Step ()
	{
		List<AAgent> list;
		
		list = GetAgentCollidersAroundPosition (
			AttachedAgent.World, transform.position + transform.forward * viewDist, range);
		list.Remove (AttachedAgent);
		
		Speed = Mathf.Clamp (Speed + (maxSpeed / 100f), 0, maxSpeed);
		
		if (list.Count > 0) {
			Speed = Mathf.Clamp (Speed - (maxSpeed / 10f), 0, maxSpeed);
		}
		
		transform.Translate (transform.forward * Speed, Space.World);
		
	}
	
	void OnDrawGizmos ()
	{
		Gizmos.DrawWireSphere (transform.position + transform.forward * viewDist, range);
		
	}
}
