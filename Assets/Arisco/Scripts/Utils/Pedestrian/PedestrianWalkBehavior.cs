using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AAgent))]
[RequireComponent(typeof(CharacterController))]
public class PedestrianWalkBehavior : SpeedDirectionBehavior
{
	//
	Vector3 lv = (Vector3.forward + Vector3.left).normalized;
	Vector3 rv = (Vector3.forward + Vector3.right).normalized;
	
	//
	private AAgent agent;
	private CharacterController cc;
	
	//
	private float range = 0.5f;
	public float viewDist = 1f;

	public float speedMultiplier = 1.0f;
	
	void Start ()
	{
		agent = GetComponent<AAgent> ();
		cc = GetComponent<CharacterController>();
	}
	
	public override void Begin ()
	{
		print ("PedestrianWalkBehavior#Begin");
		transform.LookAt (transform.position + Direction);
	}

	public override void Step ()
	{
		bool front = false;
		bool left = false;
		bool right = false;
		
		List<AAgent> list;
		
		list = GetAgentCollidersAroundPosition (
			agent.World, transform.position + transform.forward * viewDist, range);
		list.Remove (agent);
		
		if (list.Count > 0)
			front = true;
		
		list = GetAgentCollidersAroundPosition (
			agent.World, transform.position + transform.TransformDirection (lv) * viewDist, range);
		list.Remove (agent);
		
		if (list.Count > 0)
			left = true;
		
		list = GetAgentCollidersAroundPosition (
			agent.World, transform.position + transform.TransformDirection (rv) * viewDist, range);
		list.Remove (agent);
		
		if (list.Count > 0)
			right = true;

		
		List<BumpwayComponent> al = GetAgentsAroundPosition<BumpwayComponent> (agent.World, transform.position, range);
		if (al.Count > 0) {
			speedMultiplier = al [0].speedMultiplier;
		}
		
		float sp = Speed * speedMultiplier;
		if (!front) {
			//transform.Translate (transform.forward * sp, Space.World);

			cc.SimpleMove(transform.forward*sp);
		} else {
			if (!left && !right) {
				if (Random.value >= 0.5f)
					cc.SimpleMove(transform.TransformDirection (lv) * sp);
					//transform.Translate (transform.TransformDirection (lv) * sp, Space.World);
				else
					cc.SimpleMove(transform.TransformDirection (rv) * sp);
					//transform.Translate (transform.TransformDirection (rv) * sp, Space.World);
			} else if (!left) {
				cc.SimpleMove(transform.TransformDirection (lv) * sp);
				//transform.Translate (transform.TransformDirection (lv) * sp, Space.World);
			} else if (!right) {
				cc.SimpleMove(transform.TransformDirection (rv) * sp);
				//transform.Translate (transform.TransformDirection (rv) * sp, Space.World);
			} else {
				//transform.Translate (transform.forward * -sp / 10, Space.World);
				cc.SimpleMove( transform.forward*-sp / 10 );
			}
		}
	}
	
	void OnDrawGizmos ()
	{
		Gizmos.DrawWireSphere (transform.position + transform.forward, range);
		
		Gizmos.DrawWireSphere (transform.position + transform.TransformDirection (lv), range);
		
		Gizmos.DrawWireSphere (transform.position + transform.TransformDirection (rv), range);
	}
	
}
