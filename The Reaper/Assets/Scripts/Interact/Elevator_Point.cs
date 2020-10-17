using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator_Point : MonoBehaviour
{
    private Interact main;

    // Start is called before the first frame update
    void Start()
    {
        main = transform.parent.GetComponent<Interact>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            main.player = collision.gameObject;
            main.current = transform;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            main.player = collision.gameObject;
            main.current = transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            main.player = null;
        }
    }
}
