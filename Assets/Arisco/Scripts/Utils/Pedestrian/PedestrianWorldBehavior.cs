using UnityEngine;
using System.Collections;

[RequireComponent(typeof(World))]
public class PedestrianWorldBehavior : WorldBehavior
{
	private World world;
	
	//
	public AAgent pedestrian;
	public int numPedestrians = 20;
	public float minSpeed = 0.01f;
	public float maxSpeed = 0.1f;
	
	void Start ()
	{
		world = GetComponent<World> ();
	}

	public override void Initialize ()
	{
		print ("Initializing : PedestrianWorldBehavior");
		
		
		for (int i = 0; i< numPedestrians/2; i++) {
			{
				AAgent a = CreateAgent (world, pedestrian);
				PedestrianWalkBehavior pwb = a.GetComponent<PedestrianWalkBehavior> ();
			
				pwb.Direction = Vector3.right;
				a.transform.position = new Vector3 (-10, 0, (Random.value - 0.5f) * 10);
				pwb.Speed = Random.Range (minSpeed, maxSpeed);
			}
			{
				AAgent a = CreateAgent (world, pedestrian);
				PedestrianWalkBehavior pwb = a.GetComponent<PedestrianWalkBehavior> ();
			
				pwb.Direction = Vector3.left;
				a.transform.position = new Vector3 (10, 0, (Random.value - 0.5f) * 10);
				pwb.Speed = Random.Range (minSpeed, maxSpeed);
			}
		}
	}
}
