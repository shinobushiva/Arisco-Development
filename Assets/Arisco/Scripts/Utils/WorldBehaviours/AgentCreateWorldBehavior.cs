 using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgentCreateWorldBehavior : WorldBehavior {

	public AAgent target;
	public int num;

	public bool randomInTheWorld;
	public bool notTogether;

	public override void Initialize ()
	{
		print ("AgentCreateWorldBehavior#Initialize");

		LimitedWorld lw = GetComponent<LimitedWorld>();

		List<Vector3> points = new List<Vector3>();
		for(int i=0;i<num; i++){
			AAgent a = CreateAgent(AttachedWorld, target);
			if(randomInTheWorld && lw){
				Vector3 vec = Vector3.zero;
				do{
					vec = new Vector3(Random.Range(lw.size.x/-2+1, lw.size.x/2), 0, Random.Range (-lw.size.z/2+1, lw.size.z/2));
					if(lw.grid){
						vec = ToGrid(vec);
					}
				}while(notTogether && points.Contains(vec));
				points.Add(vec);
				a.transform.position = vec;
			}
		}
	}

	void Start(){
	}

}
