using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scythe : MonoBehaviour {

    //Variables
    public float damage, force;
    public bool touch;
    public static int dir;

    // Referencias
    private Rigidbody2D rg;

	// Use this for initialization
	void Start () {
        dir = -1;
        touch = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (touch)
        {
            Hit();
        }
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Object")
        {
            rg = collision.GetComponent<Rigidbody2D>();
            touch = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Object")
        {
            rg = null;
        }
    }

    public void Hit()
    {
        if (rg != null)
        {
            rg.AddForce(rg.transform.up * (force * dir));
        }
        touch = false;
    }

}
