using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UIHidder : MonoBehaviour{


	// Use this for initialization
	void Start () {
	
	}

    public float waitTime = 1;

    private float lastMoveTime;
    private Vector3 lastMousePosition;
    
    void Update ()
    {
        if (Vector3.Distance(Input.mousePosition, lastMousePosition) > waitTime)
        {
            lastMoveTime = Time.time;
        }
		if (Input.GetMouseButton (0)) {
			lastMoveTime = Time.time;
		}

        lastMousePosition = Input.mousePosition;
        
        if (Time.time - waitTime > lastMoveTime)
        {
            gameObject.SendMessage("OnHideUI",SendMessageOptions.DontRequireReceiver);
        } else
        {
            gameObject.SendMessage("OnShowUI",SendMessageOptions.DontRequireReceiver);
        }
    }
}
