using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMotor : MonoBehaviour
{
    [SerializeField] float speed = 4f;
    private CharacterController controller;
    private Animator anim;
    private Vector3 runDirection;

    public bool isStopped = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        runDirection = Vector3.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isStopped)
            HandleMove();
    }

    private void HandleMove()
    {
        
        controller.Move(speed * Time.deltaTime * runDirection);

        string input = Input.inputString;

        if (string.IsNullOrEmpty(input) || !AnimationController.isRunning)
            return;

        Debug.Log(input);

        switch(input)
        {
            case " ": case "w":
                anim.SetTrigger("jump_" + Random.Range(1,3));
                controller.center = new Vector3(0, 3, 0);
                //Vector3.MoveTowards(cvc.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset, new Vector3(0, 2, 0), 0.4f);

                break;
            case "s":
                anim.SetTrigger("slide");
                controller.height = 0.25f;
                controller.radius = 0.25f;
                controller.center = new Vector3(0, 0, 0);
                break;
            case "d":
                transform.Rotate(0, 90, 0);
                break;
            case "a":
                transform.Rotate(0, 270, 0);
                break;
            default:
                break;
        }

        ChangeDirection();
    }

    public void OnControllerAnimationEnd(AnimatorStateInfo stateInfo)
    {
        if(stateInfo.IsTag("jump"))
            controller.center = new Vector3(0, 0.8f, 0);

        if(stateInfo.IsTag("slide"))
        {
            controller.height = 1.6f;
            controller.radius = 0.32f;
            controller.center = new Vector3(0, 0.8f, 0);
        }

        //Vector3.MoveTowards(cvc.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset, new Vector3(0, 0.3f, 0), 0.4f);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.tag == "Ground")
            return;

        isStopped = true;

        // depending on hit type choose which animation to play

        anim.SetTrigger("fall_1");

        CancelInvoke("Destroy");
    }

    private void ChangeDirection()
    {

        float y = transform.localEulerAngles.y;

        runDirection = y switch
        {
            0 => Vector3.forward,
            90 => Vector3.right,
            180 => Vector3.back,
            270 => Vector3.left,
            _ => Vector3.forward,
        };
    }
}
