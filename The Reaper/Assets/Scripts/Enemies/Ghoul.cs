using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ghoul : Enemy
{
    [Header("Enemy Stats")]
    [SerializeField] private float health;
    [SerializeField] private float speed;
    [SerializeField] private EnemyType type;
    [SerializeField] private float minAtkDistance;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float nextWaypointDistance;
    [SerializeField] private float jumpCheckOffset;
    [SerializeField] private float jumpNodeHeightRequirement;
    [SerializeField] private float jumpModifier;
    [SerializeField] private float knockbackModifier;
    [SerializeField] private float visionRange;
    [SerializeField] private float stopChaseDistance;
    [SerializeField] private float timePerAttack;

    [Header("References")]
    [SerializeField] private Transform myEyes;
    [SerializeField] private Transform currentTarget;
    [SerializeField] private Animator anim;
    [SerializeField] private EnemyAttack attackController;
    [SerializeField] private GameObject effectsController;

    private int currentWaypoint;
    private bool reachedEndOfPath;
    private bool damaged;
    private Path path;
    private Seeker seeker;
    private Rigidbody2D rb;

    private float damageRecieved;
    private Transform knockbackOrigin;

    // Start is called before the first frame update
    void Start()
    {
        this.CurrentHealth = health;
        this.CurrentSpeed = speed;
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }
    
    private void UpdatePath()
    {
        if (seeker.IsDone() && currentTarget)
        {
            seeker.StartPath(rb.position, currentTarget.position, OnPathComplete);
        }
    }

    private void Update()
    {
        RaycastPhysics();

        if (currentTarget)
        {
            Attack(currentTarget);
        }
    }

    void FixedUpdate()
    {
        if (currentTarget)
        {
            CalculatePath();
        }

        if (damaged)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Damaged"))
            {
                Knockback(damageRecieved, knockbackOrigin);
            }
            else
            {
                damaged = false;
                damageRecieved = 0;
                knockbackOrigin = null;
            }
        }
    }

    #region PATHFINDING
    
    private void Move()
    {
        isGrounded = Physics2D.Raycast(transform.position, -Vector3.up, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset);

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        JumpCheck(direction.y);

        rb.AddForce(force);
        //rb.velocity = force;
    }

    private void JumpCheck(float nodeYPosition)
    {
        if (isGrounded && type != EnemyType.heavy)
        {
            if (nodeYPosition > jumpNodeHeightRequirement)
            {
                rb.AddForce(Vector2.up * speed * jumpModifier);
            }
        }
    }

    private void CalculatePath()
    {
        if (path is null)
        {
            return;
        }
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        Move();

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // Face the right direction
        if (rb.velocity.x >= 0.05f)
        {
            anim.transform.localScale = new Vector3(1f, 1f, 1f);
            myEyes.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (rb.velocity.x <= -0.05f)
        {
            anim.transform.localScale = new Vector3(-1f, 1f, 1f);
            myEyes.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    #endregion
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            Debug.Log("DEAL DAMAGE");
            currentTarget = collision.transform;
            Player.instance.PlayerDamageRecieved(attackController.damage, transform);
        }
    }

    private bool CheckDistanceAttack(Transform target)
    {
        return Vector2.Distance(transform.position, target.position) <= minAtkDistance;
    }

    private bool CheckToStopChasing(Transform target)
    {
        return (Vector2.Distance(transform.position, target.position) >= stopChaseDistance);
    }

    private void RaycastPhysics()
    {
        RaycastHit2D hit = Physics2D.Raycast(myEyes.position, myEyes.right, visionRange);
        Debug.DrawRay(myEyes.position, myEyes.right * visionRange, Color.red, 0.1f);

        if (hit.collider != null)
        {
            if (hit.collider.tag.Equals("Player") )
            {
                KeepTrackOfTarget(hit.transform);
            }
        }
        else
        {
            OnSearchEnd();
        }
    }

    private void KeepTrackOfTarget(Transform target)
    {
        currentTarget = target.transform;
    }

    private void OnSearchEnd()
    {
        if (currentTarget)
        {
            if (CheckToStopChasing(currentTarget))
            {
                currentTarget = null;
            }
        }
    }

    protected override bool CanAttack(Transform target)
    {
        if (CheckDistanceAttack(target))
        {
            Debug.Log("IN RANGE OF ATTACK!!");
            return true;
        }
        else
        {
            return false;
        }
    }
    
    protected override void DoAttack()
    {
        this.CurrentSpeed = 0;
        anim.SetBool("InSight", false);
        anim.SetTrigger("Attack");
        WaitAfterAttack(timePerAttack);
    }

    IEnumerator WaitAfterAttack(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        if (currentTarget)
        {
            KeepTrackOfTarget(currentTarget);
        }
        else
        {
            OnSearchEnd();
        }
    }

    private void Knockback(float damageRecieved, Transform knockbackOrigin)
    {
        Vector2 direction = transform.position - knockbackOrigin.position;
        rb.AddForce(new Vector2(direction.x * knockbackModifier, direction.y * knockbackModifier), ForceMode2D.Impulse);
        damaged = false;
    }

    protected override void DamageReceived(bool isDead, Transform enemyPosition)
    {
        if (isDead)
        {
            knockbackOrigin = enemyPosition;
            effectsController.SetActive(true);
            anim.SetTrigger("Hurt");
            Collider[] colliders = GetComponentsInChildren<Collider>();
            for (int i = 0; i < colliders.Length; i++)
            {
                rb.isKinematic = true;
                colliders[i].enabled = false;
            }
        }
        else
        {
            this.health = this.CurrentHealth;
            anim.SetBool("InSight", false);
            anim.SetTrigger("Hurt");
        }
    }

    protected override void Patroling()
    {
        this.CurrentSpeed = speed;
    }

    protected override bool CanChase()
    {
        if (currentTarget is null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    protected override void DoChase(Transform target)
    {
        this.CurrentSpeed = speed;
    }

}
