using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RTSInterface : MonoBehaviour {

    List<EnemyBehaviour> enemies;
    List<Vector3> patrolPoints;
    Camera_RTS cam;
    bool[] stateUsing;
    bool enemySelected;

    EnemyBehaviour currentlyControlledEnemy;
    Vector3 target;
	// Use this for initialization
	void Start () {
        enemies = new List<EnemyBehaviour>();
        enemies.AddRange(FindObjectsOfType<EnemyBehaviour>());
        stateUsing = new bool[6];
        resetStateUsing();

    }
	
	// Update is called once per frame
	void Update () {
        getKeyboardInputs();

    }
    void getKeyboardInputs()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            resetStateUsing();
            stateUsing[0] = true;
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            resetStateUsing();
            stateUsing[1] = true;
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            resetStateUsing();
            stateUsing[2] = true;
            giveCommand();
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            resetStateUsing();
            stateUsing[3] = true;
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            resetStateUsing();
            stateUsing[4] = true;
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            resetStateUsing();
            stateUsing[5] = true;
        }
        if (Input.GetMouseButtonDown(1))
        {
            currentlyControlledEnemy = null;
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit))
            {
                if(hit.rigidbody!= null)
                {
                    Debug.Log("has target");

                    target = hit.point + 2*Vector3.up;
                    giveCommand();
                }
            }
        }
    }
    public void wasClickedOn(EnemyBehaviour e)
    {
        Debug.Log("Has e");
        currentlyControlledEnemy = e;
    }
    void resetStateUsing()
    {
        for (int i = 0; i < stateUsing.Length; i++)
        {
            stateUsing[i] = false;
        }
    }
    void giveCommand()
    {
        if(currentlyControlledEnemy != null)
        {
            if (stateUsing[0])
            {
                Debug.Log("Attack");
                currentlyControlledEnemy.clearAllPrevious();
                currentlyControlledEnemy.currentState = EnemyBehaviour.State.Attack;
                
            }
            else if (stateUsing[1])
            {
                Debug.Log("Hide");
                currentlyControlledEnemy.clearAllPrevious();
                currentlyControlledEnemy.currentState = EnemyBehaviour.State.Hide;
                
            }
            else if (stateUsing[2])
            {
                Debug.Log("Idle");
                currentlyControlledEnemy.clearAllPrevious();
                currentlyControlledEnemy.currentState = EnemyBehaviour.State.Idle;
                
            }
            else if(stateUsing[3])
            {
                Debug.Log("Collect");
                currentlyControlledEnemy.clearAllPrevious();
                currentlyControlledEnemy.currentState = EnemyBehaviour.State.Collect;
            }else if (stateUsing[4])
            {
                Debug.Log("Move To");
                currentlyControlledEnemy.clearAllPrevious();
                currentlyControlledEnemy.currentState = EnemyBehaviour.State.MoveTo;
            }
            else if (stateUsing[5])
            {
                Debug.Log("Patrol");
                currentlyControlledEnemy.clearAllPrevious();
                currentlyControlledEnemy.currentState = EnemyBehaviour.State.Patrol;
            }
            currentlyControlledEnemy.assignTarget(target);
        }
    }
}
