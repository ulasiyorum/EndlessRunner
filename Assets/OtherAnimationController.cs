using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class OtherAnimationController : StateMachineBehaviour
{
    public static string[] triggerNames = { "ninjaJump", "roll", "jump_2", "slide", "jump_1", "turn" };
    private void Awake()
    {

    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsName("Jump") || stateInfo.IsName("Jump2"))
            AudioManager.Play(1);
        else if (stateInfo.IsName("Slide"))
            AudioManager.Play(3);
        base.OnStateEnter(animator, stateInfo, layerIndex);

        AnimationController.isRunning = false;
        AnimationController.SetRunningTrueAsync();

        foreach (string trigger in triggerNames)
        {
            animator.ResetTrigger(trigger);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {   
        base.OnStateExit(animator, stateInfo, layerIndex);

        if (stateInfo.IsName("Jump") || stateInfo.IsName("Jump2"))
            AudioManager.PlayOneShot(0);
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
