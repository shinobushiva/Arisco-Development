using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//http://origins.santafe.edu/system/files/Heatbugs%20with%20NetLogo.pdf
public class HeatbugRandomMoveBehavior : RandomMoveBehavior {

	public float randomRate = 0.5f;

	public override void Step ()
	{
		//unhappiness = Math.abs (idealTemperature - heatHere);
		HeatbugBehavior hb = GetComponent<HeatbugBehavior>();

		List<HeatFieldBehavior> list = GetAgentsAroundPosition<HeatFieldBehavior>(AttachedAgent.World, transform.position, .5f, false);


		HeatFieldBehavior hf = list[0];
		float heatHere = hf.Amount;

		if(Random.value < randomRate){
			base.Step();
		}else{
			List<HeatFieldBehavior> list2 = GetAgentsAroundPosition<HeatFieldBehavior>(AttachedAgent.World, transform.position, 1.5f, false);
			list2 = list2.Where(x=> Vector3.Distance(x.transform.position, transform.position) > 1f).OrderBy(x => x.Amount).ToList();
			//print (list2);
			if(heatHere > hb.idealTemperture){
				HeatFieldBehavior hfb = list2[0];
				if(heatHere > hfb.Amount){
					d = hfb.transform.position - transform.position;
				}else{
					d = Vector3.zero;
				}
			}else if(heatHere < hb.idealTemperture){
				HeatFieldBehavior hfb = list2[list2.Count-1];
				if(heatHere < hfb.Amount){
					d = hfb.transform.position - transform.position;
				}else{
					d = Vector3.zero;
				}
			}
			List<RandomMoveBehavior> rmbs = 
				GetAgentsAroundPosition<RandomMoveBehavior>(AttachedAgent.World, transform.position+d, .5f, false);
			if(rmbs.Count != 0){
				base.Step();
			}
		}
		


	}


	// Use this for initialization
	void Start () {
	
	}
	

}
