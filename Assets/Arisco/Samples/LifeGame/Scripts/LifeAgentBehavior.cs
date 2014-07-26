using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LifeAgentBehavior : ABehavior
{
	
	private bool alive = false;
	private bool nextAlive;


	public bool Alive {
		get {
			return alive;
		}
		set {
			if (value) {
				renderer.material.color = Color.red;
			} else {
				renderer.material.color  = Color.gray;
			}
			alive = value;
		}
	}

	public void Start(){
	
	}
	
	public override void Initialize ()
	{
		float rate = AriscoGUI.Instance.Get<float> ("rate", 0.5f);
		Alive = (Random.Range (0f, 1f) > rate);
	}

	private List<LifeAgentBehavior> agents;
	
	public override void Begin ()
	{
		agents = GetAgentsAroundPosition<LifeAgentBehavior> (AttachedAgent.World, transform.position, 1f);
		agents.Remove (this);
	}
	
	public override void Commit ()
	{
		Alive = nextAlive;
	}
	
	public override void Step ()	
	{
		int aliveNum = agents.Where (x => x.Alive).Count ();
		
		if (alive) {
			if (aliveNum <= 1 || aliveNum >= 4) {
				nextAlive = false;
			}
		} else {
			if (aliveNum == 3) {
				nextAlive = true;
			}
		}
	}
}
