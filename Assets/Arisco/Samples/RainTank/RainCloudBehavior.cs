using UnityEngine;
using System.Collections;

public class RainCloudBehavior : SpeedDirectionBehavior {

	public float rainAmount = 1f;
	//public Vector3 min;
	//public Vector3 max;

	private RainCloudParticle[] cubes;

	public override void Initialize ()
	{
		cubes = GetComponentsInChildren<RainCloudParticle>();
	}

	public override void Step ()
	{
		foreach(RainCloudParticle go in cubes){
			RaycastHit hit;
			if(Physics.Raycast(go.transform.position, Vector3.down, out hit, 5)){
				HeatFieldBehavior rtf = hit.collider.gameObject.GetComponent<HeatFieldBehavior>();
				if(rtf != null){
					rtf.Amount = rtf.Amount + rainAmount;
				}
			}
		}

		/*
		Direction = new Vector3(Random.Range(-2, 3), 0, Random.Range (-2, 3));

		transform.Translate(Direction * speed);
		Bounds b = new Bounds(Vector3.zero, new Vector3(max.x-min.x, 10, max.z-min.z));
		if(!b.Contains(transform.position)){
			transform.Translate(-Direction * speed);
		}
		*/
		                     
	}

}
