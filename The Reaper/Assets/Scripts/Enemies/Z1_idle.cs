using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Z1_idle : StateMachineBehaviour
{
    private Transform pointA, pointB, target;
    private bool isMoving;
    private float speed, time;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isMoving = true;
        time = 3f;
        speed = animator.GetComponent<EnemyZ1>().speed;
        pointA = animator.GetComponent<EnemyZ1>().pointA;
        pointB = animator.GetComponent<EnemyZ1>().pointB;
        target = pointA;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        LookAtTarget(animator.GetComponent<EnemyZ1>().isFacingRight, animator.transform);
        Move(animator.transform);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    private void Move(Transform transform)
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        if (transform.position == target.position)
        {
            isMoving = false;
            Timer();
        }
    }

    private void Timer()
    {
        time -= 1f;
        if (time <= 0)
        {
            time = 0;
            isMoving = true;
            if (target == pointA)
            {
                target = pointB;
            }
            else
            {
                target = pointA;
            }
        }
    }

    private void LookAtTarget(bool isFacingRight, Transform transform)
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
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
