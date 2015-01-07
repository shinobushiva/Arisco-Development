using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TachidomariBehavior : SpeedDirectionBehavior {

	void Start () {
	
	}

	public override void Initialize ()
	{
		Direction = AngleToDirection(0, Rand() * 360, 0);
	}

	public override void Step ()
	{
		List<TachidomariBehavior> agents = GetAgentsAroundPosition<TachidomariBehavior>(AttachedAgent.World, Position, 2);
		if(agents.Count >= 3){
		}else{
			Forward (1);
		}
	}

}
