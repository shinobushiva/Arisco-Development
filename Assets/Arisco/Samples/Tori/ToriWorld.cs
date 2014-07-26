 using UnityEngine;
using System.Collections;

public class ToriWorld : WorldBehavior {

	public ToriBehavior toriPref;

	public override void Initialize ()
	{
		int radius = 10;
		for(float i=0;i<360; i+=360/100f){
			AAgent a = CreateAgent(AttachedWorld, toriPref.AttachedAgent);
			a.transform.position =  new Vector3(Mathf.Cos(i)*radius, 0, Mathf.Sin(i)*radius);
			a.transform.LookAt(Vector3.zero);
			a.GetComponent<SpeedDirectionBehavior>().Direction = a.transform.forward;
		}
	}
}
