using UnityEngine;
using System.Collections;

public class Spawner : WorldObject 
{
	public GameObject spawnPrefab;
	public float spawnInterval = 5f;
	
	private float spawnTimer;
	private Vector2int pos;
	private bool full;
	
	void Start () 
	{
		pos = new Vector2int(transform.position.x, transform.position.z);
		
		transform.position = pos.ToVector3();
		spawnTimer = spawnInterval;
	}
	
	void FixedUpdate () 
	{
		if(spawnTimer > 0)
		{
			spawnTimer -= Time.deltaTime;
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
