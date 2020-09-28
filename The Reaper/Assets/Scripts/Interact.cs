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
    public Transform current;

    //DOOR 
    [SerializeField] private Transform pointA, pointB;
    
    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            switch (tipo)
            {
                case "Door":
                    Door();
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
        if (Input.GetKeyDown("e"))
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
