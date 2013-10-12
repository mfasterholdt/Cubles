using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Level : SingletonComponent<Level> 
{
	public GameObject pauseTextPrefab;
	public GameObject selectionPrefab;
	public Tile borderTile;
	
	[HideInInspector]
	public float moveInterval = 0.2f;
	
	[HideInInspector]
	public Selection selection;
	
	[HideInInspector]
	public float moveFactor = 1;
	
	private float moveTimer;
	
	[HideInInspector]	
	public Tile[,] world;
	
	[HideInInspector]
	public List<Tile> tiles;
	
	[HideInInspector]
	public static int WorldSize = 100;
	
	[HideInInspector]
	public bool stepManually;	
	
	private GameObject pauseText;
	
	public List<Merge> allMerges;
	private List<MergeEntry> mergesToDo = new List<MergeEntry>();
	
	private Tile pickedTile;
	
	private Resolver resolver;
	
	void Start () 
	{
		if(selectionPrefab)
		{
			GameObject selectionObj = Instantiate(selectionPrefab) as GameObject;
			selection = selectionObj.GetComponent<Selection>();
			selection.OnMouseClick += OnSelectionClick;
		}
		
		if(pauseTextPrefab)
		{
			pauseText = Instantiate(pauseTextPrefab) as GameObject;
			pauseText.transform.parent = transform;
			pauseText.name = "GUIPauseText";
		}
		
		RegisterWorld();
		
		resolver = new Resolver();
	}
	
	void OnSelectionClick(Selection sender, Vector2int pos)
	{
		
		Tile tile = GetTile(pos);
		
		if(!tile)
		{
			if(pickedTile)
			{
				
				//Place picked tile
				pickedTile.transform.position = pos.ToVector3();
				
				tiles.Add(pickedTile);
				world[pos.x, pos.y] = pickedTile;
				
				pickedTile.Initialize();
				pickedTile.gameObject.SetActive(true);
				pickedTile = null;
			}
			else
			{
				//Raise rock
				GameObject rockPrefab = PatternManager.Instance.GetTilePrefab(PatternTile.Type.Rock);
				CreateTile(pos, rockPrefab);
			}
		}
		else if(tile is TileRock)
		{
			//Lower Rock
			RemoveTile(tile);
		}
		else if(!pickedTile)
		{
			//Pick up tile
			tile.gameObject.SetActive(false);
			pickedTile = tile;
			
			world[pos.x, pos.y] = null;
			tiles.Remove(tile);
		}
	}
	
	void RegisterWorld()
	{
		world = new Tile[WorldSize, WorldSize];
		
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
	
	void NewHandleStep()
	{	
		
		//Add forces
		tiles.ForEach(x=> x.UpdateTileForce());
		
		
		//***Adjust Forces
		
		//***not all existing tiles are taken into consideration?
		
		//Populate resolver	
		resolver.Clear();
		for(int j=0, count = tiles.Count; j < count; j++)
		{
			Tile tile = tiles[j];
			resolver.AddCheck(tile);
		}
		
		//***Resolve, adjust forces accordingly
		
		//***Move Tiles
		
		resolver.DebugALl();
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
			
			tile.UpdateTileForce();		
		}
		
		//Perform moves
		List<Tile> list;
		
		list = tiles.FindAll(x => x is TileFollow);
		UpdateList(list);
		
		list = tiles.FindAll(x => !x.pushable);
		UpdateList(list);
		
		list = tiles.FindAll(x => x.pushable);
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
		if(x < 0 || x >= WorldSize || y < 0 || y >= WorldSize) return borderTile;
		
		return world[x, y];
	}
	
	public Tile GetTile(Vector2int pos){ return GetTile(pos.x, pos.y); }
	
	public bool AttemptMove(Vector2int pos, Tile tile)
	{
		Tile targetTile = world[pos.x, pos.y];
		
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
			world[pos.x, pos.y] = tile;
			
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
	
	public bool CreateTile(Vector2int pos, GameObject prefab)
	{
		//Occupied
		if(world[pos.x, pos.y] != null) return false;
		
		//Create tile
		GameObject obj = Instantiate(prefab, pos.ToVector3(), Quaternion.identity) as GameObject;
		Tile newTile = obj.GetComponent<Tile>();
		
		obj.transform.parent = transform;
		obj.name = newTile.GetType().ToString();
		
		tiles.Add(newTile);
		world[pos.x, pos.y] = newTile;
		
		newTile.Initialize();
		
		return true;
	}
	
	public void RemoveTile(Tile tile)
	{
		world[tile.pos.x, tile.pos.y] = null;
		
		tiles.Remove(tile);
		
		tile.Remove();
	}
}
