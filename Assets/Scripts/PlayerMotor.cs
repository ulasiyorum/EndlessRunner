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
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        runDirection = Vector3.forward;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMove();

        string input = Input.inputString;
        Debug.Log(input);
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
                break;
            case "s":
                anim.SetTrigger("slide");
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
