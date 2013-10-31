using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
	public GameObject target;
	public float speed = 5f;
	
	private Vector3 offset;
	
	void Start()
	{
		if(target != null)
		{
			offset = target.transform.position - transform.position;
		}
	}
	
	void FixedUpdate () 
	{
		if(target != null)
		{
			Vector3 camPos = transform.position;
			
			camPos += (target.transform.position - camPos - offset) * Time.deltaTime * speed;
			
			transform.position = camPos;
		}
	}
}
