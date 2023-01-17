using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMotor : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    private CharacterController controller;
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMove();
    }

    private void HandleMove()
    {
        controller.Move(speed * Time.deltaTime * Vector3.forward);

        string input = Input.inputString;

        if (string.IsNullOrEmpty(input) || anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.ToLower() != "run")
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
            default:
                break;
        }

    
    }
}
