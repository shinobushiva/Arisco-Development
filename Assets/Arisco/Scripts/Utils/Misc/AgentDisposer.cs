using UnityEngine;
using System.Collections;

public class AgentDisposer : MonoBehaviour
{

	void OnTriggerEnter (Collider c)
	{
		print (c.name);
		PedestrianWalkBehavior pwb = c.GetComponent<PedestrianWalkBehavior> ();
		if (pwb) {
			AAgent a = pwb.GetComponent<AAgent> ();
			a.World.ResignAgent (a);
		}
		
	}
}
