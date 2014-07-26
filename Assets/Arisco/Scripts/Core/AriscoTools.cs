using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arisco.Core;
using System.Linq;

public class AriscoTools : MonoBehaviour
{

	public AAgent CreateAgent (World world, AAgent agent)
	{
		
		AAgent a = Instantiate (agent) as AAgent;
		a.transform.parent = world.transform;
		world.RegisterAgent (a);
		
		return a;
	}

	public Vector3 ToGrid(Vector3 pos){
		pos.x = Mathf.RoundToInt(pos.x);
		pos.y = Mathf.RoundToInt(pos.y);
		pos.z = Mathf.RoundToInt(pos.z);
		return pos;
	}
	

	public float Rand(){
		return Random.value;
	}




	
}
