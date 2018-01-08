using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
	private float _zoom = 9f;
	public float _maxZoom = 12f;
	public float _minZoom = 3f;
	private float _zoomSpeed = 10f;
	private float _speed = 0.2f;
	private float _moveOffset = 3f;

	private float _minX = 0;
	private float _minZ = -1;
	private float _maxX = 19;
	private float _maxZ =  5;

	private Vector3 _dest;
	void Awake () 
	{
		_dest = Camera.main.transform.position;
	}
	Vector2 startPos;
	void Update () 
	{  
		Vector2 mousePos = Input.mousePosition;
		if(Input.GetMouseButtonDown(0)){
			startPos = mousePos;
			return;
		}
		if(Input.GetMouseButton(0)){
			Vector3 positionDelta = Camera.main.ScreenToViewportPoint(startPos-mousePos);
			_dest = this.transform.position + new Vector3(positionDelta.x * 10f, 0, positionDelta.y * 10f);
			_dest.x = Mathf.Clamp(_dest.x, _minX, _maxX);
			_dest.z = Mathf.Clamp(_dest.z, _minZ, _maxZ);
		}
		int x = (int) mousePos.x;
		int y = (int) mousePos.y;

		if(mousePos.x < _moveOffset)
		{
			_dest.x = Mathf.Clamp(_dest.x-_speed, _minX, _maxX);
		}
		else if(x > Screen.width - _moveOffset) 
		{
			_dest.x = Mathf.Clamp(_dest.x+_speed, _minX, _maxX);
		}
		if(mousePos.y < _moveOffset)
		{
			//mousepos is at the bottom
			//move down
			_dest.z = Mathf.Clamp(_dest.z-_speed, _minZ, _maxZ);
		}
		else if(y > Screen.height - _moveOffset) 
		{
			_dest.z = Mathf.Clamp(_dest.z+_speed, _minZ, _maxZ);
		}
		if(Input.GetAxis("Mouse ScrollWheel") != 0f)
		{
			_zoom -= Input.GetAxis("Mouse ScrollWheel") * _zoomSpeed;
     		_zoom = Mathf.Clamp(_zoom, _minZoom, _maxZoom);
			_dest.y = _zoom;		
		}
		
		Vector3 s = transform.position-_dest;
		if(s.magnitude >= 0.02)
		{
        	transform.position = Vector3.Lerp(transform.position, _dest, 0.7f* _zoom * Time.deltaTime);
		}
	}

}
