using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeatFieldBehavior : ABehavior {

	public float amount = 0;
	public float max = 100;

	//
	public float evapolate = 0.5f;
	public float dissipationRate = 0.01f;
	public float diffusionRate = 0.05f;
	
	public Color color = Color.blue;

	public float Amount {
		get {
			return amount;
		}
		set {

			if(value <= 0){
				Color c = Color.white;
				c.a = 0.1f;
				renderer.material.color = c; 
				amount = 0;
			}else if(value >= max) {
				Color c = color;
				c.a = 1f;
				renderer.material.color = c; 
				amount = max;
			}else{
				Color c = color;
				c.a = value/max;
				renderer.material.color = c;
				amount = value;
			}
		}
	}

	public override void Initialize ()
	{
		Amount = 0;
	}

	private List<HeatFieldBehavior> neighbours;

	public override void Begin ()
	{
		neighbours = GetAgentsAroundPosition<HeatFieldBehavior>(AttachedAgent.World, transform.position, 1.5f, false);
	}

	public override void Step ()
	{
		foreach(HeatFieldBehavior hfb in neighbours){
			hfb.Amount = hfb.Amount + (hfb.Amount - Amount) * diffusionRate;
		}
		Amount = Amount * (1 - dissipationRate);
		Amount = Amount - evapolate;
	}


}
