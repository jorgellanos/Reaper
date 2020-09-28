using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZ1 : MonoBehaviour
{
    public float health, stamina, speed, damage;
    public Transform target;
    public bool isFlipped, inRange;
    public float distance, dist, dir;
    public Animator an;
    
    private void Awake()
    {
        if (!an)
        {
            an = GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        dir = transform.position.x - target.position.x;
        dist = Vector2.Distance(transform.position, target.position);
        if (dist <= distance)
        {
            an.SetBool("InSight", true);
        }
        else
        {
            an.SetBool("InSight", false);
        }

        if (isFlipped)
        {
            LookAtPlayer();
        }
    }

    public void LookAtPlayer()
    {
        Vector3 vectorToTarget = target.transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed * 10);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inRange = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inRange = false;
        }
    }

    public void Attack()
    {
        if (inRange)
        {
            target.GetComponent<Player>().Health -= 3;
        }
    }
}
