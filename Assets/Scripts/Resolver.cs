using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Resolver : MonoBehaviour
{
	public List<Merge> allMerges;
	
	private Dictionary<Vector2int, List<Tile>> list;
	private List<Tile> tilesMoved = new List<Tile>();
	private List<MergeEntry> mergesToDo = new List<MergeEntry>();
	
	public Resolver () 
	{
		list = new Dictionary<Vector2int, List<Tile>>();
	}
	
	public void HandlePerformMerges()
	{
		mergesToDo.RemoveAll(x => x.PerformMerge());
	}
	
	public void HandleResolve(List<Tile> tiles)
	{
		RefreshList(tiles);
		
		ResolvePass(); //Recursive conflict resolution
		
		FindMerges();
	}

	private void RefreshList(List<Tile> tiles)
	{
		list.Clear();
		
		//Populate
		for(int j=0, count = tiles.Count; j < count; j++)
		{
			//***Currently resolves all tiles
			Tile t = tiles[j];
			
			AddTile(t);
		}
	}
	
	private void ResolvePass()
	{
		foreach(KeyValuePair<Vector2int, List<Tile>> pair in list)
		{
			List<Tile> tiles = pair.Value;
			
			if(tiles.Count > 1 && !MergePossible(tiles))
			{
				//Conflicting tiles without mergeability, cancel movements
				for(int j = 0, count = tiles.Count; j < count; j++)
				{
					Tile t = tiles[j];
					t.SetTileForce(Vector2int.zero);
					tilesMoved.Add(t);
				}
				
				tiles.RemoveAll(x => x.GetTileForce() == Vector2int.zero);	
			}
		}
		
		if(tilesMoved.Count > 0)
		{
			for(int i = 0, count = tilesMoved.Count; i < count; i++)
			{
				Tile t = tilesMoved[i];
				AddTile(t);
			}
			
			tilesMoved.Clear();
			
			ResolvePass();
		}
		else
		{
			return;
		}
	}	
	
	private void FindMerges()
	{
		foreach(KeyValuePair<Vector2int, List<Tile>> pair in list)
		{	
			List<Tile> tiles = pair.Value;	
			
			if(tiles.Count == 2)
			{
				MergeEntry entry = null;
				Tile tile = tiles[0];
				Tile target = tiles[1];
				
				//Merge found, determine type
				for(int i=0, count = allMerges.Count; i < count; i++)
				{
					Merge merge = allMerges[i];
					
					entry = merge.CheckMerge(tile, target);
					
					if(entry != null) break;
				}			
				
				if(entry == null)
				{
					Debug.LogError(string.Format("Conflict was not resolved at {0}, between {1} and {2}", pair.Key, tile, target));
				}
				else
				{
					mergesToDo.Add(entry);
				}
			}
		}
	}
		
	public bool MergePossible(List<Tile> tiles)
	{
		if(tiles.Count == 2)
		{
			Tile tile = tiles[0];
			Tile target = tiles[1];
			
			for(int i=0, count = allMerges.Count; i < count; i++)
			{
				Merge merge = allMerges[i];
				bool match = merge.TilesMatch(tile, target);
				
				if(match) return true;
			}
		}
		
		return false;
	}
		
	public void AddTile(Tile tile)
	{
		Vector2int key = tile.pos + tile.GetTileForce();
		
		if(list.ContainsKey(key))
		{
			list[key].Add(tile);	
		}
		else
		{
			list.Add(key, new List<Tile>(){tile});	
		}	
	}
	
	public void DebugAll()
	{
		foreach(KeyValuePair<Vector2int, List<Tile>> pair in list)
		{
			string output = pair.Key+": ";
			
			pair.Value.ForEach(x => output += x + ", ");
			
			Debug.Log(output);
		}
	}
}
