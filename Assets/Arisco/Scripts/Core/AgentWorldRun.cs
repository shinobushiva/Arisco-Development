using UnityEngine;
using System.Collections;
using Arisco.Core;

public class AgentWorldRun : SingletonMonoBehaviour<AgentWorldRun>
{
	
	public World world;
	public float duration = 1f;
    public float Duration {
        get{
            return duration;
        }
        set{
            duration = value;
        }
    }

	[HideInInspector]
	public CellAutomatonWorldRunner runner;
	private GameObject worldCopy;
	
	void Update ()
	{
		if (runner != null) {
			runner.Weight = duration;
		}
	}
	
	public IEnumerator Start ()
	{
		GameObject go = new GameObject ("CellAutomatonWorldRunner");
		runner = go.AddComponent<CellAutomatonWorldRunner> ();
		yield return StartCoroutine (Init ());
	}
	
	IEnumerator Init ()
	{
		print ("AgentWorldRun#Init");
		while (runner.Running && !runner.Paused) {
			yield return new WaitForEndOfFrame ();
		}

		if (world == null) {
			world = FindObjectOfType<World> ();
		}
		
		if (!worldCopy) {
			worldCopy = (GameObject)GameObject.Instantiate (world.gameObject);
			worldCopy.name = world.gameObject.name;
			worldCopy.name = worldCopy.name + "_Copy";
		}
		DestroyImmediate (world.gameObject);
		
		yield return new WaitForEndOfFrame ();
		
		world = ((GameObject)GameObject.Instantiate (worldCopy)).GetComponent<World> ();
		world.gameObject.name = worldCopy.name.Replace ("_Copy", "");
		
		yield return new WaitForEndOfFrame ();
		world.gameObject.SetActive (true);
		
		yield return new WaitForEndOfFrame ();
		worldCopy.SetActive (false);
		
		runner.World = world;

		/*
		if (AriscoChart.Instance)
			AriscoChart.Instance.Init ();
		*/
		
		AAgent[] agents = world.gameObject.GetComponentsInChildren<AAgent> ();
		
		foreach (AAgent a in agents) {
			world.RegisterAgent (a);
		}
		runner.Initialize ();
	}

	public void Play ()
	{
		if (!runner.World.Ended) {
			runner.Run ();
		} else {
			StartCoroutine (_Play ());
		}
	}

	private IEnumerator _Play ()
	{
		yield return StartCoroutine (Init ());
		runner.Run ();
	}
	
	public void Pause ()
	{
		runner.Pause (!runner.Paused);
	}

	public void Step ()
	{
		if (!runner.Running) {
			Play ();
			runner.Pause (true);
		} else {
			runner.Pause (true);
			runner.OneStep ();
		}
	}
	
	public void Stop ()
	{
		if (runner.Running) {
			runner.Stop ();
		} else {
			StartCoroutine (Init ());
		}
	}
}
