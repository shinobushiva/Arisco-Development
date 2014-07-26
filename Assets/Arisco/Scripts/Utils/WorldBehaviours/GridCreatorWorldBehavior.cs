using UnityEngine;
using System.Collections;

public class GridCreatorWorldBehavior : WorldBehavior {

	// Use this for initialization
	void Start () {
	
	}

	public GameObject prefab;

	
	public bool offsetCenter;
	public bool useLimitedWorld;

	public Vector3 size;
	public Vector3 offset;
	public Vector3 step = Vector3.one;

	
	public override void Initialize ()
	{
		AAgent aa = prefab.GetComponent<AAgent>();

		Vector3 offset = Vector3.zero;

		LimitedWorld lw = GetComponent<LimitedWorld>();
		if(useLimitedWorld && lw){
			size = lw.size;
			offset = lw.offset;
		}

		if(offsetCenter){
			offset = new Vector3((int)(-size.x/2), (int)(-size.y/2), (int)(-size.z/2));
		}

		for(int i=0;i< (int)size.x; i++){
			for(int j=0;j< (int)size.y; j++){
				for(int k=0;k< (int)size.z; k++){
					if(aa){
						AAgent a = CreateAgent(AttachedWorld, aa);
						a.transform.position = new Vector3(offset.x+i*step.x, offset.y+j*step.y, offset.z+k*step.z);
					}else{
						Instantiate(prefab, new Vector3(offset.x+i*step.x, offset.y+j*step.y, offset.z+k*step.z)+transform.position, Quaternion.identity);
					}
				}
			}
		}

	}
}
