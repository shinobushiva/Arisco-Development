using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

[CustomEditor(typeof(AAgent))]
public class AAgentEditor : Editor {
	public string scriptName = "NewABehavior";

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		EditorGUILayout.BeginHorizontal();
		/*
		if(GUILayout.Button("Add New Agent Behavior")){
			ScriptableWizard.DisplayWizard<NewAgentBehavior> ("Create New Agent Behavior Script", "Create");
		}
		*/
		EditorGUILayout.EndHorizontal();
	}


	void OnWizardCreate ()
	{
		string scene = EditorApplication.currentScene;
		int idx = scene.LastIndexOf ("/");
		string path = scene.Substring (0, idx);
		//string sname = scene.Substring(idx+1, scene.Length-idx-1);
		
		TextAsset ta = Resources.Load ("AgentBehavior_Template", typeof(TextAsset)) as TextAsset;
		string text = ta.text.Replace("<CLASS_NAME>", scriptName);
		
		Debug.Log (path);
		Debug.Log (text);
		
		File.WriteAllText (path + "/" + scriptName + ".cs", text);
		AssetDatabase.Refresh ();

	}
}
