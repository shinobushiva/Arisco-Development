using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomWalkBehavior : SpeedDirectionBehavior {

    public float maxTurnDegree = 30;
    public float excludeRadius = 0.5f;

    public override void Commit ()
	{
        if (excludeRadius > 0)
        {
            List<RandomWalkBehavior> rbs = GetAgentsAroundPosition<RandomWalkBehavior>(transform.position + Direction, excludeRadius, false, true);
            if (rbs.Count <= 0)
                Forward(1);
        } else
        {
            Forward(1);
        }
	}

    public override void Step(){
        Turn(0, Random.value * maxTurnDegree * 2 - maxTurnDegree, 0);
    }

    void Start(){
	}

}
