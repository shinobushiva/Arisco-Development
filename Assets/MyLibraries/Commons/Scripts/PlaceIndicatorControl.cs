using UnityEngine;
using System.Collections;

public class PlaceIndicatorControl : SingletonMonoBehaviour<PlaceIndicatorControl>
{

	private bool _showHide = true;
	public bool showHide = true;

	public string[] cameraNamesToShow;
	public float[] cameraDistanceToShow;

	void Start ()
	{
		if(cameraNamesToShow.Length == 0){
			cameraNamesToShow = new string[]{Camera.main.name};
		}
		CheckAndChange ();
	}

	// Update is called once per frame
	void Update ()
	{
		CheckAndChange ();

	}

	void CheckAndChange ()
	{
		if (showHide != _showHide) {
			
			PlaceIndicator[] pis = FindObjectsOfType (typeof(PlaceIndicator)) as PlaceIndicator[];
			foreach (PlaceIndicator pi in pis) {
				pi.show = showHide;
			}
			
			_showHide = showHide;
		}
	}
}
