using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arisco.Core;

public abstract class WorldRunner : MonoBehaviour
{
	protected bool pausing = false;
	protected bool paused = false;

	//
	protected bool oneStepping = false;

	//
	protected float weight = 0;
	protected bool running = false;

	protected bool started = false;
	protected bool finished = false;

	public bool Paused {
		get {
			return paused;
		}
	}

	public bool OneStepping {
		get {
			return oneStepping;
		}
	}

	public bool Finished {
		get {
			return finished;
		}
	}

	public bool Started {
		get {
			return started;
		}
	}

	public bool Running {
		get {
			return running;
		}
	}

	public float Weight {
		get{ return weight;}
		set{ weight = value;}
	}

	private World world;

	public virtual World World {
		get { return world; }
		set { world = value; }
	}

	public virtual void Initialize ()
	{

	}

	public virtual void Step ()
	{
		print ("WorldRunner#Step");
	}

	public virtual void Run ()
	{
		print ("WorldRunner#Run");
	}

	public virtual void Destroy ()
	{

	}

	public void OneStep ()
	{
		oneStepping = true;
	}
	
	public void Pause (bool p)
	{
		pausing = p;
	}
	
	public virtual void Stop ()
	{
		print ("WorldRunner#Stop");
	}

}
	