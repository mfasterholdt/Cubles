using UnityEngine;
using System.Collections;

public class Hat : MonoBehaviour 
{
	public GameObject visuals;
	
	public Material[] materials;
	
	private float minRotation = -15f;
	private float maxRotation = 15f;
	private float minScale = 0.9f;
	private float maxScale = 1.2f;
	
	public void Setup(Tile tile)
	{
		if(tile.pushable)
		{
			//Random Rotation
			Vector3 rot = transform.rotation.eulerAngles;
			rot.x += Random.Range(minRotation, maxRotation);
			rot.y += Random.Range(minRotation, maxRotation);
			rot.z += Random.Range(minRotation, maxRotation);
			transform.rotation = Quaternion.Euler(rot);
		}
		
		//Random Scale
		visuals.transform.localScale *= Random.Range(minScale, maxScale);
		
		//Random Texture
		visuals.renderer.material = materials[Random.Range(0, materials.Length)];
	}
}
