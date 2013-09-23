using UnityEngine; 
using System.Collections; 

[System.Serializable]
public class Vector2int {

	public int x;
	public int y;
	
	public Vector2int (int x, int y) 
	{
		this.x = x;
		this.y = y;
	}
	
	public Vector2int (float x, float y) 
	{
		
	}
	
	int sqrMagnitude
    	{
        get { return x * x + y * y; }
    }
	
	public override string ToString ()
	{
		return string.Format ("{0},{1}", x, y);
	}
}


