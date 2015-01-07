using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ZombieStopBehavior : ABehavior {

    public int stopTime = 100;    
    private Vector3 pos;
	
    public override void Initialize(){
        pos = transform.position;
	}
	
    public override void Begin(){
	}


    public override void Step(){
        pos = transform.position; 
	}
	
    public override void Commit(){
        if (stopTime > 0)
        {
            transform.position = pos; 
            stopTime--;
        }
	}
	
    public override void End(){
	}
	
    public override void Dispose(){
	}
}
