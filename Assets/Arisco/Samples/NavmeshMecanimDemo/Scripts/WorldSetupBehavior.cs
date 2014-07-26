using UnityEngine;
using System.Collections;

public class WorldSetupBehavior : WorldBehavior
{
	public GameObject[] objectsToSetup;

	public override void Initialize ()
	{
		World w = GetComponent<World> ();

		foreach (GameObject go in objectsToSetup) {
			print ("Setup : " + go.name);
			go.SendMessage ("Setup", w);
		}
	}
	
	public override void Begin ()
	{
	}
}
