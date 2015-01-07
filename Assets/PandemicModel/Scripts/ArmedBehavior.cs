using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ArmedBehavior : ABehavior {
	
    public float radius = 5;
    public int coolDownTime = 10;
    public int stopTime = 5;

    private int coolTime = 0;

    public override void Initialize(){
	}
	
    public override void Begin(){
	}


    public override void Step(){
	}
	
    public override void Commit(){
        if (coolTime-- <= 0)
        {
            List<ZombieBehavior> zombies = GetAgentsAroundPosition<ZombieBehavior>(transform.position, radius, false);
            if (zombies.Count > 0)
            {
                zombies [Random.Range(0, zombies.Count - 1)].GetComponent<ZombieStopBehavior>().stopTime = stopTime;
                coolTime = coolDownTime;
            }
        }
	}
	
    public override void End(){
	}
	
    public override void Dispose(){
	}
}
