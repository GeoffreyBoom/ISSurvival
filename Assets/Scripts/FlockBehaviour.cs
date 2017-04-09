using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlockBehaviour {

    //taken from lab 3
    float repulsionQueryRadius = 5.0f;
    float cohesionQueryRadius = 10.0f;
    public float cohesionFactor = 1.5f;
    public float repulsionFactor = 2.0f;
    //float alignmentFactor = 1.0f;
    public float seekSpeed = 0.5f;

    // Use this for initialization
    public List<EnemyBehaviour> enemies;
    public FlockBehaviour()
    {
        enemies = new List<EnemyBehaviour>();
    }
    void Awake()
    {
        enemies = new List<EnemyBehaviour>();
    }
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(enemies.Count > 1)
        {
            moveFlock();
        }
	}
    
    void moveFlock()
    {
        
        Vector3 avgPos = new Vector3(0.0f, 0.0f, 0.0f);
        foreach (EnemyBehaviour en in enemies)
        {
            avgPos += en.transform.position;   
        }
        avgPos /= enemies.Count;
        foreach (EnemyBehaviour en in enemies)
        {
            Vector3 accel = Vector3.zero;
            //cohesion
            if((avgPos - en.transform.position).magnitude >= cohesionQueryRadius)
            {
                accel += (avgPos - en.transform.position) * cohesionFactor;
            }

            //repulsion
            if ((avgPos - en.transform.position).magnitude < repulsionQueryRadius)
            {
                accel += -(avgPos - en.transform.position) * repulsionFactor;
            }
            //apply
            en.flockAcceleration = accel;
        }

    }    
}
