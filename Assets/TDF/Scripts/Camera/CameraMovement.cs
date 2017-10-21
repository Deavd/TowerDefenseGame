using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
	float zoom = 9f;
	public float maxZoom = 12f;
	public float minZoom = 3f;
	float zoomSpeed = 10f;
	float speed = 0.2f;
	float moveOffset = 1f;

	float minX = 0;
	float minZ = -1;
	float maxX = 19;
	float maxZ =  5;

	float distance;
	Vector3 dest;
	void Start () 
	{
		dest = Camera.main.transform.position;
	}
	void Update () 
	{
		Vector2 mousePos = Input.mousePosition;
		int x = (int) mousePos.x;
		int y = (int) mousePos.y;

		if(mousePos.x < moveOffset)
		{
			dest.x = Mathf.Clamp(dest.x-speed, minX, maxX);
		}
		else if(x > Screen.width - moveOffset) 
		{
			dest.x = Mathf.Clamp(dest.x+speed, minX, maxX);
		}
		if(mousePos.y < moveOffset)
		{
			//mousepos is at the bottom
			//move down
			dest.z = Mathf.Clamp(dest.z-speed, minZ, maxZ);
		}
		else if(y > Screen.height - moveOffset) 
		{
			dest.z = Mathf.Clamp(dest.z+speed, minZ, maxZ);
		}
		if(Input.GetAxis("Mouse ScrollWheel") != 0f)
		{
			zoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
     		zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
			dest.y = zoom;		
		}
		Vector3 s = transform.position-dest;
		if(s.magnitude >= 0.02)
		{
        	transform.position = Vector3.Lerp(transform.position, dest, 4f * Time.deltaTime);
		}
	}

}
