using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Collections;

public class NewAgentBehavior : ScriptableWizard
{

		public string scriptName = "NewABehavior";

		[MenuItem("Arisco/New Agent Behavior Script")]
		public static void _NewAgentBehavior ()
		{
				ScriptableWizard.DisplayWizard<NewAgentBehavior> ("Create New Agent Behavior Script", "Create");
		}

		void OnWizardCreate ()
		{
				//Debug.Log ("OnWizardCreate");

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

		void OnWizardUpdate ()
		{
				//Debug.Log ("OnWizardUpdate");
		}

		void OnWizardOtherButton ()
		{
				//Debug.Log ("OnWizardOtherButton");
		}

}
