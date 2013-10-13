using UnityEngine;
using System.Collections;

public class MergeEntry
{
	public Tile tile;
	public Tile target;
	
	public GameObject spawnPrefab;
	
	public MergeEntry(Tile tile, Tile target, GameObject spawnPrefab)
	{
		this.tile = tile;
		this.target = target;
		this.spawnPrefab = spawnPrefab;
	}
	
	public bool PerformMerge()
	{
		Level.Instance.RemoveTile(tile);
		Level.Instance.RemoveTile(target);
		
		if(spawnPrefab) Level.Instance.CreateTile(target.pos, spawnPrefab);
		
		return true;
	}
}
