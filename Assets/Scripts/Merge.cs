using UnityEngine;
using System.Collections;

[System.Serializable]
public class Merge
{
	public Tile tile;
	public Tile target;
	
	public GameObject spawnPrefab;
	
	//Checking Part
	public MergeEntry CheckMerge(Tile tile, Tile target)
	{
		bool match = TilesMatch(tile, target);
		
		if(match)
		{
			MergeEntry entry = new MergeEntry(tile, target,spawnPrefab);
			
			return entry;
		}
		else
		{
			return null;
		}
	}
	
	public bool TilesMatch(Tile t, Tile tar)
	{
		bool valid = t.GetType() == tile.GetType() && tar.GetType() == target.GetType();
		bool validSwap = t.GetType() == target.GetType() && tar.GetType() == tile.GetType();
		
		return valid || validSwap;
	}
}
