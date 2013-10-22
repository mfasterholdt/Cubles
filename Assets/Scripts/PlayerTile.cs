using UnityEngine;
using System.Collections;

public class PlayerTile : Tile 
{
	public GameObject cameraFollow;
	public Vector3 cameraOffset = new Vector3(0, 0, -11);
	
	public float playerMoveSpeed = 20f;
	private Vector3 moveDir;
	private Vector2int pushDir; 
		
	public float arriveDist = 0.05f;
	private bool moving;
	public bool linearMoveSpeed;
	
	public override void Initialize ()
	{
		base.Initialize ();
	}
	
	public override void CalculateForce () 
	{
		//Push
		Vector2int pushDir = Vector2int.zero;
		
		if(Input.GetKey(KeyCode.RightArrow)) pushDir.x += 1;
		if(Input.GetKey(KeyCode.LeftArrow)) pushDir.x -= 1;
		if(Input.GetKey(KeyCode.UpArrow)) pushDir.y += 1;
		if(Input.GetKey(KeyCode.DownArrow)) pushDir.y -= 1;
	
		Tile tile = Level.Instance.GetTile(pos + pushDir);			
		if(tile != null && tile.pushable)
		{
			tile.AddForce(pushDir);	
		}
	}
	
	public override void MoveTile(Vector2int p)
	{
		pos = p;
	}
	
	public void AttemptMove(Vector2int dir)
	{
		if(moving) return;
		
		dir.Clamp();
		
		Tile tile = Level.Instance.GetTile(pos + dir);
		
		if(!tile)
		{
			if(dir.x != 0 && dir.y != 0)
			{
				//Diagonal check
				tile = Level.Instance.GetTile(pos.x, pos.y + dir.y);
				
				if(tile != null && (!tile.pushable || !organic)) return;
				
				tile = Level.Instance.GetTile(pos.x + dir.x, pos.y);
				
				if(tile != null && (!tile.pushable || !organic)) return;
			}
			
			AddForce(dir);
			Level.Instance.MoveTile(this);
			force = Vector2int.zero;
			moving = true;
		}
	}
	
	public override void FixedUpdate()
	{
		//Move on intervals
		if(!moving)
		{	
			Vector2int input = Vector2int.zero;
		
			if(Input.GetKey(KeyCode.RightArrow)) input.x += 1;
			if(Input.GetKey(KeyCode.LeftArrow)) input.x -= 1;
			if(Input.GetKey(KeyCode.UpArrow)) input.y += 1;
			if(Input.GetKey(KeyCode.DownArrow)) input.y -= 1;
			
			if(input != Vector2int.zero)
			{
				AttemptMove(input);	
			}		
		}
		
		//Fade visuals
		if(transform.position != targetPos)
		{
			Vector3 diff = targetPos - transform.position;
			float dist = diff.magnitude;
			
			if(dist > arriveDist)
			{
				if(linearMoveSpeed)
				{
					transform.position += diff.normalized * Time.deltaTime * playerMoveSpeed;
				}
				else
				{
					Vector3 pos = transform.position;
					pos += diff * Time.deltaTime * playerMoveSpeed * Level.Instance.moveFactor;
					transform.position = pos;
				}
			}
			else
			{
				moving = false;
			}
		}
	}
	
	public void Update()
	{
		//Move on keydown
		Vector2int input = Vector2int.zero;
		
		if(Input.GetKeyDown(KeyCode.RightArrow)) input.x += 1;
		if(Input.GetKeyDown(KeyCode.LeftArrow)) input.x -= 1;
		if(Input.GetKeyDown(KeyCode.UpArrow)) input.y += 1;
		if(Input.GetKeyDown(KeyCode.DownArrow)) input.y -= 1;
		
		if(input != Vector2int.zero)
		{
			AttemptMove(input);	
		}
		
		CameraFollow();
	}
	
	void CameraFollow()
	{
		if(cameraFollow)
		{
			Vector3 camPos = cameraFollow.transform.position;
			camPos += (transform.position + cameraOffset - camPos) * Time.deltaTime * 5f;
			cameraFollow.transform.position = camPos;
		}	
	}
	
	void OnDrawGizmos()
	{
		if(debug)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireCube(pos.ToVector3(), Vector3.one);
		}
	}
}
