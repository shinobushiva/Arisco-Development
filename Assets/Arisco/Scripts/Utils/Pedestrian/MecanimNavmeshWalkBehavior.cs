using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AAgent))]
[RequireComponent(typeof(CharacterController))]
public class MecanimNavmeshWalkBehavior : SpeedDirectionBehavior
{
	protected Animator avatar;
	protected CharacterController controller;
	public float speedDampTime = .25f;
	public float directionDampTime = .25f;

	private NavMeshAgent navAgent;
	private Locomotion locomotion;
	
	void Start ()
	{
		// Use this for initialization
		
		avatar = GetComponent<Animator> ();
		controller = GetComponent<CharacterController> ();
		
		avatar.speed = 1f;// + UnityEngine.Random.Range (-0.4f, 0.4f);
		
		navAgent = GetComponent<NavMeshAgent>();
		navAgent.updateRotation = false;
		
		locomotion = new Locomotion(avatar);
		
	}

	public void SetDestination( Vector3 pos )
	{
			navAgent.destination = pos;
	}
	
	public override void Begin ()
	{
		//print ("Begin");
	}

	protected void SetupAgentLocomotion()
	{
		if (AgentDone())
		{
			locomotion.Do(0, 0);
		}
		else
		{
			float speed = navAgent.desiredVelocity.magnitude;
			Vector3 velocity = Quaternion.Inverse(transform.rotation) * navAgent.desiredVelocity;
			float angle = Mathf.Atan2(velocity.x, velocity.z) * 180.0f / Mathf.PI;
			locomotion.Do(speed, angle);
		}
	}

	void OnAnimatorMove()
	{
		navAgent.velocity = avatar.deltaPosition / Time.deltaTime;
		transform.rotation = avatar.rootRotation;
	}

	protected bool AgentDone()
	{
		return !navAgent.pathPending && AgentStopping();
	}
	
	protected bool AgentStopping()
	{
		return navAgent.remainingDistance <= navAgent.stoppingDistance;
	}

		
	public override void Step ()
	{
		SetupAgentLocomotion();
	}
	
	public override void End ()
	{
		avatar.SetFloat ("Speed", 0);
	}


}
