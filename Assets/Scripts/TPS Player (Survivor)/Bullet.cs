using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class Bullet : Photon.MonoBehaviour {

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
        //NOT SURE IF THIS MAKES THE BULLET CHANGE POSITION?
        SetInitialAcceleration(2.0f, this.transform.position, new Vector3(0, 0, 1));

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
		if (other.gameObject.tag == "Queen" || other.gameObject.tag == "Alien")
		{
            photonView.RPC("deleteThis", PhotonTargets.MasterClient, this.gameObject.name);
		}

		//TODO: implement enemy health reduction
	}


    [PunRPC]
    void deleteThis(string dot)
    {
        if (gameObject.name == dot)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
