using UnityEngine;
using System.Collections;

public class LifeGameWorldParameters : MonoBehaviour {

	// Use this for initialization
	void Start () {
		AriscoGUI.Instance.FloatField ("rate", 0.5f, 0, 1f);
		AriscoGUI.Instance.IntField ("num", 20, 3, 100);
		AriscoGUI.Instance.BoolField("torus", false);
	}

}
