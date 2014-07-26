using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arisco.Core;

///<summery>
///Component class
///
///@author shiva
///
///</summery>
public abstract class AComponent : AriscoTools
{

	///<summery>
	///Attached Agent
	///</summery>
	private AAgent attachedAgent;

	public AAgent AttachedAgent {
		get {
			if (attachedAgent == null)
				attachedAgent = GetComponent<AAgent> ();
			
			return attachedAgent;
		}
		set{ attachedAgent = value;}
	}

	/*
	public World AttachedWorld {
		get {
			return AttachedAgent.World;
		}
	}
	*/

	public int GetStepCount(){
		WorldStepCountBehavior wscb = AttachedAgent.World.GetComponent<WorldStepCountBehavior>();
		if(wscb)
			return wscb.StepCount;
		else
			return 0;
	}

	public List<AAgent> GetAgentCollidersAroundPosition (World world, Vector3 pos, float radius, bool includeItself = false)
	{
		List<AAgentCollider> list =  GetAgentsAroundPosition<AAgentCollider>(world, pos, radius);
		List<AAgent> agents = new List<AAgent>();
		foreach(AAgentCollider ac in list){
			agents.Add (ac.AttachedAgent);
		}
		if(!includeItself){
			agents.Remove(AttachedAgent);
		}
		
		return agents;
	}
	
	public List<AAgent> GetAgentsAroundPosition (World world, Vector3 pos, float radius, bool includeItself = false)
	{
		return GetAgentsAroundPosition<AAgent> (world, pos, radius, includeItself);
	}
	
	public List<T> GetAgentsAroundPosition<T> (World world, Vector3 pos, float radius, bool includeItself = false) where T : MonoBehaviour
	{

		bool is2D = (GetComponent<Collider2D>() != null);

		List<T> l = new List<T> ();
	
		{
			if(!is2D){
				Collider[] hitColliders = Physics.OverlapSphere (pos, radius);
				foreach (Collider c in hitColliders) {
					T a = c.GetComponent<T> ();
					if (a && !l.Contains(a)) {
						l.Add (a);
					}
				}
			}else{
				Collider2D[] hitColliders = Physics2D.OverlapCircleAll (pos, radius);
				foreach (Collider2D c in hitColliders) {
					T a = c.GetComponent<T> ();
					if (a && !l.Contains(a)) {
						l.Add (a);
					}
				}
			}
		}
		
		LimitedWorld lw = world.GetComponent<LimitedWorld>();
		if(lw && lw.closed){
			Vector3 size = lw.size;
			Bounds b = new Bounds(world.transform.position+lw.offset, size);
			
			Vector3 p = pos;
			{
				if(pos.x+1 > b.max.x){
					pos.x -= size.x;
				}else if(pos.x-1 < b.min.x){
					pos.x += size.x;
				}
				if(pos.x != p.x){
					if(!is2D){
						Collider[] hitColliders = Physics.OverlapSphere (pos, radius);
						
						foreach (Collider c in hitColliders) {
							T a = c.GetComponent<T> ();
							if (a && !l.Contains(a)) {
								l.Add (a);
							}
						}
					}else{
						Collider2D[] hitColliders = Physics2D.OverlapCircleAll (pos, radius);
						foreach (Collider2D c in hitColliders) {
							T a = c.GetComponent<T> ();
							if (a && !l.Contains(a)) {
								l.Add (a);
							}
						}
					}
				}
			}
			{
				pos = p;
				if(pos.y+1 > b.max.y){
					pos.y -= size.y;
				}else if(pos.y-1 < b.min.y){
					pos.y += size.y;
				}
				if(pos.y != p.y){
					if(!is2D){
						Collider[] hitColliders = Physics.OverlapSphere (pos, radius);
						foreach (Collider c in hitColliders) {
							T a = c.GetComponent<T> ();
							if (a && !l.Contains(a)) {
								l.Add (a);
							}
						}
					}else{
						Collider2D[] hitColliders = Physics2D.OverlapCircleAll (pos, radius);
						foreach (Collider2D c in hitColliders) {
							T a = c.GetComponent<T> ();
							if (a && !l.Contains(a)) {
								l.Add (a);
							}
						}
					}
				}
			}
			{
				pos = p;
				if(pos.z+1 > b.max.z){
					pos.z -= size.z;
				}else if(pos.z-1 < b.min.z){
					pos.z += size.z;
				}
				if(pos.z != p.z){
					if(!is2D){
						Collider[] hitColliders = Physics.OverlapSphere (pos, radius);
						foreach (Collider c in hitColliders) {
							T a = c.GetComponent<T> ();
							if (a && !l.Contains(a)) {
								l.Add (a);
							}
						}
					}else{
						Collider2D[] hitColliders = Physics2D.OverlapCircleAll (pos, radius);
						foreach (Collider2D c in hitColliders) {
							T a = c.GetComponent<T> ();
							if (a && !l.Contains(a)) {
								l.Add (a);
							}
						}
					}
				}
			}
		}

		if(!includeItself){
			l.Remove(AttachedAgent.GetComponent<T>());
		}

		return l;
	}

	void Start(){
	}

}
