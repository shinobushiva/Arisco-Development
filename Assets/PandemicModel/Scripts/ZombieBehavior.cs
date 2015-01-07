using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ZombieBehavior : ABehavior {

	public float radius = 5f;

    public override void Initialize(){
        GetComponent<ColoringComponent>().AgentColor = Color.red;
        //SendMessage("SetColor", Color.red);
	}

    public override void Begin(){
	}

    public override void Step(){
       
	}
	
    public override void Commit(){
        List<HumanBehavior> humans = GetAgentsAroundPosition<HumanBehavior> (transform.position, radius, false);
        if(humans.Count > 0){
            humans[Random.Range(0, humans.Count-1)].Infect();
        }
	}
	
    public override void End(){
	}
	
    public override void Dispose(){
	}
}
