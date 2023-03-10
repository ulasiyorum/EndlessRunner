using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = new Quaternion(transform.parent.localRotation.x,transform.parent.localRotation.y * 2, transform.parent.localRotation.z,transform.parent.localRotation.w);
    }
}
