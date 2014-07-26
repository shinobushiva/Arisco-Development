using UnityEngine;
using System.Collections;

public class HideRendererAtRuntime : MonoBehaviour
{

	public bool showHide = false;

	// Use this for initialization
	void Start ()
	{
		Renderer[] rs = GetComponentsInChildren<Renderer> ();
			
		foreach (Renderer r in rs)
			r.enabled = showHide;
	
	}
}
