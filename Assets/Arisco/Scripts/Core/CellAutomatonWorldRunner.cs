using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arisco.Core;
using System.Linq;

public sealed class CellAutomatonWorldRunner : WorldRunner
{

	private enum State
	{
		Default,
		StopCalled,
		Stopping,
		RunCalled,
		Running
	}
	private State state = State.Default;
	private float nextStepTime;
			
	public class CellAutomatonWorldEventAdapter : WorldEventAdapter
	{
		private List<AAgent> registering;
		private List<AAgent> resining;

		public CellAutomatonWorldEventAdapter (List<AAgent> registering, List<AAgent> resining)
		{
			this.registering = registering;
			this.resining = resining;
					
		}

		public override void AgentAdded (World world, AAgent agent)
		{
			//print ("CellAutomatonWorldRunner#AgentAdded:" + agent.name);
			registering.Add (agent);
		}
				
		public override void AgentRemoved (World world, AAgent agent)
		{
			resining.Add (agent);
		}
	}

	protected List<AAgent> resigningAgents = new List<AAgent> ();
	protected List<AAgent> registratingAgents = new List<AAgent> ();
			
	override public World World {
		set {
			base.World = value;
			CellAutomatonWorldEventAdapter a = new CellAutomatonWorldEventAdapter (registratingAgents, resigningAgents);
			
			print ("Add World Listener:" + a);
			World.AddWorldEventListener (a);
		}
	}

	///<summery>
	///  Initialize The World and agents
	///</summery>
	public override void Initialize ()
	{
		
		Time.timeScale = 1;
		base.World.timeTicking = true;

		pausing = false;
		paused = false;
		oneStepping = false;
		running = false;
		started = false;
		finished = false;

		// Add and Initialize exsisting agents
		List<AAgent> allAgents = World.AllAgents;
		allAgents.Clear ();
		allAgents.AddRange (registratingAgents);
		registratingAgents.Clear ();
		/*
		foreach (AAgent a in allAgents) {
			a.World = World;
			a.Initialize ();
		}
		*/
		
		World.InitializeBegin ();

		// Add and Initialize  agents who are added in World Initialize
		allAgents.AddRange (registratingAgents);
		registratingAgents.Clear ();
		foreach (AAgent a in allAgents) {
			a.World = World;
			a.Initialize ();
		}	

		World.InitializeEnd ();

	}



	void Update ()
	{
		if (state == State.Default) {
		} else if (state == State.StopCalled) {
			pausing = false;
			paused = false;
			oneStepping = false;
			running = false;

			state = State.Stopping;
		} else if (state == State.Stopping) {
			if (!World.Ended){
				foreach (AAgent a in World.AllAgents) {
					a.End ();
				}
				World.End ();
				finished = true;
				return;
			}
			//Time.timeScale = 1;
			base.World.timeTicking = true;
			finished = true;

			state = State.Default;
		} else if (state == State.RunCalled) {
			started = true;
			running = true;
			nextStepTime = 0;
			
			World.Begin ();
			foreach (AAgent a in World.AllAgents) {
				a.Begin ();
			}

			state = State.Running;

		} else if (state == State.Running) {

			if(nextStepTime < Time.time){
				base.World.timeTicking = true;
			}else{
				return;
			}

			if (running) {
				if (pausing && !oneStepping) {
					paused = true;
					base.World.timeTicking = false;
					return;
				} else if (oneStepping) {
					base.World.timeTicking = true;
					pausing = true;
					oneStepping = false;
				} else {
					base.World.timeTicking = true;
					paused = false;
				}
			
				foreach (AAgent a in World.AllAgents) {
					a.Step ();
				}
				World.Step ();
			
				foreach (AAgent a in World.AllAgents) {
					a.Commit ();
				}
			
				List<AAgent> resigningAgentsCopy = new List<AAgent> ();
				resigningAgentsCopy.AddRange (resigningAgents);
				// Process disposed agents
				foreach (AAgent a in resigningAgentsCopy) {
					a.End ();
				}
				World.ResignAgents (resigningAgentsCopy);
				for (int i=0; i<resigningAgentsCopy.Count; i++) {
					resigningAgentsCopy [i].Dispose ();
				}
				resigningAgents.Clear ();
			
				// Process being added agents
				List<AAgent> registratingAgentsCopy = new List<AAgent> ();
				registratingAgentsCopy.AddRange (registratingAgents);
				foreach (AAgent a in registratingAgentsCopy) {
					World.AllAgents.Add (a);
					a.Initialize ();
					a.Begin ();
				}
				registratingAgents.Clear ();
				World.Commit ();
			
				if (Weight == 0) {
				} else {
					base.World.timeTicking = false;
					nextStepTime = Time.time + Weight;
				}
			
				if (World.EndRequest) {
					AgentWorldRun.Instance.Stop ();
				}
			} else {
				foreach (AAgent a in World.AllAgents) {
					a.End ();
				}
				World.End ();
				finished = true;
			}

		}
	}

	public override void Stop ()
	{
		state = State.StopCalled;
	}

	///<summery>
	///until {@link World#IsFinished()} becomes true, execute {@link Agent#step()} to all agents, then 
	/// {@link World#step()}, then 、 execute {@link Agent#commit()} to all agents 
	/// and {@link World#commit()} repeatedly.
	///</summery>
	public override void Run ()
	{
		if (running) {
			return;
		}

		state = State.RunCalled;
	}

	///<summery>
	/// Dispose agents and the world
	///</summery>
	public void Dispose ()
	{
		foreach (AAgent a in World.AllAgents) {
			a.Dispose ();
		}
		World.Dispose ();
	}

}


