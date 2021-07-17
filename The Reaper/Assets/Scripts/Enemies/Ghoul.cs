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
    [SerializeField] private float visionRange;
    [SerializeField] private float startSearchDistance;
    [SerializeField] private bool isSearching;
    [SerializeField] private float timePerAttack;

    [Header("References")]
    [SerializeField] private Transform myEyes;
    [SerializeField] private Transform currentTarget;
    [SerializeField] private Animator anim;
    
    private int currentWaypoint;
    private bool reachedEndOfPath;
    private Path path;
    private Seeker seeker;
    private Rigidbody2D rb;
    private Transform targetLastSeen;

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
        CheckIfTarget();

        if (isSearching)
        {
            OnSearchEnd();
        }

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
    }

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

    private bool CheckDistanceAttack(Transform target)
    {
        return Vector2.Distance(transform.position, target.position) <= minAtkDistance;
    }

    private bool CheckStartSearchingDistance(Transform target)
    {
        return Vector2.Distance(transform.position, target.position) > startSearchDistance;
    }

    private bool CheckToStopSearching(Transform target)
    {
        if (Vector2.Distance(transform.position, target.position) <= 0.85f)
        {
            if (!currentTarget.tag.Equals("Player"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    private void CheckIfTarget()
    {
        RaycastHit2D hit = Physics2D.Raycast(myEyes.position, myEyes.right, visionRange);
        Debug.DrawRay(myEyes.position, myEyes.right * visionRange, Color.red, 0.1f);

        if (hit.collider != null && !isSearching)
        {
            if (hit.collider.tag.Equals("Player") )
            {
                KeepTrackOfTarget(hit.transform);
            }
        }
        else if (hit.collider != null && isSearching)
        {
            if (hit.collider.tag.Equals("Player"))
            {
                Debug.Log("TRACKING TARGET");
                KeepTrackOfTarget(hit.transform);
            }
            else
            {
                Debug.Log("SEARCHING TARGET");
                CheckTargetLastPosition();
            }
        }
        else if (hit.collider is null && isSearching)
        {
            OnSearchEnd();
        }
    }

    private void CheckTargetLastPosition()
    {
        currentTarget = targetLastSeen;
        OnSearchEnd();
    }

    private void KeepTrackOfTarget(Transform target)
    {
        isSearching = true;
        currentTarget = target.transform;

        if (targetLastSeen is null)
        {
            targetLastSeen = new GameObject().transform;
            targetLastSeen.position = currentTarget.position;
        }
        else
        {
            targetLastSeen.position = currentTarget.position;
        }
    }

    private void OnSearchEnd()
    {
        if (currentTarget)
        {
            if (CheckToStopSearching(targetLastSeen))
            {
                isSearching = false;
                Destroy(targetLastSeen.gameObject);
                targetLastSeen = null;
                currentTarget = null;
            }
        }
        else
        {
            if (targetLastSeen)
            {
                Destroy(targetLastSeen.gameObject);
                targetLastSeen = null;
                currentTarget = null;
            }
            isSearching = false;
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
        OnSearchEnd();

        yield return new WaitForSeconds(seconds);

        if (currentTarget)
        {
            KeepTrackOfTarget(currentTarget);
            CheckTargetLastPosition();
        }
        else
        {
            OnSearchEnd();
        }
    }

    protected override void DamageReceived(bool isDead)
    {
        this.health = this.CurrentHealth;
        anim.SetBool("InSight", false);
        anim.SetTrigger("Hurt");
    }

    protected override void Patroling()
    {
        this.CurrentSpeed = speed;

    }

    protected override void Searching(Vector3 lastSeen)
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
