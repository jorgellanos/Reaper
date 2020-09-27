using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieRun : StateMachineBehaviour
{
    public EnemyZ1 me;
    public Transform player;
    public Rigidbody2D rb;
    public float speed, ATKrange, dist;
    public bool inSight;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        me = animator.GetComponent<EnemyZ1>();
        player = me.target;
        rb = animator.GetComponent<Rigidbody2D>();
        speed = me.speed;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //me.LookAtPlayer();
        dist = me.dist;
        Vector2 target = new Vector2(player.transform.position.x, rb.transform.position.y);
        Vector2 z = Vector2.MoveTowards(animator.transform.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(z);

        if (me.dist <= ATKrange)
        {
            animator.SetTrigger("Attack");
        }
        else
        {
            speed = me.speed;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }
}
