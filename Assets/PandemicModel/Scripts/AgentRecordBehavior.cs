using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AgentRecordBehavior : ABehavior {

    public float initTime;
    public int initStep;
	
	void Initialize(){
        initTime = Time.time;
        initStep = GetStepCount();
	}
	
	void Begin(){
	}

	void Step(){
	}
	
	void Commit(){
	}
	
	void End(){
	}
	
	void Dispose(){
	}
}
