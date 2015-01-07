using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GekijoHitoBehavior : SpeedDirectionBehavior {

	void Start(){
	
	}

	public override void Initialize ()
	{

	}

	public override void Step ()
	{
        List<GekijoHitoBehavior> list = GetAgentsAroundPosition<GekijoHitoBehavior>(AttachedAgent.World, Position, 1);
		if(list.Count >= 5){
			MoveToSpaceCell(5);
		}
	}

	void OnGUI(){
		List<GekijoHitoBehavior> agents = GetAgentsAroundPosition<GekijoHitoBehavior> (
			AttachedAgent.World, transform.position, 1f);

		Vector3 v = Camera.main.WorldToScreenPoint(transform.position);
		
		GUI.Label(new Rect(v.x-4, Screen.height - v.y-13, 30, 30), ""+agents.Count);
		
	}

}
