using UnityEngine;
using System.Collections;

public class Player : Tile 
{
	public GameObject cameraFollow;
	public Vector3 cameraOffset;
	
	private float playerMoveSpeed = 3.75f;
	private Vector3 moveDir;
	private Vector2int pushDir; 
	
	public override void Initialize ()
	{
		base.Initialize ();
	}
	
	public override void CalculateForce () 
	{
		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");
		pushDir.x = x != 0 ? (int)Mathf.Sign(x) : 0;
		pushDir.y = y != 0 ? (int)Mathf.Sign(y) : 0;
		
		//Push
		Tile tile = Level.Instance.GetTile(pos + pushDir);	
		
		if(tile != null && tile.pushable)
		{
			tile.AddForce(pushDir);	
		}
		
		Vector2int path;
		path.x = Mathf.RoundToInt(transform.position.x - pos.x);
		path.y = Mathf.RoundToInt(transform.position.z - pos.y);
		
		AddForce(path);
	}
	
	public override void MoveTile(Vector2int p)
	{
		pos = p;
	}
	
	public override Vector2int HandleMove()
	{
		pos += force;
		
		return pos;
	}
	
	public override void FixedUpdate()
	{
		moveDir.x = Input.GetAxis("Horizontal");
		moveDir.z = Input.GetAxis("Vertical");
		
		if(Mathf.Abs(moveDir.x) > 0.5f || Mathf.Abs(moveDir.z) > 0.5f)
		{
			Vector3 lookAt = visuals.transform.position + moveDir.normalized;
				
			visuals.transform.LookAt(lookAt, Vector3.up);
		}
		
		Vector2int checkPos = pos;

		if(moveDir.x != 0)
		{
			checkPos.x += (int)Mathf.Sign(moveDir.x);
			Tile tile = Level.Instance.GetTile(checkPos);
			
			if(tile)
			{
				if((moveDir.x > 0 && visuals.transform.position.x > pos.x) ||(moveDir.x < 0 && visuals.transform.position.x < pos.x) )
				{
					moveDir.x = 0;	
				}
			}
		}
		
		if(moveDir.z != 0)
		{
			checkPos = pos;
			checkPos.y += (int)Mathf.Sign(moveDir.z);
			Tile tile = Level.Instance.GetTile(checkPos);
			
			if(tile)
			{
				if((moveDir.z > 0 && visuals.transform.position.z > pos.y) ||(moveDir.z < 0 && visuals.transform.position.z < pos.y) )
				{
					moveDir.z = 0;	
				}
			}
		}	

		
		transform.position += moveDir * Time.deltaTime * playerMoveSpeed;
		
		if(cameraFollow)
		{
			Vector3 camPos = cameraFollow.transform.position;
			camPos += (transform.position + cameraOffset - camPos) * Time.deltaTime * 5f;
			cameraFollow.transform.position = camPos;
		}
	}
	
	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		
		//Gizmos.DrawWireCube(pos.ToVector3(), Vector3.one);
	}
}
