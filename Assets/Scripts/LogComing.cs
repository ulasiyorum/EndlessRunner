using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogComing : MonoBehaviour
{
    private int rotation;
    private Animator anim;
    private bool coming;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rotation = GroundMotor.currentAngle;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Agent")
            return;
        coming = true;
        anim.SetTrigger("log");
    }

    // Update is called once per frame
    void Update()
    {
        if (!coming)
            return;

        if (rotation == -90)
            transform.position += new Vector3(0, 0, Time.deltaTime * -3.6f);
        else if (rotation == 90)
            transform.position += new Vector3(0, 0, Time.deltaTime * 3.6f);
        else if (rotation == 0)
            transform.position += new Vector3(-3.6f * Time.deltaTime, 0, 0);
        else
            transform.position += new Vector3(Time.deltaTime * 3.6f, 0, 0);


        transform.Rotate(Time.deltaTime * 2f,0,0);
    }
}
