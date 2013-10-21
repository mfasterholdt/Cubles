using UnityEngine;
using System.Collections;

public class Player : Tile 
{
	private Vector2int input;
	
	public GameObject cameraFollow;
	public Vector3 cameraOffset;

	public override void Initialize ()
	{
		moveSpeed = 20f;
		base.Initialize ();
	}
	
	public override void CalculateForce () 
	{
		if(input != Vector2int.zero)
		{
			AddForce(input);
			input = Vector2int.zero;
		}
	}	
	
	public void Update()
	{
		float x = Input.GetAxis("Horizontal");
		if(x != 0) input.x = (int)Mathf.Sign(x);
		
		float y = Input.GetAxis("Vertical");
		if(y != 0) input.y = (int)Mathf.Sign(y);
		
		if(cameraFollow)
		{
			cameraFollow.transform.position = transform.position + cameraOffset;
		}
	}
}
