﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_TPS : MonoBehaviour {

	[SerializeField]
	GameObject player;

	float rotationSpeed;
	float zoomSpeed;
	float minAngleY;
	float maxAngleY;

	void Start () {

		rotationSpeed = 1.0f;
		zoomSpeed = 1.0f;

		transform.SetParent(player.transform);
		transform.position = player.transform.position - (2.0f * player.transform.forward) + (2.0f * Vector3.up);
		transform.forward = player.transform.forward + Vector3.down * 0.5f;

		minAngleY = transform.rotation.eulerAngles.x - 20.0f;
		maxAngleY = transform.rotation.eulerAngles.x + 20.0f;
	}
	
	void Update () {

		//Zoom
		transform.position += transform.forward * Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

		//X-rotation
		transform.RotateAround(player.transform.position, Vector3.up, Input.GetAxis("Mouse X") * rotationSpeed);

		//Y-rotation
		if ((transform.rotation.eulerAngles.x >= minAngleY && transform.rotation.eulerAngles.x <= maxAngleY)
			|| (transform.rotation.eulerAngles.x > maxAngleY && Input.GetAxis("Mouse Y") < 0.0f)
			|| (transform.rotation.eulerAngles.x < minAngleY && Input.GetAxis("Mouse Y") > 0.0f))
		{
			transform.RotateAround(player.transform.position, transform.right, Input.GetAxis("Mouse Y") * rotationSpeed);
		}

		//Lock Z-rotation
		Vector3 rotationEuler = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0.0f);
		transform.eulerAngles = rotationEuler;
	}
}
