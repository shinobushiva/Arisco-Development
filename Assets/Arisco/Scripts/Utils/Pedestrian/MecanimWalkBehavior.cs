using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AAgent))]
[RequireComponent(typeof(CharacterController))]
public class MecanimWalkBehavior : SpeedDirectionBehavior
{
	protected Animator avatar;
	protected CharacterController controller;
	public float speedDampTime = .25f;
	public float directionDampTime = .25f;
	
	void Initialize ()
	{
		avatar = GetComponent<Animator> ();
		controller = GetComponent<CharacterController> ();
	}

	void Step ()
	{
		avatar.SetFloat ("Speed", Speed, speedDampTime, Time.deltaTime);
		avatar.speed = Speed;
		
		Vector3 curentDir = avatar.rootRotation * Vector3.forward;
		Vector3 wantedDir = Direction.normalized;

		if (Vector3.Dot (curentDir, wantedDir) > 0) {
			avatar.SetFloat ("Direction", Vector3.Cross (curentDir, wantedDir).y, directionDampTime, Time.deltaTime);
		} else {
			avatar.SetFloat ("Direction", Vector3.Cross (curentDir, wantedDir).y > 0 ? 1 : -1, directionDampTime, Time.deltaTime);
		}
		controller.Move (avatar.deltaPosition);
		transform.rotation = avatar.rootRotation;
		
	}
	
	void End ()
	{
		avatar.SetFloat ("Speed", 0);
	}

	float avatarSpeed;
	void FixedUpdate(){
		if(!AttachedAgent.World)
			return;

		if(!AttachedAgent.World.timeTicking){
			if(avatarSpeed == 0){
				avatarSpeed = avatar.speed;
			}
			avatar.speed = 0;
		}else{
			if(avatarSpeed != 0){
				avatar.speed = avatarSpeed;
			}
			avatarSpeed = 0;
		}
	}

}
