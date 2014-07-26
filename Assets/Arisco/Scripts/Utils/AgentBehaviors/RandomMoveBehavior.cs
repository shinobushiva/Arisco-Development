﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomMoveBehavior : SpeedDirectionBehavior {

	private int[] values = new int[]{-1, 0, 1};

	public bool notTogether = false;
	public int timesToTry = 10;

	protected Vector3 d;

	public override void Step ()
	{
		d = Vector3.zero;

		for(int i=0; i<timesToTry; i++){
			//Vector3 pos = transform.position;

			while(d.magnitude == 0){
				d = new Vector3(values[Random.Range(0, 3)], 0, values[Random.Range(0, 3)]);
			}

			if(notTogether){
				List<RandomMoveBehavior> list = GetAgentsAroundPosition<RandomMoveBehavior>(AttachedAgent.World, Position+d, .5f, false);
				if(list.Count == 0){
					break;
				}
			}else{
				break;
			}
			
			d = Vector3.zero;
		}
		                     
	}

	public override void Commit ()
	{
		transform.Translate(d * Speed);
		LimitedWorld lw = AttachedAgent.World.GetComponent<LimitedWorld>();
		
		if(lw){
			Vector3 max = lw.size + lw.offset;
			Vector3 min = lw.offset;
			Bounds b = new Bounds(lw.offset, new Vector3(max.x-min.x, max.y - min.y, max.z-min.z));
			if(!b.Contains(transform.position)){
				transform.Translate(-d * Speed);
			}
		}
	}

	void Start(){
	}

}
