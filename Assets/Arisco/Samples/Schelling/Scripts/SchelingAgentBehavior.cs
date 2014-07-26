using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SchelingAgentBehavior : ABehavior
{

	[SerializeField]
	public enum Type{ 
		Penny, Dime, Empty
	}

	public Type type = Type.Empty;
	
	public bool IsSatisfied(Vector3 pos, float rate){
		List<SchelingAgentBehavior> agents = GetAgentsAroundPosition<SchelingAgentBehavior> (
			AttachedAgent.World, pos, 1f); 
		
		int numDime = agents.Where(x => x.type == Type.Dime).Count();
		int numPenny = agents.Where(x => x.type == Type.Penny).Count();

		if(numDime == 0 && numPenny == 0)
			return false;
		
		if(type == Type.Dime){
			float f = numDime/(float)(numDime + numPenny);
			if(f >= rate){
				return true;
			}
		}
		
		if(type == Type.Penny){
			float f = numPenny/(float)(numDime + numPenny);
			if(f >= rate){
				return true;
			}
		}

		return false;
	}


}
