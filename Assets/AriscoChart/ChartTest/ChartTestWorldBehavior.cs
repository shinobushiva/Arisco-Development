using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChartTestWorldBehavior : WorldBehavior
{
	
	void Initialize ()
	{
		print ("ChartTestWorldBehavior#Initialize");
		AriscoChart.Instance.AddLibraries("google.load('visualization', '1', {'packages': ['geochart']});");
		AriscoChart.Instance.AddChart ("chart_0", "Line Chart", AriscoChart.ChartType.Line, 100, 50);
		AriscoChart.Instance.AddChart ("chart_1", "Geo Chart", AriscoChart.ChartType.Pie, 100, 50);
		AriscoChart.Instance.SetOptionString (
			"chart_1", 
            @"is3D: true,
			 slices: {  1: {offset: 0.2},
                    2: {offset: 0.3},
                    3: {offset: 0.4},
                    5: {offset: 0.5},
          	 },
			"
		);
		//AriscoChart.Instance.AddChart ("chart_1", "Geo Chart", "google.visualization.GeoChart", 100, 50);

	}

	void Commit ()
	{
		string d1 = AriscoChart.Instance.ToDataString (
			new List<object> (){
	             "Year", "Sales", "Expenses"
			},
		new List<List<object>> (){
			new List<object>(){"2004",  1000, 400},
			new List<object>(){"2005",  1170, 460},
			new List<object>(){"2006",  660,  1120},
			new List<object>(){"2007",  1030, 540}
			}
		);

		string d2 = AriscoChart.Instance.ToDataString (
			new List<object> (){
	             "Task", "Hours per Day"
			},
			new List<List<object>> (){
				new List<object>(){"Work",  11},
				new List<object>(){"Eat",  2},
				new List<object>(){"Commute",  2},
				new List<object>(){"Watch TV",  2},
				new List<object>(){"Sleep",  7}
			}
		);
		/*
		d2 = @"[
          ['Country', 'Popularity'],
          ['Germany', 200],
          ['United States', 300],
          ['Brazil', 400],
          ['Canada', 500],
          ['France', 600],
          ['RU', 700]
        ]";
		 */

		AriscoChart.Instance.SetDataString ("chart_0", d1);
		AriscoChart.Instance.SetDataString ("chart_1", d2);

	}
}
