using UnityEngine;
using System.Collections;

public class SchelingWorldParameters : MonoBehaviour {

	// Use this for initialization
	void Start () {
		AriscoGUI.Instance.IntField ("Num", 8, 5, 100);
		AriscoGUI.Instance.FloatField ("Rate to Remove", 0.25f, 0f, 1f);
		AriscoGUI.Instance.FloatField ("Community Index", 0.66f, 0f, 1f);
		AriscoGUI.Instance.BoolField ("Randomize Order", true);
		AriscoGUI.Instance.BoolField ("All Agents at Once", false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
