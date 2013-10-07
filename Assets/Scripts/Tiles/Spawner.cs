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
		var x = Mathf.RoundToInt(transform.position.x);
		var y = Mathf.RoundToInt(transform.position.z); 	
		
		pos = new Vector2int(x, y);
		
		transform.position = new Vector3(x, 0, y);
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
			Tile tile = Level.Instance.GetTile(pos.x, pos.y);
			
			if(tile == null) 
			{
				full = false;
				spawnTimer = spawnInterval;
			}
		}
		else
		{
			bool valid = Level.Instance.CreateTile(spawnPrefab, pos);
			
			if(valid) full = true;	
		}
	}
}
