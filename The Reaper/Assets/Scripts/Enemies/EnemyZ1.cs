using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZ1 : MonoBehaviour
{
    public float health, stamina, speed, damage, attackRange, SightRange;
    public Transform target, pointA, pointB;
    [SerializeField] private EnemyAttack at;
    [HideInInspector] public bool isFacingRight, runTime;
    [HideInInspector] public float oldSpeed;
    private Animator an;
    private Rigidbody2D rb;
    private Transform m_GroundCheck;
    private bool m_Grounded;
    [SerializeField] private LayerMask m_WhatIsGround;

    private void Awake()
    {
        pointA.parent = transform.parent;
        pointB.parent = transform.parent;
        m_GroundCheck = transform.Find("Feet");
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
            LookAtPlayer();
        }
    }

    private void FixedUpdate()
    {
        GroundCheck();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Scythe")
        {
            Hurt(collision.GetComponent<Scythe>().damage);
            if (health <= 0)
            {
                an.SetTrigger("Death");
                transform.Find("Effects").gameObject.SetActive(true);
            }
        }
    }

    private void GroundCheck()
    {
        m_Grounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, 0.05f, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
            }
        }

        if (!m_Grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, -250f * Time.deltaTime);
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }

    private void CheckForTarget()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.Find("EYES").position, transform.right * (SightRange + 2));
        Debug.DrawRay(transform.Find("EYES").position, transform.right * (SightRange + 2));
        // If it hits something...
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Player")
            {
                target = hit.transform;
                an.SetBool("InSight", true);
                if (Vector2.Distance(transform.position, target.position) >= SightRange)
                {
                    target = null;
                    an.SetBool("InSight", false);
                }
            }
        }
    }

    public void SetAtkSpeed(float speed)
    {
        an.SetFloat("ATKspeed", speed);
    }

    private void LookAtPlayer()
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

    public void Hurt(float damage)
    {
        health -= damage;
        an.SetTrigger("Hurt");
        Vector3 dir = transform.right;
        //rb.AddForce(new Vector2(-dir.x * 3f, 1.2f), ForceMode2D.Impulse);
    }

    public void Dead()
    {
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
