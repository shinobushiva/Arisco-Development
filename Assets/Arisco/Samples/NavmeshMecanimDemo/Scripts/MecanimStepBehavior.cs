using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AAgent))]
[RequireComponent(typeof(CharacterController))]
public class MecanimStepBehavior : ABehavior
{
	protected Animator avatar;
	protected CharacterController controller;
	
	void Initialize ()
	{
		avatar = GetComponent<Animator> ();
		controller = GetComponent<CharacterController> ();

		avatar.StartPlayback ();
		avatar.speed = 0;
		recording = -1;
	}

	int recording = -1;

	void Update ()
	{
		if (AttachedAgent.World && !AttachedAgent.World.timeTicking) {
			if(recording < 0)
				return;

			//print (""+recording+" : "+(avatar.recorderStopTime - avatar.recorderStartTime));
			if (recording >= 0 && recording < 1) {
				recording++;
			}else if (recording >= 1) {
				avatar.StopRecording ();
				avatar.StartPlayback ();
				avatar.speed = 0;
				recording = -1;
			}
		}
	}

	void Step ()
	{
		if (recording == -1) {
			avatar.StopPlayback ();
			avatar.StartRecording (1);
			avatar.speed = 1;
			recording = 0;
		}

		//avatar.SetFloat ("Speed", 1, 0.25f, Time.deltaTime);
		//avatar.speed = 1;

	}
	
	void End ()
	{
		//avatar.SetFloat ("Speed", 0);
		avatar.StartPlayback ();
		avatar.speed = 0;
		recording = -1;
	}

	/*
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
	*/

}
