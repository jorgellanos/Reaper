using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZ1 : MonoBehaviour
{
    public float health, stamina, speed, damage;
    [SerializeField] private Transform target;
    [SerializeField] private EnemyAttack at;
    private bool isFacingRight, inRange, targetAquired, hit;
    private float distance, dist, dir, oldSpeed;
    private Animator an;
    
    private void Awake()
    {
        oldSpeed = speed;
        at.damage = damage;
        if (!an)
        {
            an = GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckForTarget();
        FollowTarget();
        if (target)
        {
            LookAtPlayer();
        }
    }

    private void CheckForTarget()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right * 5f);
        Debug.DrawRay(transform.position, transform.right * 5f);
        // If it hits something...
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Player")
            {
                targetAquired = true;
                target = hit.transform;
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
        if (vectorToTarget.x > 0)
        {
            isFacingRight = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (vectorToTarget.x < 0)
        {
            isFacingRight = false;
            transform.rotation = Quaternion.Euler(0,180,0);
        }
    }

    private void Attack()
    {
        if (target && hit)
        {
            if (inRange)
            {
                an.Play("Attack");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inRange = true;
            StartCoroutine(AttackSequence(0.5f));
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

    IEnumerator AttackSequence(float delay)
    {
        speed = 0;
        yield return new WaitForSeconds(delay);
        hit = true;
        Attack();
        speed = oldSpeed;
    }
}
