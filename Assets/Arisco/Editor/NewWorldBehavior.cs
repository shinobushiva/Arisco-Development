using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Collections;

public class NewWorldBehavior : ScriptableWizard
{

		public string scriptName = "NewWBehavior";

		[MenuItem("Arisco/New World Behavior Script")]
		public static void _NewWorldBehavior ()
		{
				ScriptableWizard.DisplayWizard<NewWorldBehavior> ("Create New World Behavior Script", "Create");
		}

		void OnWizardCreate ()
		{
				//Debug.Log ("OnWizardCreate");

				string scene = EditorApplication.currentScene;
				int idx = scene.LastIndexOf ("/");
				string path = scene.Substring (0, idx);
				//string sname = scene.Substring(idx+1, scene.Length-idx-1);

				TextAsset ta = Resources.Load ("WorldBehavior_Template", typeof(TextAsset)) as TextAsset;
				string text = ta.text.Replace("<CLASS_NAME>", scriptName);

				Debug.Log (path);
				Debug.Log (text);

				File.WriteAllText (path + "/" + scriptName + ".cs", text);
				AssetDatabase.Refresh ();

		}

		void OnWizardUpdate ()
		{
				//Debug.Log ("OnWizardUpdate");
		}

		void OnWizardOtherButton ()
		{
				//Debug.Log ("OnWizardOtherButton");
		}
	
}
