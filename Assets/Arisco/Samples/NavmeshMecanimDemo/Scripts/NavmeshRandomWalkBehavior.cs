using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NavmeshRandomWalkBehavior : ABehavior
{
	private SpawningPoint[] sps;
	private Transform target;
	protected NavMeshAgent		agent;
	protected Animator			animator;
	protected Locomotion 		locomotion;

	protected void SetDestination (Vector3 pos)
	{
		agent.destination = pos;
	}

	void Initialize ()
	{
		agent = GetComponent<NavMeshAgent> ();
		agent.updateRotation = false;
		agent.updatePosition = false;

		sps = FindObjectsOfType<SpawningPoint> ();
	}
	
	void Begin ()
	{
		//SpawningPoint sp = sps.OrderBy (x => (Vector3.Distance (x.transform.position, transform.position))).ToArray () [0];
		
		animator = GetComponent<Animator> ();
		locomotion = new Locomotion (animator);
		target = transform;
		SetDestination (target.position);
	}

	protected void SetupAgentLocomotion ()
	{
		if (AgentDone ()) {
			locomotion.Do (0, 0);
			target = sps [Random.Range (0, sps.Length)].transform;
			SetDestination (target.position);
		} else {

			float speed = agent.desiredVelocity.magnitude;
			
			Vector3 velocity = Quaternion.Inverse (transform.rotation) * agent.desiredVelocity;
			float angle = Mathf.Atan2 (velocity.x, velocity.z) * 180.0f / Mathf.PI;
			
			locomotion.Do (speed, angle);
		}
	}
	
	protected bool AgentDone ()
	{
		return !agent.pathPending && AgentStopping ();
	}
	
	protected bool AgentStopping ()
	{
		return agent.remainingDistance <= agent.stoppingDistance;
	}

	void Step ()
	{
		SetupAgentLocomotion ();
	}

	void Commit ()
	{

	}

}
