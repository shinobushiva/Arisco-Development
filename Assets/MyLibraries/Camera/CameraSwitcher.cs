using UnityEngine;
using System.Collections;

public class CameraSwitcher :  MonoBehaviour
{
	
	public SwitchableCamera[] cameras;
	public SwitchableCamera main;

	// Use this for initialization
	void Start ()
	{
		main.isMain = true;
		foreach (SwitchableCamera sc in cameras) {
			sc.switcher = this;
			//sc.OnOff();
		}
		SystemWide.Instance.currentMainCamera = main.camera;
	}
	
	public void Switch (SwitchableCamera c)
	{
		//main.OnOff();
		//c.OnOff();

		main.camera.rect = c.camera.rect;
		float d = main.camera.depth;
		main.camera.depth = c.camera.depth;
		c.camera.depth = d;
		c.camera.rect = new Rect (0, 0, 1, 1);
		main.isMain = false;
		main = c;
		main.isMain = true;
		SystemWide.Instance.currentMainCamera = main.camera;

	}
}
