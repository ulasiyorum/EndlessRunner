using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMotor : MonoBehaviour
{
    private float speed = 6.7f;
    private CapsuleCollider controller;
    private Animator anim;
    private Vector3 runDirection;
    private float cd = 0;
    public bool isStopped = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CapsuleCollider>();
        runDirection = Vector3.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isStopped)
            HandleMove();

        cd += Time.deltaTime;
    }


    private void HandleMove()
    {

        //controller.Move(speed * Time.deltaTime * runDirection);
        transform.position += new Vector3(runDirection.x * Time.deltaTime * speed,runDirection.y * Time.deltaTime * speed,runDirection.z * Time.deltaTime * speed);
        string input = Input.inputString;

        if (string.IsNullOrEmpty(input) || !AnimationController.isRunning)
            return;

        Debug.Log(input);

        if ((input == "d" || input == "a") && cd < 1f)
        {
            cd = 0f;
            return;
        }
        switch (input)
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
                controller.center = new Vector3(0, 0.1f, 0);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ground" || other.isTrigger)
            return;

        isStopped = true;
        AssetsHandler.Instance.endUI.SetActive(true);

        // depending on hit type choose which animation to play

        anim.SetTrigger("fall_1");
    }

    public bool ChangeDirection()
    {

        Vector3 lastDirection = runDirection;
        float y = transform.localEulerAngles.y;
        runDirection = y switch
        {
            0 => Vector3.forward,
            90 => Vector3.right,
            180 => Vector3.back,
            270 => Vector3.left,
            _ => Vector3.forward,
        };


        if (runDirection != lastDirection)
        {
            GroundMotor[] motors = FindObjectsOfType<GroundMotor>();
            GameObject go = null;
            int rNumber = 0;
            foreach (var motor in motors)
            {
                if (motor.transform.parent.GetComponent<BoxCollider>().bounds.Contains(transform.position))
                    rNumber = motor.roomNumber + 1;
            }
            foreach(var motor in motors)
            {
                if (motor.roomNumber == rNumber)
                    go = motor.transform.parent.gameObject;
            }

            if (Vector3.Distance(transform.position, go.transform.position) > 3.2f)
                return false;

            if (runDirection == Vector3.forward || runDirection == Vector3.back)
            {
                
                Debug.Log("run x");
                transform.position = new Vector3(go.transform.position.x, transform.position.y, transform.position.z);
            } else
            {
                Debug.Log("run y");
                transform.position = new Vector3(transform.position.x, transform.position.y, go.transform.position.z);
            }
        }
        return true;
    }

}
