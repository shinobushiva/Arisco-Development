using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class MedicBehavior : ABehavior {
	
    public float radius = 5;

    public override void Initialize(){
	}
	
    public override void Begin(){
	}


    public override void Step(){
	}
	
    public override void Commit(){
        List<HumanBehavior> humans = GetAgentsAroundPosition<HumanBehavior> (transform.position, radius, false, true);
        if(humans.Count > 0){
            humans[Random.Range(0, humans.Count-1)].DisInfect();
        }
	}
	
    public override void End(){
	}
	
    public override void Dispose(){
	}
}
