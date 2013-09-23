using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class PatternManager : SingletonComponent<PatternManager> 
{
	public List<TileInfo> tileInfo;
	public Dictionary<PatternTile.Type, GameObject>tilePrefabs;
	
	public TileInfo GetTileInfo(PatternTile.Type type)
	{
		TileInfo info = tileInfo.Find(x=> x.type == type);
		
		return info;
	}
	
	public GameObject GetTilePrefab(PatternTile.Type type)
	{
		//Initialize
		if(tilePrefabs == null)
		{
			tilePrefabs = new Dictionary<PatternTile.Type, GameObject>();
			
			for(int i=0, count = tileInfo.Count; i<count; i++)
			{
				TileInfo info = tileInfo[i];
				
				tilePrefabs.Add(info.type, info.prefab);	
			}
		}
		
		return tilePrefabs[type];
	}
}
