using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;

public class AriscoChart : SingletonMonoBehaviour<AriscoChart>
{

	public enum ChartType 
	{
		Line,
		Pie,
	}
	
	private UWKWebView view;

	public bool show = true;
	private float nextDraw = 0;
	private AriscoGUI.WindowHandler handler;

	private bool repainting = false;
	private Rect preRect;

	void Start ()
	{	
		view = GetComponent<UWKWebView> ();
		print ("ProcessStarted");

		if (!show)
			return;

		view.LoadFinished += LoadFinished;
		html = Resources.Load<TextAsset> ("linechart").text;
		view.LoadHTML (html);

		handler = AriscoGUI.Instance.Window ("Arisco Chart", 10, 
			(funcs) => {
			funcs.Add ((width) => {
				if (!view)
					return;

				float mag = AriscoGUI.GUI_SCALE;
				GUILayout.Label(view.WebTexture);
				//view.Transparency = transparency;
			});
		}, null);

		
		repainting = false;

	}


	// transparency
	public float transparency = 100.0f;
	[Range(0.1f, 1f)]
	public float
		refreshRate = 0.5f;
	private string lineChartURL;
	private string html;
	private List<string> ids = new List<string> ();
	private List<string> divs = new List<string> ();
	private List<string> titles = new List<string> ();
	private List<string> types = new List<string> ();
	private List<string> dataStrings = new List<string> ();

	//
	private List<string> librariesToLoad = new List<string> ();
	private List<string> options = new List<string> ();

	public void AddLibraries (string lib)
	{
		librariesToLoad.Add (lib);
	}

	public void AddChart (string id, string title, ChartType t, int w=100, int h=100)
	{
		if (t == ChartType.Line)
			AddChart (id, title, "google.visualization.LineChart", w, h);
		else if (t == ChartType.Pie)
			AddChart (id, title, "google.visualization.PieChart", w, h);
	}

	public void AddChart (string id, string title, string t, int w=100, int h=100)
	{
		string div = "<div id='" + id + "' style='width: " + w + "%; height: " + h + "%;'></div>";
		ids.Add (id);
		divs.Add (div);
		titles.Add (title);
		dataStrings.Add ("[]");
		options.Add ("");
		types.Add (t);
	}

	public void SetOptionString (string id, string option)
	{
		int i = ids.IndexOf (id);
		options [i] = option;
	}

	public void SetDataString (string id, string dataString)
	{
		int i = ids.IndexOf (id);
		dataStrings [i] = dataString;

		Repaint ();
	}

	public void Init ()
	{
		ids.Clear ();
		divs.Clear ();
		titles.Clear ();
		dataStrings.Clear ();
		librariesToLoad.Clear ();
	}

	private string CreateHTML ()
	{
		StringBuilder build = new StringBuilder ();
		for (int i = 0; i < ids.Count; i++) {
			string buf = @"
			{
				var data = google.visualization.arrayToDataTable(
          			#data_array#
        		);
			
				var options = {
					title: '#title#',
					#options#
				};
				
				var chart = new #chart_type#(document.getElementById('#chart_div#'));
				chart.draw(data, options);
			}
			";
			buf = buf.Replace ("#title#", titles [i]);
			buf = buf.Replace ("#chart_div#", ids [i]);
			buf = buf.Replace ("#chart_type#", types [i]);
			buf = buf.Replace ("#data_array#", dataStrings [i]);
			buf = buf.Replace ("#options#", options [i]);
			build.Append (buf);
		}
		string h = html.Replace ("#chart_data#", build.ToString ());
		build = new StringBuilder ();
		foreach (string lib in librariesToLoad) {
			build.Append (lib);
		}
		h = h.Replace ("#libraries_to_load#", build.ToString ());
		build = new StringBuilder ();
		build.Append ("<div>");
		for (int i = 0; i < ids.Count; i++) {
			build.Append (divs [i]);
		}
		build.Append ("</div>");
		h = h.Replace ("#chart_divs#", build.ToString ());
		return h;
	}

	private void Repaint ()
	{
		if (!view)
			return;

		if (repainting)
			return;

		repainting = true;
		preRect = handler.rect;

		//print ("Repaint");
		string h = CreateHTML ();
		view.CurrentWidth = (int)handler.rect.width;
		view.CurrentHeight = (int)handler.rect.height;
		view.LoadHTML (h);

		//image.texture = view.WebTexture;

	}

	public string ToDataString (List<object> headers, List<List<object>> values)
	{

		StringBuilder buf = new StringBuilder ();
		buf.Append ("[");
		buf.AppendList (headers);
		foreach (List<object> lf in values) {
			buf.Append (",");
			buf.AppendList (lf);
		}
		buf.Append ("]");

		return buf.ToString ();
	}

	// Delegate called when a tab's page is loaded
	void LoadFinished (UWKWebView view)
	{
		print ("LoadFinished");
		repainting = false;
	}
}

static class ListExtensions
{
	public static void AppendList (this StringBuilder buf, List<object> l)
	{
		
		buf.Append ("[");
		foreach (object v in l) {
			if (v is string)
				buf.Append ("'").Append (v).Append ("',");
			else
				buf.Append (v).Append (",");
		}
		buf.Append ("]");
	}

}

