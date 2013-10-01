using UnityEngine;
using System.Collections;

public class MergeEntry
{
	public Tile tile;
	public Tile target;
	
	public GameObject spawnPrefab;
	private int wait = 0;
	
	public MergeEntry(Tile tile, Tile target, GameObject spawnPrefab)
	{
		this.tile = tile;
		this.target = target;
		this.spawnPrefab = spawnPrefab;
	}
	
	public bool PerformMerge()
	{
		if(wait <= 0)
		{
			Level.Instance.RemoveTile(tile);
			Level.Instance.RemoveTile(target);
			
			if(spawnPrefab) Level.Instance.CreateTile(spawnPrefab, target.pos);
			
			return true;
		}
		else
		{
			wait--;
			return false;
		}
	}
}
