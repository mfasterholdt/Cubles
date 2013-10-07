using UnityEngine;
using System.Collections;

public class EnvironmentTile : WorldObject 
{
	public Color gizmoColour = new Color(0.1f, 0.35f, 0.9f, 0.5f);
	protected Vector3 gizmoSize = new Vector3(1, 0.4f, 1);
	
	void OnDrawGizmos()
	{
		if(!Application.isPlaying)
		{
			Gizmos.color = gizmoColour;
			Vector3 pos = transform.position;
			pos.y = -0.5f; 
			Gizmos.DrawCube(pos, gizmoSize);
		}
	}
}
