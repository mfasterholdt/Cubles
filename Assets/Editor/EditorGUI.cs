using UnityEngine;
using UnityEditor;
using System.Collections;

[InitializeOnLoad]
public static class EditorGUI 
{		
	static bool listening;
	
	static EditorGUI()
	{
		if(!listening)
		{
			
			SceneView.onSceneGUIDelegate += OnDrawGUI;
			
			listening = true;
		}
	}
	
	static GUISkin skin;
	
	static void OnDrawGUI(SceneView view)
	{
		if(skin == null) skin = GUI.skin;
		GUI.skin = skin;
		
		
		Level level = GameObject.FindObjectOfType(typeof(Level)) as Level;
		
		if(level == null) return;
		
		GUILayout.BeginArea(new Rect(20, 20, 120, 70));	
		
			string txt = "";
		
			Color defaultColor = GUI.backgroundColor;
		
			if(level.stepManually)
			{
				GUI.backgroundColor = Color.red;
				txt = "Paused";
			}
			else
			{
				GUI.backgroundColor = Color.green;
				txt = "Running : " + level.moveInterval;	
			}
		
			if(GUILayout.Button(txt))
			{
				level.stepManually = !level.stepManually;
				EditorUtility.SetDirty(level);
			}
		
			GUI.backgroundColor = defaultColor;
		
		GUILayout.EndArea();
	}
}
