using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerMotor : MonoBehaviour
{
    private Vector3 startTouchPosition;
    private Vector3 endTouchPosition;
    private string input;

    public float Speed { get => speed; }
    private float speed = 6.2f;
    private CapsuleCollider controller;
    private Animator anim;
    private Vector3 runDirection;
    private float cd = 0;
    public bool isStopped = false;

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    private void SwipeInput()
    {
        if (IsPointerOverUIObject())
        {
            input = "";
            return;
        }

        input = Input.inputString;
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            startTouchPosition = Input.GetTouch(0).position;
        }

        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            endTouchPosition = Input.GetTouch(0).position;

            if (Mathf.Abs(endTouchPosition.x - startTouchPosition.x) > Mathf.Abs(endTouchPosition.y - startTouchPosition.y))
            {
                if (endTouchPosition.x < startTouchPosition.x)
                {
                    input = "a";
                }
                else if (endTouchPosition.x > startTouchPosition.x)
                {
                    input = "d";
                }
            }
            else
            {
                if (endTouchPosition.y > startTouchPosition.y)
                {
                    input = "w";
                }
                else if (startTouchPosition.y > endTouchPosition.y)
                {
                    input = "s";
                }
            }
        }
    }

    void Start()
    {
        profile = GetComponent<PlayerProfile>();
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
        if(!isStopped)
            speed += Time.deltaTime / 100;
    }

    public void GameOver()
    {
        FirebaseManager.GameOver(PlayerPrefs.GetString("name"),GetComponent<PlayerProfile>().Score);
    }

    PlayerProfile profile;

    private void HandleMove()
    {

        //controller.Move(speed * Time.deltaTime * runDirection);
        transform.position += new Vector3(runDirection.x * Time.deltaTime * speed * Potions.Swiftness,runDirection.y * Time.deltaTime * speed * Potions.Swiftness,runDirection.z * Time.deltaTime * speed * Potions.Swiftness);
        SwipeInput();
        if (string.IsNullOrEmpty(input))
            return;

        if ((input == "d" || input == "a") && cd > 1f)
        {
            cd = 0f;
        } else if((input == "d" || input == "a") && cd < 1f)
        {
            return;
        }
        switch (input)
        {
            case " ": case "w":
                //anim.SetTrigger("jump_" + Random.Range(1,3));
                if ((anim.GetCurrentAnimatorStateInfo(0).IsName("Jump2") || anim.GetCurrentAnimatorStateInfo(0).IsName("Jump")) && !AnimationController.isRunning)
                    break;
                else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Slide") && !AnimationController.isRunning)
                {
                    anim.Play("Run");
                    controller.height = 1.6f;
                    controller.radius = 0.32f;
                    controller.center = new Vector3(0, 0.8f, 0);
                    break;
                }
                int r = Random.Range(1, 3);
                if (r == 1)
                    anim.Play("Jump");
                else
                    anim.Play("Jump2");
                controller.center = new Vector3(0, 3, 0);
                //Vector3.MoveTowards(cvc.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset, new Vector3(0, 2, 0), 0.4f);

                break;
            case "s":
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Slide") && !AnimationController.isRunning)
                    break;
                else if((anim.GetCurrentAnimatorStateInfo(0).IsName("Jump2") || anim.GetCurrentAnimatorStateInfo(0).IsName("Jump")) && !AnimationController.isRunning)
                {
                    anim.Play("Run");
                    controller.height = 1.6f;
                    controller.radius = 0.32f;
                    controller.center = new Vector3(0, 0.8f, 0);
                    break;
                }
                anim.Play("Slide");
                //anim.SetTrigger("slide");
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
        if (other.gameObject.tag == "Ground" || other.isTrigger || (Potions.Invicible && other.gameObject.tag != "Wall"))
            return;

        isStopped = true;
        AssetsHandler.Instance.endUI.SetActive(true);

        // depending on hit type choose which animation to play
        anim.Play("Fall");
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
