using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpeedDirectionBehavior))]
public class CarRoundBehavior : ABehavior {

	//private SpeedDirectionBehavior sdb;

	public Vector3 center = Vector3.zero;
	public float radius = 10f;

	public override void Initialize ()
	{
		//sdb = GetComponent<SpeedDirectionBehavior>();
	}

	public override void Step ()
	{

		//float speed = sdb.Speed;
		//Vector3 dir = sdb.Direction;
		//360*B1/(PI()*A1*2)
		
		//float dist = Vector3.Distance(transform.position, center);
		Vector3 pos = center + (transform.position - center).normalized * radius;

		transform.position = pos;

		float ang = 0;
		if(pos.z <= 0){
			ang = 360 - Vector3.Angle(Vector3.right*radius, pos);
		}else{
			ang = Vector3.Angle(Vector3.right*radius, pos);
		}
		//print (ang);
		Vector3 eAng = -1 * Vector3.up * ang;
		transform.eulerAngles = eAng;



		/*
		float dig = 360*speed/Mathf.PI*10*2;

		dir.z +=  speed*Mathf.Cos(dig);
		dir.x +=  speed*Mathf.Sin(dig);
		sdb.Direction = dir;

		transform.LookAt (transform.position + dir);
		*/

		//transform.Rotate(-Vector3.up * (Mathf.PI*2*speed));
		//transform.RotateAround(Vector3.zero, Vector3.up, dig);


	}

	void OnDrawGizmos ()
	{
		Gizmos.color = Color.red; 
		Gizmos.DrawLine(transform.position, transform.position + transform.forward*3);
		
	}
}
