using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(World))]	
[RequireComponent(typeof(BoxCollider))]	
public class ClosedAreaWorldBehavior : WorldBehavior
{
	private World world;
	
	void Start ()
	{
		world = GetComponent<World> ();
	}
	
	public override void Step ()
	{
		List<AAgent> agents = world.AllAgents;
		
		foreach (AAgent agt in agents) {
		
			Vector3 pos = agt.transform.position;
			
			float xdist = pos.x - transform.position.x;
			float ydist = pos.y - transform.position.y;
			float zdist = pos.z - transform.position.z;
			
			Vector3 b = collider.bounds.extents;// * 1.1f
			//float max = (int)Mathf.Max (Mathf.Abs (xdist) / b.x, Mathf.Abs (ydist) / b.y, Mathf.Abs (zdist) / b.z);
			
			bool x = Mathf.Abs (xdist) > b.x;
			bool y = Mathf.Abs (ydist) > b.y;
			bool z = Mathf.Abs (zdist) > b.z;
			
			if (x) {
				if (xdist < 0) {
					agt.transform.position += new Vector3 (b.x * 2, 0, 0);
				} else {
					agt.transform.position += new Vector3 (-b.x * 2, 0, 0);
				}
			} else if (y) {
				if (ydist < 0) {
					agt.transform.position += new Vector3 (0, b.y * 2, 0);
				} else {
					agt.transform.position += new Vector3 (0, -b.y * 2, 0);
				}
			} else if (z) {
				if (zdist < 0) {
					agt.transform.position += new Vector3 (0, 0, b.z * 2);
				} else {
					agt.transform.position += new Vector3 (0, 0, -b.z * 2);
				}
			}
			
		}
	}

	

}
