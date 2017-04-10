using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon; 


public class QueenBehaviour : Photon.MonoBehaviour
{

    LineRenderer line;
    float radius = 1.25f;
    float increment = 0.2f;
    bool drawPoints = false;
    float speed = 4.0f;
    [SerializeField]
    public int numOfEggs = 1;
    bool movingState = false;

    public static bool isOn = false;

    /*
     *
     *if the queen is spawning eggs, the line circonference should be ON and eggs have to be within the circle if they want to bloom. 
     * If the line renderer is ON, then the Queen cannot move. 
     * Otherwise if off.  
     * 
     */

    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
    }


    // Update is called once per frame
    void Update()
    {
        if (drawPoints == false)
        {
            getMouseInputs();
        }
        else
        {
            if (isOn == false)
            {
                if (increment <= radius)
                {
                    CreatePoints(increment);
                    increment += increment * Time.deltaTime * speed;
                }
                else
                {
                    isOn = true;
                    drawPoints = false;
                }

            }
            else
            {
                if (increment >= 0.2f)
                {
                    CreatePoints(increment);
                    increment -= increment * Time.deltaTime * speed;
                }
                else
                {
                    isOn = false;
                    drawPoints = false;
                }
            }

        }


        if(movingState == true)
        {
            Debug.Log("FUCK YOU");
            GetComponent<EnemyBehaviour>().moving();
        }

        //If line renderer was drawn for the Queen's egg spawning circonference:
        if (isOn)
        {
            //if the user presses space then it instantiates 4 eggs within its radius: 
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //spawn 4-5 eggs within the radius: 
                for (int i = 0; i < numOfEggs; i++)
                {
                    Vector3 position = transform.position +  new Vector3(Random.Range(-radius , radius), 0, Random.Range(-radius, radius));
                    GameObject egg = PhotonNetwork.Instantiate("Egg", position, this.transform.rotation, 0);
                    egg.GetComponentInChildren<TextMesh>().text = "8";
                    egg.transform.GetChild(0).transform.LookAt(Camera.main.transform);
                }
            }

        }
    }

    void getMouseInputs()
    {
        if (this.transform.position == GetComponent<EnemyBehaviour>().getTarget())
        {
            movingState = false;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            moveQueen();
        }

        if (Input.GetMouseButtonDown(1) && movingState == false)
        {

            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.rigidbody  != null)
                {
                    if (hit.rigidbody.gameObject.tag == "Queen")
                    {
                        drawPoints = true;
                    }
                }

            }
            
        }
    }

    void moveQueen()
    {
        if (isOn == false)
        {
            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("I AM HERE");
               GetComponent<EnemyBehaviour>().assignTarget(new Vector3(hit.transform.position.x, transform.position.y , hit.transform.position.z));
               movingState = true;
            }
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);

        }
        else
        {
            // Network player, receive data
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();

        }
    }


    void CreatePoints(float increment)
    {
        line.positionCount = (100 + 1);
        line.useWorldSpace = false;
        float x;
        float z;

        float angle = 0f;

        for (int i = 0; i < (100 + 1); i++)
        {

            x = Mathf.Sin(Mathf.Deg2Rad * angle) * increment;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * increment;

            line.SetPosition(i, new Vector3(x, 0, z));

            angle += (360f / 100);
        }
    }

}

