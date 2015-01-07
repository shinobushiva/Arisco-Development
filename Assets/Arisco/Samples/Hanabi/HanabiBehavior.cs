 using UnityEngine;
using System.Collections;

public class HanabiBehavior : SpeedDirectionBehavior {

	WorldStepCountBehavior counter;

	public override void Initialize ()
	{
		counter = AttachedAgent.World.GetComponent<WorldStepCountBehavior> ();

		Position = new Vector3(0, 0, 2.5f);
		Direction = new Vector3(0, 0, 1);

	}

	public override void Step ()
	{
		if(counter.StepCount == 30){ 
			if( Rand () < 0.5f ){
				Direction = AngleToDirection(0, 180 - Rand ()*30 - 90, 0);
			}else{
				Direction = AngleToDirection(0, Rand ()*30 - 90, 0);
			}
		}

		if(counter.StepCount >= 40 ){
			Turn (0, Rand () * 20 - 10, 0);
		}

		Forward(1);

	}

}
