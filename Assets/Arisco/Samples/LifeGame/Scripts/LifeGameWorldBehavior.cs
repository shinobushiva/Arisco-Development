using UnityEngine;
using System.Collections;

public class LifeGameWorldBehavior : WorldBehavior
{
	
	public AAgent lifePrefab;

	public override void Initialize ()
	{	

		int num = AriscoGUI.Instance.Get<int> ("num", 20);
		bool torus = AriscoGUI.Instance.Get<bool>("torus", true);
		Camera.main.transform.position = new Vector3 (0,  0, -num);

		if(GetComponent<LimitedWorld>()){
			GetComponent<LimitedWorld>().size = Vector3.one * num;
		}
		
		float offset = num / 2;
		
		for (int i=0; i<num; i++) {
			for (int j=0; j<num; j++) {
                AAgent a = CreateAgent (AttachedWorld, lifePrefab, new Vector3 (i - offset, j - offset, 0));
			}
		}

		
		LimitedWorld tw = GetComponent<LimitedWorld>();
		if(tw && torus){
			tw.size = Vector3.one * num;
			tw.offset = Vector3.one * (num%2 == 0 ? -.5f : 0);
		}else if(tw){
			tw.enabled = false;
		}
	}
	 
	void Start(){
	}
}
