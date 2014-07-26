using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class WalkerCreationWorldBehavior : WorldBehavior
{
	public int number = 1000;
	private SpawningPoint[] sps;
	private int num = 0;
	private int counter = 0;

	public override void Step ()
	{
		if ((counter++) % 50 != 0) {
			return;
		}

		if (num < number) {
			AAgent pref = WalkerFactory.Instance.GetAWalker ();
			AAgent a = CreateAgent (AttachedWorld, pref);
		
			int i = Random.Range (0, sps.Length);
			NavMeshAgent nma = a.GetComponent<NavMeshAgent> ();
			if (nma)
				nma.enabled = false;
			a.transform.position = sps [i].transform.position;
			if (nma)
				nma.enabled = true;

			num++;
		}
		
		print ("Pedestrian Counts : " + counter);

	}

	public override void Initialize ()
	{
		print ("PedestrianCreationWorldBehavior#Initialize");


		sps = FindObjectsOfType<SpawningPoint> ();


	}
}
