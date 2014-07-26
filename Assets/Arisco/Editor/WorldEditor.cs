using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Reflection;
using System;

[CustomEditor(typeof(World))]
public class WorldEditor : Editor {
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		EditorGUILayout.BeginHorizontal();
		/*
		if(GUILayout.Button("Add New World Behavior")){
		}
		*/
		EditorGUILayout.EndHorizontal();
	}
}
