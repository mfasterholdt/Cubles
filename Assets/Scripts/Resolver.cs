using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Resolver
{
	private Dictionary<Vector2int, List<Tile>> resolver;
	
	public Resolver () 
	{
		resolver = new Dictionary<Vector2int, List<Tile>>();
	}
	
	public void AddCheck(Tile tile)
	{
		if(tile.GetTileForce() != Vector2int.zero)
		{
			Add(tile);	
		}
	}
	
	private void Add(Tile tile)
	{
		Vector2int key = tile.pos + tile.GetTileForce();
		
		if(resolver.ContainsKey(key))
		{
			resolver[key].Add(tile);	
		}
		else
		{
			resolver.Add(key, new List<Tile>(){tile});	
		}
	}
	
	public void Clear()
	{
		resolver.Clear();	
	}
	
	public void DebugALl()
	{
		foreach(KeyValuePair<Vector2int, List<Tile>> pair in resolver)
		{
			string output = pair.Key+": ";
			
			pair.Value.ForEach(x => output += x + ", ");
			
			Debug.Log(output);
		}
	}
}
