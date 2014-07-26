using UnityEngine;
using System.Collections;

public class SwitchableCamera : MonoBehaviour
{
	
	private GUIStyle noStyle;
	//private GUIStyle style;

	private int cullingMask;
	private CameraClearFlags clearFlags;

	void Awake(){
		cullingMask = camera.cullingMask;
		clearFlags = camera.clearFlags;
	}
	
	void Start ()
	{
		noStyle = new GUIStyle ();
		noStyle.onActive.background = null;
		noStyle.onNormal.background = null;

		//style = new GUIStyle();
		
//#if UNITY_EDITOR	
//#elif UNITY_IPHONE
//		camera.enabled = false;
//#endif

	}

	private void On(bool f){
		if(!f){
			camera.clearFlags = CameraClearFlags.Depth;
			camera.cullingMask = 0;
		}else{
			camera.clearFlags = clearFlags;
			camera.cullingMask = cullingMask;
		}
	}
	
	[HideInInspector]
	public CameraSwitcher switcher;
	[HideInInspector]
	public bool isMain = false;

	IEnumerator OnRoutine(){
		yield return new WaitForEndOfFrame();
		if(!isMain){
			On(false);
		}else{
			On(true);
		}
	}

	public void OnOff(){
		StartCoroutine(OnRoutine());
	}
	
	void OnGUI ()
	{

		if (!isMain) {
//			#if UNITY_EDITOR
			if (GUI.Button (
				new Rect (camera.rect.x * Screen.width, Screen.height - camera.rect.y * Screen.height - camera.rect.height * Screen.height, camera.rect.width * Screen.width, camera.rect.height * Screen.height), 
				"", noStyle
				)
				) {
 				switcher.Switch (this);
			}
//			#elif UNITY_IPHONE
//			if (GUI.Button (
//				new Rect (camera.rect.x * Screen.width, Screen.height - camera.rect.y * Screen.height - camera.rect.height * Screen.height, camera.rect.width * Screen.width, camera.rect.height * Screen.height), 
//				camera.name
//				)
//			    ) {
//				switcher.main.camera.enabled = false;
//				switcher.Switch (this);
//				camera.enabled = true;
//			}
//			camera.enabled = false;
//			#endif
		}else{
			camera.enabled = true;
		}
	}
}
