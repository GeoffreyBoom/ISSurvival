using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

	[SerializeField]
	Transform gun = null;
	[SerializeField]
	GameObject bullet = null;

	// Use this for initialization
	void Start () {
		if(gun == null)
		{
			print("gun is null");
		}
		if (bullet == null)
		{
			print("bullet is null");
		}
	}

	float velocity = 5;
	// Update is called once per frame
	void Update () {
		if (gun != null && bullet != null)
		{
			if (Input.GetMouseButton(0))
			{
				Bullet b = Instantiate(bullet).GetComponent<Bullet>();
				b.SetInitialAcceleration(velocity, gun.position + gun.forward.normalized*1, gun.forward);
			}
		}
	}
}
