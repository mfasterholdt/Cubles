using UnityEngine;
using System.Collections;

public class WorldObject : MonoBehaviour 
{
	void OnDrawGizmosSelected()
	{
		if(transform.parent == null || !transform.parent.GetComponent<WorldObject>())
		{
			if(!Input.GetMouseButton(0))
			{
				Vector3 pos = transform.position;
				pos.x = Mathf.Round(pos.x);
				pos.y = Mathf.Round(pos.y);
				pos.z = 0;
				transform.position = pos;
			}
		}
	}
}
