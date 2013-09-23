using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class PatternTile : WorldObject 
{
	
	public enum Type { Flee, Follow, Gem, Rock, Waiter}
	public enum Shape { Square, Circle, Diamond }
	public Type type;
	
	private TileInfo info;
	
	void Start()
	{
		GameObject obj = Instantiate(PatternManager.Instance.GetTilePrefab(type), transform.position, Quaternion.identity) as GameObject;
		obj.transform.parent = transform.parent;
		obj.name = "Tile"+type;
		
		gameObject.SetActive(false);
		//Destroy(this.gameObject);
	}
	
	void OnDrawGizmos()
	{
		if(EditorApplication.isPlaying) return;

		if(info == null || info.type != type)
		{
			gameObject.name = "Tile"+type;
			info = PatternManager.Instance.GetTileInfo(type);
		}
		
		Gizmos.color = info.color;
		
		if(info.shape == PatternTile.Shape.Circle)
		{
			Gizmos.DrawSphere(transform.position, 0.5f);
		}
		else
		{
			Gizmos.DrawCube(transform.position, Vector3.one);
		}
	}
}
