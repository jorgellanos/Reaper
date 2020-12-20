using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Z1_idle : StateMachineBehaviour
{
    private bool isMoving;
    private float speed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isMoving = true;
        speed = animator.GetComponent<EnemyZ1>().speed;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetComponent<EnemyZ1>().type == EnemyZ1.Tipo.patrol)
        {
            Move(animator.gameObject);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    private void Move(GameObject obj)
    {
        float dir = 1;
        if (obj.GetComponent<EnemyZ1>().isFacingRight)
        {
            dir = 1;
        }
        else
        {
            dir = -1;
        }
        obj.GetComponent<Rigidbody2D>().velocity = new Vector2(speed * dir, 0);
    }
}
