using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    //GENERAL
    public GameObject player;
    public string tipo;
    public bool action;
    public bool interactable;

    //DOOR 
    public Transform pointA, pointB, current;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("John");

        if (tipo == "Door")
        {
            current = pointA;
        }
    }

    // Update is called once per frame
    void Update()
    {
        interactable = player.GetComponent<Player>().interact;
        if (interactable)
        {
            switch (tipo)
            {
                case "Door":
                    if (action)
                    {
                        Door();
                    }
                    break;

                case "Item":
                    if (action)
                    {
                        PickUp();
                    }
                    break;

                case "Action":
                    if (action)
                    {
                        Activate();
                    }
                    break;

                case "Talk":
                    if (action)
                    {
                        Talk();
                    }
                    break;
            }
        }
    }

    public void Door()
    {
        if (current == pointA)
        {
            player.transform.position = pointB.position;
            current = pointB;
            action = false;
        }
        else
        {
            player.transform.position = pointA.position;
            current = pointA;
            action = false;
        }
    }

    public void PickUp()
    {

    }

    public void Activate()
    {

    }

    public void Talk()
    {

    }
}
