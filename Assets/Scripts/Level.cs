using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Level : SingletonComponent<Level> 
{
	[HideInInspector]
	public float moveInterval = 0.2f;
	
	[HideInInspector]
	public float moveFactor = 1;
	
	private float moveTimer;
	
	public Tile[,] world;
	
	//[HideInInspector]
	public List<Tile> tiles;
	
	private int worldSize = 100;
	
	public Vector2int[] setAdjacent;
	public static Vector2int[] AdjacentTile;
	public Tile borderTile;
	
	[HideInInspector]
	public bool stepManually;	
	
	private GameObject pauseText;
	public GameObject pauseTextPrefab;
	
	public List<Merge> allMerges;
	private List<MergeEntry> mergesToDo = new List<MergeEntry>();
	
	void Start () 
	{
		if(pauseTextPrefab)
		{
			pauseText = Instantiate(pauseTextPrefab) as GameObject;
			pauseText.transform.parent = transform;
			pauseText.name = "GUIPauseText";
		}

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
			if(stepManually)
			{
				stepManually = false;
			}
			else if(moveInterval < 0.7f)
			{
				moveInterval *= 2f;
				moveFactor /= 2;
			}
		}
		
		//Faster
		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			if(stepManually)
			{
				stepManually = false;	
			}
			else if(moveInterval > 0.08f)
			{
				moveInterval /= 2;
				moveFactor *= 2;
			}

		}
			
		//Time
		if(!stepManually)
		{
			if(Time.time > moveTimer)
			{
				HandleStep();
				moveTimer = Time.time + moveInterval;
			}
		}
		
		//Handle GUI
		if(pauseText != null)
		{
			if(pauseText.activeSelf != stepManually)
			{
				pauseText.SetActive(stepManually);
			}
		}
	}
	
	void HandleStep()
	{	
		Tile tile;
		
		//Merges
		mergesToDo.RemoveAll(x => x.PerformMerge());
		
		//Add forces
		for(int i=0, count = tiles.Count; i < count; i++)
		{
			tile = tiles[i];
			
			tile.SetTileForce();		
		}
				
		//Perform moves
		List<Tile> list;
		
		list = tiles.FindAll(x => !x.movable && x is TileFollow);
		UpdateList(list);
		
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
	
	public bool AttemptMove(int x, int y, Tile tile)
	{
		Tile targetTile = world[x, y];
		
		if(targetTile != null)
		{
			bool valid  = MergeCheck(tile, targetTile);
			
			if(valid)
			{
				//Move tile
				world[tile.pos.x, tile.pos.y] = null;
				return true;			
			}
			else
			{			
				return false;
			}
		}
		else
		{
			//Move tile
			world[tile.pos.x, tile.pos.y] = null;
			world[x,y] = tile;
			
			return true;	
		}
	}
	
	public bool MergeCheck(Tile tile, Tile target)
	{
		MergeEntry entry = null;
		
		for(int i=0, count = allMerges.Count; i < count; i++)
		{
			Merge merge = allMerges[i];
			
			entry = merge.CheckMerge(tile, target);
			
			if(entry != null) break;
		}
		
		if(entry != null)
		{
			mergesToDo.Add(entry);
			return true;
		}
		else
		{
			return false;
		}
	}
	
	public void CreateTile(GameObject prefab, Vector2int p)
	{
		Vector3 pos = new Vector3(p.x, 0, p.y);
		
		GameObject obj = Instantiate(prefab, pos, Quaternion.identity) as GameObject;
		Tile newTile = obj.GetComponent<Tile>();
		
		obj.transform.parent = transform;
		obj.name = newTile.GetType().ToString();
		
		tiles.Add(newTile);
		world[p.x, p.y] = newTile;
		
		newTile.Initialize();
	}
	
	public void RemoveTile(Tile tile)
	{
		world[tile.pos.x, tile.pos.y] = null;
		
		tiles.Remove(tile);
		
		tile.Remove();
	}
}
