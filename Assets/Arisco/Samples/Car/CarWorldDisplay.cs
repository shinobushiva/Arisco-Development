using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarWorldDisplay : WorldBehavior
{

	public override void Initialize ()
	{
		base.Initialize ();

		AriscoChart.Instance.AddChart("speed", "Total Speed", AriscoChart.ChartType.Line, 100, 100);


		speedSumList = new List<float> ();

	}

	public float speedSum;
	public float speedSumMax;
	public List<float> speedSumList;

	public override void Step ()
	{
		speedSum = 0;


		List<AAgent> list = AttachedWorld.AllAgents;
		foreach (AAgent a in list) {
			SpeedDirectionBehavior sdb = a.GetComponent<SpeedDirectionBehavior> ();
			speedSum += sdb.Speed;
		}
		speedSum /= list.Count;

		if (speedSumList.Count >= 1000) {
			speedSumList.RemoveAt (0);
		}

		speedSumList.Add (speedSum);

		speedSumMax = Mathf.Max (speedSum, speedSumMax);

	}

	public override void Commit ()
	{
		/*
		if(ChartPlot.Instance)
			ChartPlot.Instance.Repaint();
		*/

		if (speedSumList.Count > 2) {
			List<object> titles = new List<object> (){
				"Step", "Speed"
			};

			List<List<object>> values = new List<List<object>> ();
			for (int i=0; i< speedSumList.Count; i++) {
				float f = speedSumList [i];
				values.Add (new List<object> (){i, f});
			}
			if(AriscoChart.Instance)
				AriscoChart.Instance.SetDataString ("speed",
					AriscoChart.Instance.ToDataString(titles, values)
				);
		}
	}


}


