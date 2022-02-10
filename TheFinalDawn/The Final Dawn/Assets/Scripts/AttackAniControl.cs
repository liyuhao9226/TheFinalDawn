using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAniControl : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       heros = GameObject.FindGameObjectsWithTag("heropiece");
        enemies = GameObject.FindGameObjectsWithTag("enemy");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
      GameObject[] heros;
    GameObject[] enemies;
    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        heros = GameObject.FindGameObjectsWithTag("heropiece");
        enemies = GameObject.FindGameObjectsWithTag("enemy");
        foreach (var item in heros)
        {         
            item.GetComponent<HeroBasic>().ani.SetBool("isattack", false);
           // Debug.Log(22);
        }
        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.GetComponent<Enemy>().ani.SetBool("isattack", false);
            }
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
