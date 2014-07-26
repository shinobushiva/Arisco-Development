using UnityEngine;
using UnityEditor;
using System.Collections;

public class RainTankMenu : Editor {

	public static int width = 50;
	public static int height = 50;

	[MenuItem("RainTank/CreateField")]
	public static void CreateField(){

		GameObject go = new GameObject("RainTankField");
		go.transform.position = Vector3.zero;

		for(int i=0;i<width;i++){
			for(int j=0;j<height;j++){
				GameObject f = GameObject.CreatePrimitive(PrimitiveType.Cube);
				f.transform.position = new Vector3(i-width/2 - 0.5f, 0, j-height/2 - 0.5f);
				f.transform.parent = go.transform;
				f.AddComponent<AAgent>();
				//f.renderer.material.shader = Shader.Find("Transparent/Diffuse");
				//HeatFieldBehavior rtf = f.AddComponent<HeatFieldBehavior>();
				//rtf.Amount = Random.Range(0, 100);

			}
		}

	}
}
