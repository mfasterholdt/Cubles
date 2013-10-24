using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : WorldObject 
{
	public bool organic = true;
	public bool pushable = false;
	public GameObject visuals;
	public int age = 0;
	public bool debug;
	
	[HideInInspector]
	public Vector2int pos;
	
	protected Vector3 targetPos;
	protected Vector3 dir;
	
	protected Vector2int force;
	protected float moveSpeed = 13f;
	protected int ageTimer;
	
	public enum Direction{Right, Left, Up, Down}
	public bool roll;
	
	protected bool sparking;
	protected int countSparks;
	//protected Vector2int sparkForce;
	
	public virtual void Initialize () 
	{
		pos = new Vector2int(transform.position.x, transform.position.z);
		
		force = Vector2int.zero;
		
		targetPos = transform.position;
		
		ageTimer = age;
	}
	
	public virtual void AddForce(int x, int y)
	{				
		//Add force
		force.x += x;
		force.y += y;
	}
	
	public virtual void AddForce(Vector2int f){	AddForce(f.x, f.y);	}
	
	public virtual void HandleForce()
	{
		bool valid = CheckAge();
		
		if(valid) CalculateForce();
	}
	
	public virtual bool CheckAge()
	{
		if(age == 0) return true;
		
		if(ageTimer > 0)
		{
			ageTimer--;
			return false;
		}
		else
		{
			ageTimer = age;
			return true;
		}
		
	}
	
	public virtual void GainSpark(Vector2int dir)
	{
	}
	
	public virtual void LoseSpark()
	{
	}
	
	public virtual void CalculateForce()
	{

	}
	
	public virtual Vector2int GetTileForce()
	{
		return force;
	}
	
	public virtual void SetTileForce(Vector2int f)
	{
		force = f;
	}
	
	public virtual bool AdjustForce()
	{
		//Clamp force
		force.Clamp();

		//Force adjustment mainly for diagonal forces
		Tile tileX = Level.Instance.GetTile(pos.x + force.x, pos.y);
		if(tileX != null && tileX.GetTileForce() == Vector2int.zero && !tileX.pushable) force.x = 0; 
		
		Tile tileY = Level.Instance.GetTile(pos.x, pos.y + force.y);
		if(tileY != null && tileY.GetTileForce() == Vector2int.zero && !tileY.pushable) force.y = 0;
		
		//Target occupied
		Tile tile = Level.Instance.GetTile(pos + force);
		if(tile != null)
		{
			//Target static
			if(tile.GetTileForce() == Vector2int.zero)
			{
				force = Vector2int.zero;
			}
			
			//***Prevent objects from swapping places, might not be possible?
		}
		
		//Does this object have force
		if(force != Vector2int.zero)
			return true;
		else
			return false; 
	}
	
	public virtual void MoveTile(Vector2int p)
	{
		//Visual move target
		Vector3 newPos = p.ToVector3();
		targetPos = newPos;
		
		//Visual Rotation
		if(visuals!=null)
		{
			dir.x = p.x - pos.x;
			dir.z = p.y - pos.y;
			
			visuals.transform.LookAt(visuals.transform.position + dir);		
		}
		
		//Logic move
		pos = p;
	}
	
	public virtual Vector2int HandleMove()
	{
		Vector2int p = pos + force;
		
		//Visual move target
		Vector3 newPos = p.ToVector3();
		targetPos = newPos;
		
		//Visual Rotation
		if(visuals!=null)
		{
			dir.x = p.x - pos.x;
			dir.z = p.y - pos.y;
			
			visuals.transform.LookAt(visuals.transform.position + dir);		
		}
		
		//Logic move
		pos = p;
		
		return p;
	}
	
	public virtual void HandlePrepare()
	{
	}
	
	public virtual void HandleEnd()
	{
		if(!roll)
		{
			force = Vector2int.zero;
		}
	}
	
	public virtual void FixedUpdate()
	{
		if(transform.position != targetPos)
		{
			Vector3 pos = transform.position;
			
			pos += (targetPos - pos) * Time.deltaTime * moveSpeed * Level.Instance.moveFactor;
			
			transform.position = pos;
		}
	}
	
	public static Vector2int GetDirectionVector(Tile.Direction d)
	{
		if(d == Tile.Direction.Up)
			return Vector2int.up;
		else if(d == Tile.Direction.Down)
			return Vector2int.down;
		else if(d == Tile.Direction.Left)
			return Vector2int.left;
		else 
			return Vector2int.right;
	}
	
	void OnDrawGizmos()
	{
		if(sparking)
		{
			Gizmos.color = new Color(1, 1, 1, 0.35f);
			Gizmos.DrawSphere(visuals.transform.position + Vector3.up * 0.6f, 0.65f);
		}
	}
	
	public virtual void Remove()
	{
		Destroy(gameObject);
	}
}
