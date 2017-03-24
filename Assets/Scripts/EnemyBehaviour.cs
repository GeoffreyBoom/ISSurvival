using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBehaviour : MonoBehaviour {

    public enum State {Idle, MoveTo, Patrol, Attack, Hide};

    public State currentState = State.Idle;

    public bool isSelected = false;
    bool returnToIdle = false;

    public Vector3 target;
    public Vector3 patrolNextTarget;
    public Vector3 enemyNextTarget;

    //variables for the arrive behaviour
    float nearRadius = 0.5f;
    float arriveRadius = 0.1f;
    float nearMaxSpeed = 10.0f;
    float arriveMaxSpeed = 2.0f;
    float acceleration = 5.0f;

    //variables for flocking
    public Vector3 flockAcceleration = Vector3.zero;
    public Quaternion flockRotation = Quaternion.identity;

    public List<State> futurStates;
    public List<Vector3> futurTargets;

    RTSInterface inter;
	
    // Use this for initialization
	void Start () {
        clearAllFutur();
        inter = FindObjectOfType<RTSInterface>();
    }
	
	// Update is called once per frame
	void Update () {
        checkCurrentState();
	}
    
    void checkCurrentState()
    {
        if (isEnemyDetected())
        {
            if(currentState != State.Attack)
            {
                storeStateAsNext(currentState,target);
                currentState = State.Attack;
                Debug.Log("Enemy Detected");
            }                      
            target = enemyNextTarget;
        }else
        {
            if (currentState == State.Attack)
            {
                currentState = State.Idle;
            }
        }

        isResourceDetected();

        if(currentState == State.Attack)
        {
            attackBehaviour();
        }else if(currentState == State.MoveTo)
        {
            moveToBehaviour();
        }else if(currentState == State.Patrol)
        {
            patrolBehaviour();
        }else if(currentState == State.Hide)
        {
            hideBehaviour();
        }else
        {
            idleBehaviour();
        }  
    }

    void idleBehaviour()
    {
        if(futurStates.Count != 0)
        {
            if (!returnToIdle)
            {
                storeState(State.MoveTo, transform.position);
                returnToIdle = true;
            }
            goToNextState();
        }
        else
        {
            returnToIdle = false;
        }
    }

    void attackBehaviour()
    {
        if (!moving())
        {
            //TO DO implement attacking behaviour
        }       
    }

    void moveToBehaviour()
    {
        if (!moving())
        {
            currentState = State.Idle;
            //goToLastState();
        }
    }

    void patrolBehaviour()
    {
        if(futurStates.Count != 0)
        {
            storeState(currentState, target);
            goToNextState();
        }
        else if (!moving())
        {
            //change target
            Vector3 temp = target;
            assignTarget(patrolNextTarget);
            assignPatrolTarget(temp);
        }
     }

    void hideBehaviour()
    {
        //implement hiding behaviour
    }

    bool isEnemyDetected()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, 10.0f, transform.forward, out hit, 15.0f))
        {
            if (hit.collider.gameObject.name.Equals("Enemy"))
            {
                enemyNextTarget = hit.collider.gameObject.transform.position;
                return true;
            }
        }
        return false;
    }

    void isResourceDetected()
    {
        RaycastHit hit;
        if(Physics.SphereCast(transform.position,10.0f,transform.forward, out hit, 15.0f))
        {
            if (hit.collider.gameObject.name.Equals("Resource") && hit.collider.gameObject.transform.position != target)
            {
                storeState(State.MoveTo, hit.collider.gameObject.transform.position);
            }
        }
    }

    bool moving()
    {
        //the direction in which to move
        Vector3 direction = (target - transform.position).normalized;
        //the distance to the target
        float dist = (target - transform.position).magnitude;
        
        //if closer than arrive radius
        if (dist < arriveRadius)
        {
            //move directly to target then stop moving 
            transform.position = target;
            flockAcceleration = Vector3.zero;
            return false;
        }
        //if not at target
        //turn towards target (align)
        //move towards target (arrive)
            //if not oriented towards target
            if (transform.forward != direction)
            {
                //turn towards target using align
                Quaternion fwr = Quaternion.LookRotation(transform.forward);
                Quaternion dir = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(fwr, dir, 0.20f);
            }
            else
            {
                //move to target
                Vector3 velocity = Vector3.zero;
                //if before the near radius
                if (dist > nearRadius)
                {
                    //accelerate towards target (or move at max speed)
                    velocity += direction * acceleration + flockAcceleration;
                    if (velocity.magnitude > nearMaxSpeed)
                    {
                        velocity = direction * nearMaxSpeed;
                    }
                }//if before arrive radius
                else if (dist > arriveRadius)
                {
                    //accelerate towards target (or move at max speed)
                    velocity += direction * acceleration;
                    if (velocity.magnitude > arriveMaxSpeed)
                    {
                        velocity = direction * arriveMaxSpeed;
                    }
                }
                //move npc
                transform.position += velocity * Time.deltaTime;
            }
        flockAcceleration = Vector3.zero;
        return true;
    }

    public void assignTarget(Vector3 newTarget)
    {
        target = newTarget;
    }
    public void assignPatrolTarget(Vector3 newTarget)
    {
        patrolNextTarget = newTarget;
    }

    void goToNextState()
    {
        if (futurStates.Count != 0)
        {
            currentState = futurStates[0];
            target = futurTargets[0];
            futurStates.RemoveAt(0);
            futurTargets.RemoveAt(0);

        }
    }
    void storeStateAsNext(State nState, Vector3 nTarget)
    {
        bool canAdd = true;
        for (int i = 0; i < futurTargets.Count; i++)
        {
            if (futurTargets[i] == nTarget)
            {
                canAdd = false;
                break;
            }
        }

        if (canAdd)
        {
            futurTargets.Insert(0, nTarget);
            futurStates.Insert(0, nState);
        }
    }
    void storeState(State nState, Vector3 nTarget)
    {
        bool canAdd = true;
        for(int i = 0; i < futurTargets.Count; i++)
        {
            if(futurTargets[i] == nTarget)
            {
                canAdd = false;
                break;
            }
        }

        if (canAdd)
        {
            futurTargets.Add(nTarget);
            futurStates.Add(nState);
        }
    }
    public void clearAllFutur()
    {
        futurTargets = new List<Vector3>();
        futurStates = new List<State>();
    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("Hit");
        if (col.gameObject.name.Equals("Resource"))
        {
            Debug.Log("Hit by player");
            inter.GotResource();
        }
    }
}

