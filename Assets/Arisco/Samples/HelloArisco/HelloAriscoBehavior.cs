using UnityEngine;
using System.Collections;

public class HelloAriscoBehavior : ABehavior
{

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		public override void Step ()
		{
				transform.Translate (Vector3.forward);
		}
}
