using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeatbugBehavior : ABehavior {

	public float idealTemperture = 100;
	public float heatAmount = 100;

	public override void Initialize ()
	{
		idealTemperture = Random.Range(idealTemperture/10, idealTemperture);
		heatAmount = Random.Range(heatAmount/10, heatAmount);
	}

	public override void Step ()
	{
		//unhappiness = Math.abs (idealTemperature - heatHere);

		List<HeatFieldBehavior> list = GetAgentsAroundPosition<HeatFieldBehavior>(AttachedAgent.World, transform.position, .5f, false);
		HeatFieldBehavior hf = list[0];
		float heatHere = hf.Amount;

		float unhappiness = Mathf.Abs(idealTemperture - heatHere);
		if(unhappiness == 0){
			GetComponent<SpeedDirectionBehavior>().Speed = 0;
		}else{
			GetComponent<SpeedDirectionBehavior>().Speed = 1;
		}

		hf.Amount = hf.Amount + heatAmount;
	}



	// Use this for initialization
	void Start () {
	
	}
	

}
