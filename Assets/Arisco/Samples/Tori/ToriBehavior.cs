using UnityEngine;
using System.Collections;

public class ToriBehavior : SpeedDirectionBehavior {

	public override void Step ()
	{
		Forward(1);
	}
}
