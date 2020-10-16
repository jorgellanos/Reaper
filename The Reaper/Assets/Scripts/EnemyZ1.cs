using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZ1 : MonoBehaviour
{
    public float health, stamina, speed, damage, distance;
    [SerializeField] private Transform target;
    [SerializeField] private EnemyAttack at;
    private bool isFacingRight, inRange, targetAquired, hit;
    private float oldSpeed;
    private Animator an;
    private Rigidbody2D rb;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
        if (target)
        {
            CheckRange();
            LookAtPlayer();
            FollowTarget();
            if (inRange)
            {
                Attack();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Scythe")
        {
            Hurt(collision.GetComponent<Scythe>().damage);
        }
    }

    private void CheckRange()
    {
        distance = Vector2.Distance(transform.position, target.position);
        if (distance <= 1.5f)
        {
            inRange = true;
            hit = true;
        }
        else
        {
            inRange = false;
        }
    }

    private void CheckForTarget()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right * 5f);
        Debug.DrawRay(transform.position, transform.right * 5f);
        // If it hits something...
        if (hit.collider != null)
        {
            an.SetBool("InSight", false);
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
        if (!inRange)
        {
            an.SetBool("InSight", true);
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
        if (hit)
        {
            if (!an.GetCurrentAnimatorStateInfo(0).IsTag("Attacking") || !an.GetCurrentAnimatorStateInfo(0).IsTag("Hurted"))
            {
                StartCoroutine(AttackSequence(0.5f));
            }
        }
    }

    public void Hurt(float damage)
    {
        health -= damage;
        an.SetTrigger("Hurt");
        Vector3 dir = transform.right;
        rb.AddForce(new Vector2(-dir.x * 3f, 1.5f), ForceMode2D.Impulse);
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
        an.SetTrigger("Attack");
        speed = oldSpeed;
        hit = false;
    }
}
