using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    private Vector3 spaceBetween;
    void Start()
    {
        GameHandler.Instance.player.enabled = true;
        spaceBetween = transform.position;
    }

    void Update()
    {
        transform.position = GameHandler.Instance.player.transform.position + spaceBetween;
    }
}
