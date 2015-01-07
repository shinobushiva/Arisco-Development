using UnityEngine;
using System.Collections; 
using UnityEngine.UI;

public class PlayControl : SingletonMonoBehaviour<PlayControl>
{

		public Button play;
		public Button pause;
		public Button step;
		public Button finish;
		public Canvas canvas;

		public void PlayClicked ()
		{
				AgentWorldRun.Instance.Play ();
		}

		public void PauseClicked ()
		{
				AgentWorldRun.Instance.Pause ();
		}

		public void StepClicked ()
		{
				AgentWorldRun.Instance.Step ();
		}

		public void StopClicked ()
		{
				AgentWorldRun.Instance.Stop ();
		}

		void Update ()
		{
				ManageButtons ();
		}

		public void OnShowUI ()
		{
				canvas.enabled = true;
		}

		public void OnHideUI ()
		{
				canvas.enabled = false;
		}

		void ManageButtons ()
		{
				if (!AgentWorldRun.Instance.runner.Running && !AgentWorldRun.Instance.runner.Finished) {
						play.interactable = true;
				} else {
						play.interactable = false;
				}

				if (AgentWorldRun.Instance.runner.Running) {
						pause.interactable = true;
						if (AgentWorldRun.Instance.runner.Paused) {
								pause.GetComponentInChildren<Text> ().text = "Un Pause";
						} else {
								pause.GetComponentInChildren<Text> ().text = "Pause";
						}
				} else {
						pause.interactable = false;
				}

				if (!AgentWorldRun.Instance.runner.Finished) {
						step.interactable = true;
				} else {
						step.interactable = false;
				}

				if (AgentWorldRun.Instance.runner.Running || AgentWorldRun.Instance.runner.Finished || !AgentWorldRun.Instance.runner.Started) {
						finish.interactable = true;
						if (AgentWorldRun.Instance.runner.Finished || !AgentWorldRun.Instance.runner.Started) {
								finish.GetComponentInChildren<Text> ().text = "Rebuild";
						} else {
								finish.GetComponentInChildren<Text> ().text = "Finish";
						}
				} else {
						finish.interactable = false;
				}


		}

		public bool useOldUI = true;
	
		void Start ()
		{
				if (AriscoGUI.Instance)
						AriscoGUI.Instance.AddGUIFunc (DrawGUI);
		}

		public void DrawGUI (int f)
		{

				if (!useOldUI)
						return;

				GUILayout.Window (80, new Rect (10, 10, 100, 150), (windowId) => {
						GUILayout.BeginVertical (GUILayout.ExpandWidth (true));

						GUI.enabled = !AgentWorldRun.Instance.runner.Running && !AgentWorldRun.Instance.runner.Finished;
						if (GUILayout.Button ("Play")) {
								AgentWorldRun.Instance.Play ();
						}

						GUI.enabled = AgentWorldRun.Instance.runner.Running;
						string text = "";
						if (AgentWorldRun.Instance.runner.Paused) {
								text = "Un Pause";
						} else {
								text = "Pause";
						}
						if (GUILayout.Button (text)) {
								AgentWorldRun.Instance.Pause ();
						}

						GUI.enabled = !AgentWorldRun.Instance.runner.Finished;
						if (GUILayout.Button ("Step")) {
								AgentWorldRun.Instance.Step ();
						}

						GUI.enabled = AgentWorldRun.Instance.runner.Running || AgentWorldRun.Instance.runner.Finished || !AgentWorldRun.Instance.runner.Started;
						if (AgentWorldRun.Instance.runner.Finished || !AgentWorldRun.Instance.runner.Started) {
								text = "Rebuild";
						} else {
								text = "Finish";
						}
						if (GUILayout.Button (text)) {
								AgentWorldRun.Instance.Stop ();
						}

						GUILayout.BeginHorizontal (GUILayout.ExpandWidth (true));
						float duration = AgentWorldRun.Instance.duration;
						duration = GUILayout.HorizontalSlider (duration, 0f, 1f, GUILayout.ExpandWidth (true));
						duration = ((int)(duration * 100)) / 100f;
						AgentWorldRun.Instance.duration = duration;
						GUILayout.Label (string.Format ("{0:f2}", duration), GUILayout.MaxWidth (20));
						GUILayout.EndHorizontal ();


						GUI.enabled = true;
						GUILayout.EndVertical ();
				}, "Play Control");
		}
	
}
