using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBehaviour : MonoBehaviour {

    public enum State {Idle, MoveTo, Patrol, Attack, Hide, Collect};

    public State currentState = State.Idle;

    public bool isSelected = false;

    public Vector3 target;
    public Vector3 nextTarget;
    public Vector3 lastTarget;

    public List<Vector3> patrolPoints;

    RTSInterface inter;

    //variables for the arrive behaviour
    float nearRadius = 0.5f;
    float arriveRadius = 0.1f;
    float nearMaxSpeed = 10.0f;
    float arriveMaxSpeed = 2.0f;
    float acceleration = 5.0f;

    

    public Stack<State> previousStates;
    public Stack<Vector3> previousTargets;
    //public static bool allowMultipleSelections = false; 
	// Use this for initialization
	void Start () {
        clearAllPrevious();
        inter = FindObjectOfType<RTSInterface>();
    }
	
	// Update is called once per frame
	void Update () {
        checkCurrentState();
	}
    
    void checkCurrentState()
    {
        if(currentState == State.Attack)
        {
            attackBehaviour();
        }else if(currentState == State.Collect)
        {
            collectBehaviour();
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
        if (isEnemyDetected())
        {
            storeState();
            currentState = State.Attack;
            Debug.Log("Enemy Detected");
        }
        else if (isResourceDetected())
        {
            storeState();
            currentState = State.Collect;
            Debug.Log("Resource Detected");
        }
    }

    void attackBehaviour()
    {
        if (!moving())
        {
            //TO DO implement attacking behaviour
            goToLastState();
        }
        
        if (!isEnemyDetected())
        {
            //goToLastState();
            //currentState = State.Idle;
            //return to previous state
        }
    }

    void collectBehaviour()
    {
        if (!moving())
        {
            //gather ressource
            //return to previous state
            goToLastState();
        }
        if (isEnemyDetected())
        {
            storeState();
            currentState = State.Attack;
        }
        else if (isResourceDetected())
        {
            storeState();
            currentState = State.Collect;
        }
    }

    void moveToBehaviour()
    {
        if (!moving())
        {
            //currentState = State.Idle;
            goToLastState();
        }
    }

    void patrolBehaviour()
    {
        if (!moving())
        {
            //change target
            /*patrolPoints.Add(target);
            target = patrolPoints[0];
            patrolPoints.RemoveAt(0);
            */
            assignTarget(lastTarget);

        }
        if (isEnemyDetected())
        {
            storeState();
            currentState = State.Attack;
        }
        else if (isResourceDetected())
        {
            storeState();
            currentState = State.Collect;
        }
    }

    void hideBehaviour()
    {
        //implement hiding behaviour
        if (isEnemyDetected())
        {
            //will not go attack target right away
        }
    }

    bool isEnemyDetected()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, 10.0f, transform.forward, out hit, 15.0f))
        {
            if (hit.collider.gameObject.name.Equals("Enemy") && hit.collider.gameObject.transform.position != target)
            {
                nextTarget = hit.collider.gameObject.transform.position;
                return true;
            }
        }
        return false;
    }

    bool isResourceDetected()
    {
        RaycastHit hit;
        if(Physics.SphereCast(transform.position,10.0f,transform.forward, out hit, 15.0f))
        {
            if (hit.collider.gameObject.name.Equals("Resource") && hit.collider.gameObject.transform.position != target)
            {
                nextTarget = hit.collider.gameObject.transform.position;
                return true;
            }
        }
        return false;
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
                    velocity += direction * acceleration;
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
        return true;
    }

    public void assignTarget(Vector3 newTarget)
    {
        lastTarget = target;
        target = newTarget;
    }

    void goToLastState()
    {
        if (previousStates.Count != 0)
        {
            currentState = previousStates.Pop();
            target = previousTargets.Pop();
        }else
        {
            currentState = State.Idle;
        }
    }
    void storeState()
    {
        if(currentState != State.Idle)
        {
            previousStates.Push(currentState);
            previousTargets.Push(target);
        }
        else
        {
            previousStates.Push(State.MoveTo);
            previousTargets.Push(transform.position);
        }
        
        if (nextTarget != new Vector3(-999.9f, -999.9f, -999.9f))
        {
            target = nextTarget;
        }
        nextTarget = new Vector3(-999.9f, -999.9f, -999.9f);
    }
    public void clearAllPrevious()
    {
        previousTargets = new Stack<Vector3>();
        previousStates = new Stack<State>();
        patrolPoints = new List<Vector3>();
    }
    void OnMouseDown()
    {
        Debug.Log("Selected");
        inter.wasClickedOn(this);
    }
}

