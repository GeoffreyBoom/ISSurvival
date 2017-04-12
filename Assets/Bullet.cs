using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	float StartTimestamp;

	[SerializeField]
	float LifeTime = 10000;
	[SerializeField]
	Rigidbody rigid;

	// Use this for initialization
	void Start () {
		StartTimestamp = Time.time;
		
	}


	// Update is called once per frame
	void Update()
	{
		if (Time.time - StartTimestamp < LifeTime)
		{
			Destroy(this.gameObject);
		}
	}

	public void SetInitialAcceleration(float velocity, Vector3 position, Vector3 direction)
	{
		transform.position = position;
		transform.forward = direction;
		if (rigid == null)
		{
			rigid = gameObject.GetComponent<Rigidbody>();
		}
		if (rigid != null)
		{
			rigid.velocity = transform.forward.normalized * velocity;
		}
		else
		{
			print("bullet does not have RigidBody");
		}
	}
	void OnTriggerEnter(Collider other)
	{

		//TODO: implement the behavior for networking.
		TPSPlayer OtherPlayer = null;
		if ((OtherPlayer = other.gameObject.GetComponent<TPSPlayer>() )!= null)
		{
			OtherPlayer.TakeDamage();
		}

		//TODO: implement enemy health reduction
	}
}
