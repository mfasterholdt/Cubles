using UnityEngine;
using System.Collections;

public class Spawner : WorldObject 
{
	public GameObject spawnPrefab;
	public int spawnInterval = 15;
	
	private int spawnTimer;
	private Vector2int pos;
	private bool full;
	
	void Start () 
	{
		pos = new Vector2int(transform.position.x, transform.position.z);
		
		transform.position = pos.ToVector3();
		spawnTimer = spawnInterval;
		
		Level.Instance.OnStep += OnStep;
	}
	
	void OnStep()
	{
		if(spawnTimer > 0)
		{
			spawnTimer--;
		}
		else if(full)
		{
			Tile tile = Level.Instance.GetTile(pos);
			
			if(tile == null) 
			{
				full = false;
				spawnTimer = spawnInterval;
			}
		}
		else
		{
			bool valid = Level.Instance.CreateTile(pos, spawnPrefab);
			
			if(valid) full = true;	
		}
	}
}
