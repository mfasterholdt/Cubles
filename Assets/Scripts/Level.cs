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
	
	private bool stepManually = false;
	
	void Start () 
	{
		moveTimer = 0.25f;

		RegisterWorld();
		
		AdjacentTile = setAdjacent;
	}
	
	void RegisterWorld()
	{
		world = new Tile[worldSize, worldSize];
		
		//Tile[] preplaced = GetComponentsInChildren<Tile>();
		Tile[] preplaced = FindObjectsOfType(typeof(Tile)) as Tile[];
		
		for(int i=0, count = preplaced.Length; i<count; i++)
		{
			Tile tile = preplaced[i];
			
			tile.Initialize();
			
			world[tile.pos.x, tile.pos.y] = tile;
			
			tiles.Add(tile);
		}
	}
	
	void Update () 
	{
		//Handle Time
		
		//Pause Time
		if(Input.GetKeyDown(KeyCode.Space))
		{
			HandleStep();
			stepManually = true;
		}
		
		//Slower
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			if(moveInterval < 0.7f)
			{
				moveInterval *= 2f;
				moveFactor /= 2;
			}
			
			stepManually = false;
		}
		
		//Faster
		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			if(moveInterval > 0.08f)
			{
				moveInterval /= 2;
				moveFactor *= 2;
			}
			
			stepManually = false;
		}
			
		//Time
		if(!stepManually){
			if(Time.time > moveTimer)
			{
				HandleStep();
				moveTimer = Time.time + moveInterval;
			}
		}
	}
	
	 
	
	void HandleStep()
	{		
		Tile tile;
		
		//Add forces
		for(int i=0, count = tiles.Count; i < count; i++)
		{
			tile = tiles[i];
			
			tile.SetTileForce();		
		}
		
		//Perform moves
		List<Tile> list;
		
		list = tiles.FindAll(x => !x.movable);
		UpdateList(list);
		
		list = tiles.FindAll(x => x.movable);
		UpdateList(list);		
	}
	
	void UpdateList(List<Tile> list)
	{
		for(int i=0, count = list.Count; i < count; i++)
		{
			Tile tile = list[i];
			
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
