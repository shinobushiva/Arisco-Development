using UnityEngine;
using System.Collections;
 
[ExecuteInEditMode]
public class PlaceIndicator : MonoBehaviour
{
	private CameraSwitcher switcher;
	public Color color = Color.red;
	public float radiusPerScreen = 0.01f;
	public string text = "text here";
	public bool showText = true;
	public GUISkin skin;
	public Texture2D rt;
	public bool show = true;
	public bool forceShowInMainCamera = false;
	public Alignment alignment = Alignment.Center;
	[System.Serializable]
	public enum Alignment
	{
		Center,
		TopLeft,
		TopRight,
		BottomLeft,
		BottomRight
	}

	// Use this for initialization
	void Start ()
	{
		if (!rt) {
			rt = new Texture2D (1, 1, TextureFormat.ARGB32, false);
			rt.SetPixels (new Color[]{color});
			rt.Apply ();
		}
		if (PlaceIndicatorControl.Instance)
			show = PlaceIndicatorControl.Instance.showHide;

	}

	bool mouseOn = false;

	void Update ()
	{
		bool b = mouseOn;
		Vector3 mp = Input.mousePosition;
		if (textureRect.Contains (new Vector2 (mp.x, Screen.height - mp.y))) {
			b = true;
		} else {
			b = false;
		}
		if (mouseOn != b) {
			if (b) {
				if (!mouseOn)
					SendMessage ("OnMouseEnter", SendMessageOptions.DontRequireReceiver);
			} else {
				SendMessage ("OnMouseExit", SendMessageOptions.DontRequireReceiver);
			}
		} else {
			if (mouseOn)
				SendMessage ("OnMouseOver", SendMessageOptions.DontRequireReceiver);
		}
		mouseOn = b;
	}

	private Rect textureRect;

	void OnGUI ()
	{
		if (switcher == null) {
			switcher = FindObjectOfType<CameraSwitcher> ();
		}
		if (switcher == null)
			return;

		Camera c = switcher.main.camera;

		if (show && c) {

			int idx = System.Array.IndexOf (PlaceIndicatorControl.Instance.cameraNamesToShow, c.name);
			if (!forceShowInMainCamera && idx < 0) {
				return;
			}
			if (!(forceShowInMainCamera && c.tag == "MainCamera") && idx >= 0 &&
				Vector3.Distance (transform.position, c.transform.position) > PlaceIndicatorControl.Instance.cameraDistanceToShow [idx]) {
				return;
			}
				
			Plane[] planes = GeometryUtility.CalculateFrustumPlanes (c);
			if (!GeometryUtility.TestPlanesAABB (planes, new Bounds (transform.position, Vector3.one))) {
				return;
			}


			GUI.skin = skin;
			Vector3 p = c.WorldToScreenPoint (transform.position);
			p.y = Screen.height - p.y;

			float rw = Screen.width * radiusPerScreen;
			float rh = rt.height / (float)rt.width * rw;
			/*
			rw *= camera.rect.width;
			rh *= camera.rect.height;
			*/

			Vector2 off = Vector2.zero;
			switch (alignment) {
			case Alignment.Center:
				break;
			case Alignment.TopLeft:
				off.x = rw / 2;
				off.y = rh / 2;
				break;
			case Alignment.TopRight:
				off.x = -rw / 2;
				off.y = rh / 2;
				break;
			case Alignment.BottomLeft:
				off.x = rw / 2;
				off.y = -rh / 2;
				break;
			case Alignment.BottomRight:
				off.x = -rw / 2;
				off.y = -rh / 2;
				break;
			}

			textureRect = new Rect (p.x - rw / 2 + off.x, p.y - rh / 2 + off.y, rw, rh);

			GUI.DrawTexture (textureRect, rt);

			idx = System.Array.IndexOf (PlaceIndicatorControl.Instance.cameraNamesToShow, c.name);
			if (idx >= 0) {
				if (showText)
					GUI.Label (new Rect (p.x - rw / 2, p.y - rw / 2, rw, rw), text);
			}
		}
	}
}
