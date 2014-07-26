using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;

public abstract class AriscoChart  : SingletonMonoBehaviour<AriscoChart>
{


	public enum ChartType
	{
		Line,
		Pie,
	}

	public abstract void AddLibraries(string lib);

	public abstract void AddChart (string id, string title, ChartType t, int w=100, int h=100);
	
	public abstract void AddChart (string id, string title, string t, int w=100, int h=100);

	public abstract void SetOptionString (string id, string option);

	public abstract void SetDataString (string id, string dataString);

	public abstract void Init ();

	public abstract string ToDataString (List<object> headers, List<List<object>> values);


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

