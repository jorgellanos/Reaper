using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Z1_idle : StateMachineBehaviour
{
    private bool isMoving;
    private float speed, dir;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetComponent<EnemyZ1>().isFacingRight)
        {
            dir = 1;
        }
        else
        {
            dir = -1;
        }
        isMoving = true;
        speed = animator.GetComponent<EnemyZ1>().speed;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetComponent<EnemyZ1>().type == EnemyZ1.Tipo.patrol)
        {
            Move(animator.GetComponent<EnemyZ1>());
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    private void Move(EnemyZ1 obj)
    {
        if (obj.m_Grounded)
        {
            if (!obj.fallWarning)
            {
                Debug.Log("TURN!!");
                TurnAround(obj);
                dir *= -1;
            }
        }
        obj.GetComponent<Rigidbody2D>().velocity = new Vector2(speed * dir, 0);
    }

    public void TurnAround(EnemyZ1 en)
    {
        en.transform.rotation = Quaternion.Euler(0, 180, 0);
        if (en.isFacingRight)
        {
            en.isFacingRight = false;
            dir = -1;
        }
        else
        {
            en.isFacingRight = true;
            dir = 1;
        }
    }
}
