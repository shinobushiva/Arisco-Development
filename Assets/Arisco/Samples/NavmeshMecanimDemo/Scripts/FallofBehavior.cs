using UnityEngine;
using System.Collections;

public class FallofBehavior : ABehavior {

	public float height = 0;


	void Step(){
		if(transform.position.y < height)
			AttachedAgent.World.ResignAgent(AttachedAgent);
	}

}
