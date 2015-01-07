using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arisco.Core;
using System.Linq;

public class AriscoTools : MonoBehaviour
{
    public AAgent CreateAgent (World world, AAgent agent, Vector3 pos)
	{
		AAgent a = Instantiate (agent) as AAgent;
        a.transform.parent = world.transform;
        a.transform.position = pos;
		world.RegisterAgent (a);
		
		return a;
	}

    public AAgent CreateAgent (World world, AAgent agent)
    {
        return CreateAgent(world, agent, Vector3.zero);
    }

    public void ResignAgent(World world, AAgent agent){
        world.ResignAgent(agent);
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
