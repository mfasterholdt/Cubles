using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Level : SingletonComponent<Level> 
{
	public float moveInterval = 0.2f;
	public float moveFactor = 1;
	
	private float moveTimer;
	
	public Tile[,] world;
	
	[HideInInspector]
	public List<Tile> tiles;
	
	private int worldSize = 100;
	
	public Vector2int[] setAdjacent;
	public static Vector2int[] AdjacentTile;
	public Tile borderTile;
	
	void Start () 
	{
		moveTimer = 0.25f;

		RegisterWorld();
		
		AdjacentTile = setAdjacent;
	}
	
	void RegisterWorld()
	{
		world = new Tile[worldSize, worldSize];
		
		Tile[] preplaced = GetComponentsInChildren<Tile>();
		
		for(int i=0, count = preplaced.Length; i<count; i++)
		{
			Tile tile = preplaced[i];
			
			tile.Initialize();
			
			world[tile.pos.x, tile.pos.y] = tile;
			
			tiles.Add(tile);
		}
	}
	
	bool turnByKeystroke;
	
	void Update () 
	{
		//Handle Time
		
		//Pause Time
		if(Input.GetKeyDown(KeyCode.Space))
		{
			HandleMoves();
			turnByKeystroke = true;
		}
		
		//Slower
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			if(moveInterval < 0.7f)
			{
				moveInterval *= 2f;
				moveFactor /= 2;
			}
			
			turnByKeystroke = false;
		}
		
		//Faster
		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			if(moveInterval > 0.08f)
			{
				moveInterval /= 2;
				moveFactor *= 2;
			}
			
			turnByKeystroke = false;
		}
			
		//Time
		if(!turnByKeystroke){
			if(Time.time > moveTimer)
			{
				HandleMoves();
				moveTimer = Time.time + moveInterval;
			}
		}
	}
	
	void HandleMoves()
	{		
		Tile tile;
		
		//Add forces
		for(int i=0, count = tiles.Count; i < count; i++)
		{
			tile = tiles[i];
			
			tile.SetTileForce();		
		}
		
		//Handles moves
		for(int i=0, count = tiles.Count; i < count; i++)
		{
			tile = tiles[i];
			
			tile.UpdateTile();
		}
		
	}
	
	public Tile GetTile(int x, int y)
	{
		if(x < 0 || x >= worldSize || y < 0 || y >= worldSize) return borderTile;
		
		return world[x, y];
	}
	
	public bool AttemptMove(int x, int y, Tile t)
	{
		Tile targetTile = world[x, y];
		
		if(targetTile != null)
		{
			return false;
		}
		else
		{
			world[t.pos.x, t.pos.y] = null;
			
			world[x,y] = t;
			
			return true;
		}
	}	
}
