using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    public Transform Target;
    public Transform Obstruction;
    //private Vector3 spaceBetween;
    void Start()
    {
        Obstruction = Target;
        //GameHandler.Instance.player.enabled = true;
        //spaceBetween = transform.position;
    }

    void Update()
    {
        //transform.rotation = Quaternion.Slerp(transform.rotation, GameHandler.Instance.player.transform.rotation,0.1f);
        //transform.position = GameHandler.Instance.player.transform.position + spaceBetween;
    }

    private void LateUpdate()
    {
    }

    
}
