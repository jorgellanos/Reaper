using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    //GENERAL
    public GameObject player;
    public enum Tipo { Door, Locked, Item, Action, Talk };
    public Tipo type;
    private bool action;
    [HideInInspector] public Transform current;
    [HideInInspector] public bool interactable;

    //DOOR 
    [SerializeField] private Transform pointA, pointB;
    
    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            switch (type)
            {
                case Tipo.Door:
                    Door();
                    break;
                case Tipo.Locked:
                    break;
                case Tipo.Item:
                    break;
                case Tipo.Action:
                    break;
                case Tipo.Talk:
                    break;
                default:
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
