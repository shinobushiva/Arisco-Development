using UnityEngine;
using System.Collections;

public class AriscoSystemInializeWorldBehavior : WorldBehavior
{
	
	public override void Initialize ()
	{
        AriscoSystem.Initialize();
	}
	
	void Start(){
    }
	
}
