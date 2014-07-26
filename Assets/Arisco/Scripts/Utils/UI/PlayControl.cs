using UnityEngine;
using System.Collections;

public class PlayControl : SingletonMonoBehaviour<PlayControl>
{

	/*
	public UIButton play;
	public UIButton pause;
	public UIButton stop;
	*/

	public void PlayClicked (GameObject g)
	{
		AgentWorldRun.Instance.Play ();
	}

	public void PauseClicked (GameObject g)
	{
		AgentWorldRun.Instance.Pause ();
	}

	public void StopClicked (GameObject g)
	{
		AgentWorldRun.Instance.Stop ();
	}

	void Start(){
		if(AriscoGUI.Instance)
			AriscoGUI.Instance.AddGUIFunc(DrawGUI);
	}


	public void DrawGUI(int f){

		GUILayout.Window(80, new Rect(10, 10, 100, 150), (windowId) =>{
			GUILayout.BeginVertical(GUILayout.ExpandWidth(true));

			GUI.enabled = !AgentWorldRun.Instance.runner.Running && !AgentWorldRun.Instance.runner.Finished;
			if(GUILayout.Button("Play")){
				AgentWorldRun.Instance.Play ();
			}

			GUI.enabled = AgentWorldRun.Instance.runner.Running;
			string text = "";
			if(AgentWorldRun.Instance.runner.Paused){
				text = "Un Pause";
			}else{
				text = "Pause";
			}
			if(GUILayout.Button(text)){
				AgentWorldRun.Instance.Pause ();
			}

			GUI.enabled  = !AgentWorldRun.Instance.runner.Finished;
			if(GUILayout.Button("Step")){
				AgentWorldRun.Instance.Step ();
			}

			GUI.enabled = AgentWorldRun.Instance.runner.Running || AgentWorldRun.Instance.runner.Finished || !AgentWorldRun.Instance.runner.Started;
			if(AgentWorldRun.Instance.runner.Finished || !AgentWorldRun.Instance.runner.Started){
				text = "Rebuild";
			}else{
				text = "Finish";
			}
			if(GUILayout.Button(text)){
				AgentWorldRun.Instance.Stop ();
			}

			GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
			float duration = AgentWorldRun.Instance.duration;
			duration = GUILayout.HorizontalSlider(duration, 0f, 1f, GUILayout.ExpandWidth(true));
			duration = ((int)(duration * 100))/100f;
			AgentWorldRun.Instance.duration = duration;
			GUILayout.Label(string.Format("{0:f2}",duration), GUILayout.MaxWidth(20));
			GUILayout.EndHorizontal();


			GUI.enabled = true;
			GUILayout.EndVertical();
		},"Play Control");
	}
}
