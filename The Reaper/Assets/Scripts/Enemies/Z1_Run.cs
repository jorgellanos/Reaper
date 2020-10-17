using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Z1_Run : StateMachineBehaviour
{
    private Transform target, me;
    private float speed, distance, attackRange;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        me = animator.GetComponent<Transform>();
        speed = animator.GetComponent<EnemyZ1>().speed;
        attackRange = animator.GetComponent<EnemyZ1>().attackRange;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        me.transform.position = Vector2.MoveTowards(me.transform.position, target.position, speed * Time.fixedDeltaTime);
        distance = Vector2.Distance(me.transform.position, target.position);
        if (distance <= attackRange)
        {
            animator.SetTrigger("Attack");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }
}
