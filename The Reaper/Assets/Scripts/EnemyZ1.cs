using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZ1 : MonoBehaviour
{
    public float health, stamina, speed, damage;
    public Transform target;
    public bool isFlipped, inRange, targetAquired;
    public float distance, dist, dir;
    public Animator an;
    
    private void Awake()
    {
        if (!an)
        {
            //an = GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckForTarget();
        FollowTarget();
    }

    private void CheckForTarget()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * 5f);
        Debug.DrawRay(transform.position, Vector2.right * 5f);
        // If it hits something...
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Player")
            {
                targetAquired = true;
                target = hit.transform;
                LookAtPlayer();
            }
            else
            {
                if (targetAquired)
                {
                    StartCoroutine(Find(5));
                }
            }
        }
    }

    public void FollowTarget()
    {
        if (target)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
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

    IEnumerator Find(int sec)
    {
        yield return new WaitForSeconds(sec);
        targetAquired = false;
        target = null;
    }
}
