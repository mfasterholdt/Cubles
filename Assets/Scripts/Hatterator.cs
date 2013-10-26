using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hatterator : MonoBehaviour 
{
	public bool hatsForEverybody;
	public Hat[] hats;  
	public List<Tile>targets;
	
	void Start()
	{
		if(targets != null && targets.Count > 0)
		{
			foreach(Tile tile in targets)
			{
				GenerateHat(tile);		
			}
		}
		
		if(hatsForEverybody)
		{
			Tile[] preplaced = FindObjectsOfType(typeof(Tile)) as Tile[];
			
			for(int i=0, count = preplaced.Length; i<count; i++)
			{
				Tile tile = preplaced[i];
				if(tile.organic) GenerateHat(tile);
			}
		}
	}
	
	void GenerateHat(Tile tile)
	{
		if(Random.value > 0.95f) return;
		
		int i = Random.Range(0, hats.Length);
		
		Hat newHat = Instantiate(hats[i], tile.transform.position, Quaternion.identity) as Hat;
		
		if(tile.visuals)
		{	
			newHat.transform.parent = tile.visuals.transform;
		}
		else
		{
			newHat.transform.parent = tile.transform;
		}
		newHat.Setup(tile);
	}
}
