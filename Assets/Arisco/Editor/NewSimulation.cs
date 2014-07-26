using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Collections;

public class NewSimulation : ScriptableWizard
{

		public string simulationName = "New Simulation";

		[MenuItem("Arisco/New Simulation")]
		public static void _NewSimulation ()
		{
			ScriptableWizard.DisplayWizard<NewSimulation> ("Create New Simulation", "Create");
		}

		void OnWizardCreate ()
		{

				string p = GetPath ("NewSimulation.cs");
				//Debug.Log (p);
				string pSystemDefault = p.Replace ("Editor/NewSimulation.cs", "Prefabs/SystemDefault.prefab");
				string pWorld = p.Replace ("Editor/NewSimulation.cs", "Prefabs/World.prefab");
				string pAgent = p.Replace ("Editor/NewSimulation.cs", "Prefabs/Agent.prefab");

				EditorApplication.SaveCurrentSceneIfUserWantsTo ();
				EditorApplication.NewScene ();

				string guid = AssetDatabase.CreateFolder ("Assets", simulationName);
				string newFolderPath = AssetDatabase.GUIDToAssetPath (guid);
				//Debug.Log (newFolderPath);
				int anchor = newFolderPath.LastIndexOf ("/");

				string newName = newFolderPath.Substring (anchor + 1, newFolderPath.Length - anchor - 1);
				string pname = newName.Replace (" ", "");
		
				EditorApplication.SaveScene (newFolderPath + "/" + newName + ".unity");

				DestroyImmediate(GameObject.FindWithTag("MainCamera"));

				World world = null;
				{
						string pp = newFolderPath + "/" + pname + "World.prefab";
						AssetDatabase.CopyAsset (pWorld, pp);
						AssetDatabase.Refresh ();
						EditorApplication.SaveAssets();
						GameObject g = (GameObject)AssetDatabase.LoadAssetAtPath (pp, typeof(GameObject));
						GameObject w = (GameObject)PrefabUtility.InstantiatePrefab (g);
						world = w.GetComponent<World> ();
				}
				EditorApplication.SaveScene ();

				{
						string pp = newFolderPath + "/" + pname + "Agent.prefab";
						AssetDatabase.CopyAsset (pAgent, pp);
						AssetDatabase.Refresh ();
						EditorApplication.SaveAssets();
						GameObject g = (GameObject)AssetDatabase.LoadAssetAtPath (pp, typeof(GameObject));
						GameObject a = (GameObject)PrefabUtility.InstantiatePrefab (g);
						//Debug.Log (a.transform.parent);
						a.transform.parent = world.transform;
						//Debug.Log (a.transform.parent);
				}
				EditorApplication.SaveScene ();

		GameObject systemDefault = null;
		{
			GameObject sd = (GameObject)AssetDatabase.LoadAssetAtPath (pSystemDefault, typeof(GameObject));
			systemDefault = (GameObject)PrefabUtility.InstantiatePrefab (sd);
			systemDefault.name = "System";
		}
		systemDefault.GetComponentInChildren<AgentWorldRun> ().world = world;
		EditorApplication.SaveScene ();
				

		}

		void OnWizardUpdate ()
		{
				//Debug.Log ("OnWizardUpdate");
		}

		void OnWizardOtherButton ()
		{
				//Debug.Log ("OnWizardOtherButton");
		}


		/// <summary>
		/// Used to get assets of a certain type and file extension from entire project
		/// </summary>
		/// <param name="type">The type to retrieve. eg typeof(GameObject).</param>
		/// <param name="fileExtension">The file extention the type uses eg ".prefab".</param>
		/// <returns>An Object array of assets.</returns>
		public static string GetPath (string filename)
		{
				DirectoryInfo directory = new DirectoryInfo (Application.dataPath);
				FileInfo[] goFileInfo = directory.GetFiles (filename, SearchOption.AllDirectories);
		
				int i = 0;
				int goFileInfoLength = goFileInfo.Length;
				FileInfo tempGoFileInfo;
				string tempFilePath = null;
				for (; i < goFileInfoLength; i++) {
						tempGoFileInfo = goFileInfo [i];
						if (tempGoFileInfo == null)
								continue;
			
						tempFilePath = tempGoFileInfo.FullName;
						tempFilePath = tempFilePath.Replace (@"\", "/").Replace (Application.dataPath, "Assets");


				}
				return tempFilePath;
		}
}
