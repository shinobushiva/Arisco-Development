using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpeedDirectionBehavior : ABehavior, ISpeedDirection
{

	public float speed = 1;
	public float Speed {
		get {
			return speed;
		}
		set {
			speed = value;
		}
	}

	public Vector3 direction = Vector3.forward;
	public Vector3 Direction {
		set {
			direction = value;
		}
		get {
			return direction;
		}
	}

	public Vector3 Position {
		set{
			transform.position = value;
		}
		get{
			return transform.position;
		}
	}

	public void LookAt(Vector3 target, bool yUp = false){
		if(yUp){
			target.y = transform.position.y;
		}
		transform.LookAt(target);
		Direction = transform.forward;
	}
	
	public void Turn(float x, float y, float z){
		Quaternion q = Quaternion.FromToRotation(Vector3.forward, Direction);
		Vector3 v3 = q.eulerAngles;

		Direction = AngleToDirection(v3.x+x, v3.y+y, v3.z+z).normalized;
	}

	public void Forward(float d){
		transform.Translate(Direction * Speed * d, Space.World);
	}

	public Vector3 AngleToDirection(float x, float y, float z){
		return AngleToDirection(x, y, z, Vector3.forward);
	}

	public Vector3 AngleToDirection(float x, float y, float z, Vector3 b){
		return (Quaternion.Euler( x, y, z ) * b).normalized;
	}
	
	public Vector3 AngleToDirection(Vector3 ang, Vector3 b){
		return AngleToDirection(ang.x, ang.y, ang.z, b);
	}

	#region cell functions
	public void ForwardDirectionCell(int x, int y, int z, int d){
		transform.Translate(new Vector3(x, y, z) * d, Space.World);
	}

	private static bool IsInRange(AAgent a, AAgent b, int d){
		return Mathf.Abs(a.transform.position.x - b.transform.position.x) <= d
				&& Mathf.Abs(a.transform.position.y - b.transform.position.y) <= d
				&& Mathf.Abs(a.transform.position.z - b.transform.position.z) <= d;
	}
	
	public void MoveToSpaceCell(int d){
		AAgent a = AttachedAgent;
		List<AAgent> list = a.World.AllAgents.Where(x => IsInRange(a, x, d)).ToList();
		LimitedWorld lw = a.World.GetComponent<LimitedWorld>();
		Bounds b = new Bounds();
		if(lw)
			b = lw.Bound;

		List<Vector3> candidates = new List<Vector3>();
		for(int x=-d;x<=d;x++){
			for(int y=-d;y<=d;y++){
				for(int z=-d;z<=d;z++){
					Vector3 v = ToGrid(new Vector3(x, y, z)+transform.position);
					if(lw){
						if(b.Contains(v)){
							candidates.Add(v);
						}
					}else{
						candidates.Add(v);
					}
				}
			}
		}
		foreach(AAgent agent in list){
			Vector3 p = ToGrid(agent.transform.position);
			candidates.Remove(p);
		}
		candidates.Remove(transform.position);
		candidates = candidates.OrderBy(x => Vector3.Distance(transform.position, x)).ToList();

		if(candidates.Count > 0)
			a.transform.position = candidates[0];

	}
	#endregion

}