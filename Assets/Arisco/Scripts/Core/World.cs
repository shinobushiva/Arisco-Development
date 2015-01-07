#define NO_OVERRIDE
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arisco.Core;
using System.Linq;

/// <summary>
/// The class is representing Agent's behavioring environment
///
///@author shiva
///
/// </summary>
[RequireComponent(typeof(AriscoSystemInializeWorldBehavior))]
public class World : MonoBehaviour
{
    [HideInInspector]
	public bool timeTicking;

	#if NO_OVERRIDE
	private bool sending = false;
	
	private bool Check ()
	{
		if (!sending) {
			sending = true;
		} else {
			sending = false;
		}
		return sending;
	}
	#endif

	private bool endRequest = false;

	public bool EndRequest {
		get {
			return endRequest;
		}
		set {
			endRequest = value;
		}
	}

	/// <summary>
	/// The world's event listeners
	///</summary>
	protected List<IWorldEventListener> listeners = new List<IWorldEventListener> ();
	//
	
	private List<WorldBehavior> behaviors = new List<WorldBehavior> ();
	///<summery>
	///Components that hold by agents
	///</summery>
	public List<WorldBehavior> ABehaviors {
		get{ return behaviors;}
		set{ behaviors = value;}
	}
	
	protected List<AAgent> allAgents = new List<AAgent> ();
	/// <summary>
	/// All agents in this world
	/// </summary>
	public List<AAgent> AllAgents {
		get{ return allAgents;}
	}
	
	protected bool began;
	/// <summary>
	/// Check if the world ihas begun
	/// </summary>
	public bool Began {
		get{ return began; }
		set{ began = value; }
	}

	protected bool ended;
	/// <summary>
	/// Check if the world has ended
	/// </summary>
	public bool Ended {
		get { return ended; }
	}
			
	protected bool initialized;
	/// <summary>
	/// Check if the world has initialized
	/// </summary>
	public bool Initialized {
		get{ return initialized; }
	}

	/// <summary>
	/// Program the initializing behavior of the world in the inherit class
	/// </summary>
	public void InitializeBegin ()
	{
		
		WorldBehavior[] bs = GetComponents<WorldBehavior> ();
		behaviors.AddRange (bs);
		
		#if NO_OVERRIDE
			SendMessage ("Initialize", SendMessageOptions.DontRequireReceiver);
		#else
		foreach (WorldBehavior be in behaviors) {
			be.Initialize ();
		}
		#endif
	}
	
	public void InitializeEnd ()
	{
		
		foreach (IWorldEventListener l in listeners) {
			l.Initialized (this);
		}
		initialized = true;
	}
	
	/// <summary>
	/// Program the begining behavior of the world in the inherit class
	/// </summary>
	public void Begin ()
	{
		#if NO_OVERRIDE
		if (Check ())
			SendMessage ("Begin", SendMessageOptions.DontRequireReceiver);
		#else
		foreach (WorldBehavior be in behaviors) {
			be.Begin ();
		}
		#endif
		foreach (IWorldEventListener l in listeners) {
			l.Began (this);
		}
		began = true;
	}
	
	/// <summary>
	/// Program the step behavior of the world in the inherit class
	/// </summary>
	public void Step ()
	{
		#if NO_OVERRIDE
		if (Check ())
			SendMessage ("Step", SendMessageOptions.DontRequireReceiver);
		#else
		foreach (WorldBehavior be in behaviors) {
			be.Step ();
		}
		#endif
		foreach (IWorldEventListener l in listeners) {
			l.Stepped (this);
		}
	}

	/// <summary>
	/// Program the committing behavior of the world in the inherit class
	/// </summary>
	public void Commit ()
	{
#if NO_OVERRIDE
		if (Check ())
			SendMessage ("Commit", SendMessageOptions.DontRequireReceiver);
#else
		foreach (WorldBehavior be in behaviors) {
			be.Commit ();
		}
#endif
		foreach (IWorldEventListener l in listeners) {
			l.Committed (this);
		}
	}

	/// <summary>
	/// Program the destorying behavior of the world in the inherit class
	/// </summary>
	public void Dispose ()
	{
#if NO_OVERRIDE
		if (Check ())
			SendMessage ("Dispose", SendMessageOptions.DontRequireReceiver);
#else
		foreach (WorldBehavior be in behaviors) {
			be.Dispose ();
		}
#endif
		foreach (IWorldEventListener l in listeners) {
			l.Disposed (this);
		}
	}

	/// <summary>
	/// Program the ending behavior of the world in the inherit class
	/// </summary>
	public void End ()
	{
#if NO_OVERRIDE
		if (Check ())
			SendMessage ("End", SendMessageOptions.DontRequireReceiver);
#else
		foreach (WorldBehavior be in behaviors) {
			be.End ();
		}
#endif
		foreach (IWorldEventListener l in listeners) {
			l.Ended (this);
		}
		ended = true;
	}



	/// <summary>
	/// Add a world event listener
	///
	///@param l
	///           Event Listener {@link IWorldEventListener}
	/// </summary>
	public void AddWorldEventListener (IWorldEventListener l)
	{

		if (listeners == null) {
			listeners = new List<IWorldEventListener> ();
		}
		listeners.Add (l);

	}

	/// <summary>
	/// Remove all event listeners
	/// </summary>
	public void ClearWorldEventListeners ()
	{
		listeners.Clear ();
	}

	/// <summary>
	/// Remove a event listener
	///
	///@param l
	///           Event Lister {@link IWorldEventListener}
	/// </summary>
	public void RemoveWorldEventListener (IWorldEventListener l)
	{

		if (listeners.Contains (l)) {
			listeners.Remove (l);
		}

	}

	/// <summary>
	/// Register an agent to the world<br>
	/// The agent will be registered and started in {@link World#commit()} after running every ohter exsisting agents.
	/// </summary>
	public void RegisterAgent (AAgent agent)
	{
		foreach (IWorldEventListener l in listeners) {
			l.AgentAdded (this, agent);
		}
		agent.World = this;
	}

	/// <summary>
	/// Remove an agent from the world
	/// The agent will be removed in {@link World#commit()} after commiting every ohter exsisting agents.
	/// </summary>
	public void ResignAgent (AAgent agent)
	{
		foreach (IWorldEventListener l in listeners) {
			l.AgentRemoved (this, agent);
		}
	}
	
	/// <summary>
	/// Remove agents from the world
	/// Agents will be removed in {@link World#commit()} after commiting every ohter exsisting agents.
	/// </summary>
	public void ResignAgents (List<AAgent> agents)
	{
		allAgents = new List<AAgent> (allAgents.Except (agents));
	}

}