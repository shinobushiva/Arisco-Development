#define NO_OVERRIDE
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arisco.Core;

///<summery>
/// A class represent Agent
///
///@author shiva
///
///</summery>
public class AAgent : AComponent
{

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
	
		///<summery>
		/// Map components that the agent has.
		///</summery>
		private List<ABehavior> behaviors = new List<ABehavior> ();

		public List<ABehavior> Behaviors {
				get{ return behaviors;}
				set{ behaviors = value;}
		}
	
		//private string name;

		///<summery>
		/// Hold agent's event listeners.
		///</summery>
		private List<IAgentEventListener> listeners = new List<IAgentEventListener> ();

		///<summery>
		/// The world where the agent is in.
		///</summery>
		private World world;

		public World World {
				get{ return world; }
				set{ world = value; }
		}

		///<summery>
		/// Hold if the agent has already begun.
		///</summery>
		protected bool began;

		public bool Began {
				get{ return began; }
				set{ began = value; }
		}
	
		///<summery>
		/// Hold if the agent has already ended.
		///</summery>
		protected bool ended;

		public bool Ended {
				get{ return ended; }
				set{ ended = value; }
		}

		///<summery>
		/// 
		///</summery>
		public void Initialize ()
		{
				//behaviors.Clear();

				ABehavior[] bs = GetComponents<ABehavior> ();
				behaviors.AddRange (bs);

				AComponent[] acs = GetComponents<AComponent> ();
				foreach (AComponent ac in acs) {
						ac.AttachedAgent = this;
				}

#if NO_OVERRIDE
				if (Check ())
						SendMessage ("Initialize", SendMessageOptions.DontRequireReceiver);
#else
				foreach (ABehavior be in Behaviors) {
						be.Initialize ();
				}
#endif

				foreach (IAgentEventListener l in listeners) {
						l.Initialized (this);
				}

		}

		///<summery>
		///  
		///</summery>
		public void Begin ()
		{
#if NO_OVERRIDE
				if (Check ())
						SendMessage ("Begin", SendMessageOptions.DontRequireReceiver);
#else
				foreach (ABehavior be in Behaviors) {
						be.Begin ();
				}
#endif
				Began = true;

				foreach (IAgentEventListener l in listeners) {
						l.Began (this);
				}
		}

		///<summery>
		///
		///</summery>
		public void Step ()
		{
#if NO_OVERRIDE
				if (Check ())
						SendMessage ("Step", SendMessageOptions.DontRequireReceiver);
#else
				foreach (ABehavior be in Behaviors) {
					be.Step ();
				}
#endif		
				foreach (IAgentEventListener l in listeners) {
						l.Stepped (this);
				}
		}

	///<summery>
	/// 
	///</summery>
	public void Commit ()
	{
		#if NO_OVERRIDE
		if (Check ())
			SendMessage ("Commit", SendMessageOptions.DontRequireReceiver);
		#else
		foreach (ABehavior be in Behaviors) {
			be.Commit ();
		}
		#endif
		
		foreach (IAgentEventListener l in listeners) {
			l.Committed (this);
		}
	}
	
	///<summery>
	/// 
	///</summery>
	public void Dispose ()
	{
		#if NO_OVERRIDE
		if (Check ())
			SendMessage ("Dispose", SendMessageOptions.DontRequireReceiver);
		#else
		foreach (ABehavior be in Behaviors) {
			be.Dispose ();
		}
		#endif
		
		foreach (IAgentEventListener l in listeners) {
			l.Disposed (this);
		}
		
		Destroy (gameObject);
		
	}
	
	///<summery>
	/// 
	///</summery>
	public void End ()
	{
		#if NO_OVERRIDE
		if (Check ())
			SendMessage ("End", SendMessageOptions.DontRequireReceiver);
		#else
		foreach (ABehavior be in Behaviors) {
			be.End ();
		}
		#endif
		Ended = true;
		
		foreach (IAgentEventListener l in listeners) {
			l.Ended (this);
		}
	}
	
	/// <summary>
	/// Add a event listener
	///
	///@param l
		///           Event Listener {@link IAgentEventListener}
		/// </summary>
		public void AddAgentEventListener (IAgentEventListener l)
		{
				if (listeners == null) {
						listeners = new List<IAgentEventListener> ();
				}
				listeners.Add (l);
		}
	
		/// <summary>
		/// Remove all event listeners
		/// </summary>
		public void ClearAgentEventListeners ()
		{
				listeners.Clear ();
		}
	
		/// <summary>
		/// Remove a event listener
		///
		///@param l
		///           Event Lister {@link IAgentEventListener}
		/// </summary>
		public void RemoveAgentEventListener (IAgentEventListener l)
		{	
				if (listeners.Contains (l)) {
						listeners.Remove (l);
				}
		}
}