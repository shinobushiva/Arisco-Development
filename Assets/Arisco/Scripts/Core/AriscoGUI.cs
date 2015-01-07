using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class AriscoGUI : SingletonMonoBehaviour<AriscoGUI>
{

	public class WindowHandler
	{
		public Rect rect;

		private bool scroll;
		public bool Scroll {
			get {
				return scroll;
			}
			set {
				scroll = value;
			}
		}
		
		private bool showHide = false;
		public bool ShowHide {
			get {
				return showHide;
			}
			set {
				showHide = value;
				if (parent != null) {
					parent.Disabled = showHide;
				}
			}
		}

		private bool disabled = false;

		public bool Disabled {
			get {
				return disabled;
			}
			set {
				disabled = value;
			}
		}

		public WindowHandler parent;
	}

	private Dictionary<string, object> values = new Dictionary<string, object> ();
	private Dictionary<string, string> disps = new Dictionary<string, string> ();
	private Dictionary<string, string> errors = new Dictionary<string, string> ();
	private List<Action<int>> funcs = new List<Action<int>> ();
	private List<Action<int>> windowFuncs = new List<Action<int>> ();
	//private Dictionary<string,Action<int>> funcMap = new Dictionary<string,Action<int>> ();
	private Dictionary<string,Action<int>> windowFuncMap = new Dictionary<string,Action<int>> ();
	private List<Action<int>> guiFuncs = new List<Action<int>> ();

	public void AddGUIFunc (Action<int> f)
	{
		guiFuncs.Add (f);
	}
	
	public T Get<T> (string attr, T def)
	{	
		if (!values.ContainsKey (attr) || values [attr] == null) {
			return (T)def;
		}
		
		if (!(values [attr] is T)) {
			Debug.Log ("Class Cast Problem : from " + values [attr].GetType ());
			return def;
		}


		return (T)values [attr];
	}

	public WindowHandler Window (string title, int wid,
	                             Action<List<Action<int>>> f, WindowHandler parent = null, bool scroll=true)
	{
		List<Action<int>> funcs = new List<Action<int>> ();
		f (funcs);

		WindowHandler handler = new WindowHandler ();
		handler.parent = parent;
		handler.Scroll = scroll;
		Rect r = handler.rect;

		GUIStyle style = new GUIStyle ();
		style.fontSize = 12;
		style.active.textColor = Color.white;
		style.normal.textColor = Color.white;
		Vector2 scrollPosition = Vector2.zero;
		
		ResizeWindow resizeW = new ResizeWindow (10);

		bool resizing = false;
		GUI.WindowFunction wFunc = (windowID) => {

			if (handler.Disabled)
				GUI.enabled = false;

			if (GUI.Button (new Rect (10, 3, 24, 18), "X", style)) {
				handler.ShowHide = false;
			}
			GUILayout.BeginVertical (GUILayout.ExpandWidth (true), 
			                         GUILayout.MinHeight (handler.rect.height - 20 - 10)
			);
			if (handler.Scroll) {
				scrollPosition = GUILayout.BeginScrollView (scrollPosition);
			}
			foreach (Action<int> ff in funcs) {
				ff (120);
			}
			if (handler.Scroll) {
				GUILayout.EndScrollView ();
			}
			GUILayout.EndVertical ();

			//Resize.ResizeF (ref handler.rect);
			resizeW.ResizeXF(ref handler.rect);
			resizeW.ResizeDownF(ref handler.rect);
			handler.rect.width = Mathf.Min(handler.rect.width, (Screen.width/GUI_SCALE));
			handler.rect.height = Mathf.Min(handler.rect.height, (Screen.height/GUI_SCALE));
			if (!resizeW.ResizeUp && !resizeW.ResizeDown && !resizeW.ResizeLeft && !resizeW.ResizeRight) {
				GUI.DragWindow ();
			}

			GUI.enabled = true;
		};

		handler.rect = new Rect (Screen.width*0.2f/GUI_SCALE, Screen.height*0.1f/GUI_SCALE, Screen.width*0.6f/GUI_SCALE, Screen.height*0.8f/GUI_SCALE);
		
		Action<int> convert = (width) => {
			if (handler.ShowHide) {
				if (handler.parent == null && handler.ShowHide)
					GUI.enabled = false;
				if (parent == null && GUILayout.Button (title)) {
					handler.ShowHide = false;
				}
				GUI.enabled = true;
		
				if (handler.Disabled)
					GUI.enabled = false;
				handler.rect = GUILayout.Window (wid, handler.rect, wFunc, title);
				GUI.enabled = true;

			} else {

				if (handler.parent == null && handler.ShowHide)
					GUI.enabled = false;
				if (parent == null && GUILayout.Button (title)) {
					if (handler.rect.height <= 60) {
						handler.rect.height = Screen.height * 0.8f;
					}
					if (handler.rect.x >= Screen.width*0.8f/GUI_SCALE) {
						//handler.rect.x = Screen.width * 0.5f;
						handler.rect = new Rect (Screen.width*0.2f/GUI_SCALE, Screen.height*0.1f/GUI_SCALE, Screen.width*0.6f/GUI_SCALE, Screen.height*0.8f/GUI_SCALE);
					}
					if (handler.rect.y >= Screen.height*0.8f/GUI_SCALE) {
						//handler.rect.y = Screen.height * 0.5f;
						handler.rect = new Rect (Screen.width*0.2f/GUI_SCALE, Screen.height*0.1f/GUI_SCALE, Screen.width*0.6f/GUI_SCALE, Screen.height*0.8f/GUI_SCALE);
					}

					handler.ShowHide = true;
				}
				GUI.enabled = true;
			}

		};

		if (windowFuncMap.ContainsKey (title)) {
			windowFuncs.Remove (windowFuncMap [title]);
		}
		windowFuncMap [title] = convert;
		windowFuncs.Add (convert);
		
		return handler;
	}

	public void Button (string attr, Action f, List<Action<int>> funcs)
	{
	
		Action<int> convert = (width) => {
			if (GUILayout.Button (attr)) {
				f ();
			}
		};
		AddFunc (attr, convert, funcs);
	}

	public void TextField (string attr, string val, List<Action<int>> funcs)
	{
		val = Get<string> (attr, val);
	
		values [attr] = val;
		errors [attr] = null;
		disps [attr] = "" + val;
	
		Action<int> convert = (width) => {
			GUILayout.BeginHorizontal ();
			GUILayout.Box (attr, GUILayout.Width (width));
			val = GUILayout.TextField (val);
			values [attr] = val;
			GUILayout.EndHorizontal ();
		};
		AddFunc (attr, convert, funcs);
	}

	public void BoolField (string attr, bool val)
	{
		BoolField (attr, val, this.funcs);
	}

	public void BoolField (string attr, bool val, List<Action<int>> funcs, Action<bool> callback = null)
	{
		val = Get<bool> (attr, val);
		
		values [attr] = val;
		errors [attr] = null;
		disps [attr] = "" + val;
		
		Action<int> convert = (width) => {
			GUILayout.BeginHorizontal ();
			GUILayout.Box (attr, GUILayout.Width (width));
			val = GUILayout.Toggle (val, val ? "On" : "Off");
			values [attr] = val;
			GUILayout.EndHorizontal ();

			if (callback != null)
				callback (val);
		};
		AddFunc (attr, convert, funcs);

	}

	public void IntField (string attr, int val, List<Action<int>> funcs)
	{
		IntField (attr, val, int.MinValue, int.MaxValue, funcs);
	}

	public void IntField (string attr, int val, int min, int max)
	{
		IntField (attr, val, min, max, this.funcs);
	}
	
	public void IntField (string attr, int val, int min, int max, List<Action<int>> funcs)
	{
		val = Get<int> (attr, val);
	
		values [attr] = val;
		errors [attr] = null;
		disps [attr] = "" + val;
	
		Action<int> convert = (width) => {
			GUILayout.BeginHorizontal ();
			GUILayout.Box (attr, GUILayout.Width (width));
		
			Color org = GUI.color;
			if (errors [attr] != null)
				GUI.color = Color.red;
			disps [attr] = GUILayout.TextField (disps [attr]);
			GUI.color = org;
		
			errors [attr] = null;
		
			if (disps [attr].Trim ().Length == 0) {
				values [attr] = 0;
			} else {
				int a = 0;
				if (int.TryParse (disps [attr], out a)) {
					val = a;
				} else {
					errors [attr] = "Number format error";
				}
				if (val > max) {
					val = max;
					errors [attr] = "The value should be less than " + max;
				} else if (val < min) {
					val = min;
					errors [attr] = "The value should be more than " + min;
				}
				values [attr] = val;
			}
			GUILayout.EndHorizontal ();
		};
		AddFunc (attr, convert, funcs);
	}
	
	public void FloatField (string attr, float val, List<Action<int>> funcs)
	{
		FloatField (attr, val, float.MinValue, float.MaxValue, funcs);
	}
	
	public void FloatField (string attr, float val, float min, float max)
	{
		FloatField (attr, val, min, max, this.funcs);
	}
	
	public void FloatField (string attr, float val, float min, float max, List<Action<int>> funcs)
	{
		val = Get<float> (attr, val);
	
		values [attr] = val;
		errors [attr] = null;
		disps [attr] = "" + val;
	
		Action<int> convert = (width) => {
			GUILayout.BeginHorizontal ();
			GUILayout.Box (attr, GUILayout.Width (width));
	
			Color org = GUI.color;
			if (errors [attr] != null)
				GUI.color = Color.red;
			disps [attr] = GUILayout.TextField (disps [attr]);
			GUI.color = org;
	
			errors [attr] = null;
	
			if (disps [attr].Trim ().Length == 0) {
				values [attr] = 0;
			} else {
				float a = 0;
				if (float.TryParse (disps [attr], out a)) {
					val = a;
				} else {
					errors [attr] = "Number format error";
				}
				if (val > max) {
					val = max;
					errors [attr] = "The value should be less than " + max;
				} else if (val < min) {
					val = min;
					errors [attr] = "The value should be more than " + min;
				}
				values [attr] = val;
			}
			GUILayout.EndHorizontal ();
		};
		AddFunc (attr, convert, funcs);
	}

	public void SelectionBox (string attr, bool multiple, string[] paramStrs, bool[] vals, List<Action<int>> funcs)
	{
	
		//values [attr] = val;
		errors [attr] = null;
		//disps [attr] = "" + val;

		if (multiple) {
			List<int> storedVals = Get<List<int>> (attr, new List<int> ());
			List<bool> selected = new List<bool> ();
			if (storedVals.Count > 0)
				for (int i=0; i<storedVals.Count; i++) {
					if (storedVals.Contains (i)) {
						selected.Add (true);
					} else {
						selected.Add (false);
					}
				}
			else
				selected = new List<bool> (vals);
			
			if (selected.Count < paramStrs.Length) {
				for (int i=selected.Count; i<paramStrs.Length; i++) {
					selected.Add (false);
				}
			}
		
			List<int> idxes = new List<int> ();
			Action<int> convert = (width) => {
				idxes.Clear ();
				GUILayout.BeginVertical ();
				GUILayout.Box (attr);
				for (int i = 0; i<paramStrs.Length; i++) {
					bool change = GUILayout.Toggle (selected [i], paramStrs [i]);
					selected [i] = change;
					if (selected [i]) {
						idxes.Add (i);
					}
				}
				GUILayout.EndVertical ();
				values [attr] = idxes;
			};
			AddFunc (attr, convert, funcs);
		} else {
			int selected = Get<int> (attr, 0);
			Action<int> convert = (width) => {
				GUILayout.BeginVertical ();
				GUILayout.Box (attr);
				for (int i = 0; i<paramStrs.Length; i++) {
					bool change = GUILayout.Toggle (i == selected, paramStrs [i]);
					if (change)
						selected = i;
				}
				GUILayout.EndVertical ();
				values [attr] = selected;
			};
			
			//if(funcMap.ContainsKey(attr)){
			AddFunc (attr, convert, funcs);
		}
	}

	private void AddFunc (string attr, Action<int> convert, List<Action<int>> funcs)
	{
		funcs.Add (convert);
	}

	void DoMyWindow (int windowID)
	{
		GUILayout.BeginVertical (GUILayout.MinHeight (Screen.height * 0.8f), GUILayout.ExpandWidth (true));
		foreach (Action<int> f in funcs) {
			f ((int)(Screen.width*0.5f/GUI_SCALE));
		}
		GUILayout.EndVertical ();
	
		GUI.DragWindow ();
	}
	
	//bool open = false;

	void Awake ()
	{
		AriscoGUI.Instance.Window ("Control", 0, 
			(funcs) => {
			funcs.Add ((width) => {
				DoMyWindow (0);
			});
		}, null);
	}

	private static float guiScale = 1.0f;
	public static float GUI_SCALE {
		get {
			guiScale = Screen.width / 800f;
			return guiScale;
		}
	}

	float lastMoveTime;
	Vector3 lastMousePosition;

	void OnGUI ()
	{
		if (Vector3.Distance (Input.mousePosition, lastMousePosition) > 1) {
			lastMoveTime = Time.time;
		}
		lastMousePosition = Input.mousePosition;

		if (Time.time - 1 > lastMoveTime) {
			return;
		}

		
		Matrix4x4 mat = GUI.matrix;

		GUI.matrix = Matrix4x4.Scale (Vector3.one * GUI_SCALE);
	
		GUILayout.BeginHorizontal (GUILayout.MinWidth (Screen.width / GUI_SCALE));
		GUILayout.FlexibleSpace ();
		GUILayout.BeginVertical ();
		foreach (Action<int> f in windowFuncs) {
			f ((int)(Screen.width*0.5f/GUI_SCALE));
		}
		GUILayout.EndVertical ();
		GUILayout.EndHorizontal ();

		foreach (Action<int> f in guiFuncs)
			f (0);
	
		GUI.matrix = mat;
	
	}

	/*
	public void Setup (World w)
	{
		print ("ParameterControl#Setup : " + w.name);
		funcs.Clear ();
		windowFuncs.Clear ();
	}
	*/
	
}
