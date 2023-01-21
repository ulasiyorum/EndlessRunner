using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMotor : MonoBehaviour
{
    [SerializeField] float speed = 3f;
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
    }

    private void HandleMove()
    {
        controller.Move(speed * Time.deltaTime * runDirection);

        string input = Input.inputString;

        if (string.IsNullOrEmpty(input) || anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.ToLower() != "run")
            return;

        Debug.Log(input);
        Vector3 newDir = runDirection;
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
                newDir = Vector3.right;
                break;
            case "a":
                transform.Rotate(0, 270, 0);
                newDir = Vector3.left;
                break;
            default:
                break;
        }

        // Find new running direction according to player's direction

    }
}
