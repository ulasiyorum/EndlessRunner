using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMotor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        Generate();
    }

    public void Generate()
    {
        GameObject go = Instantiate(this.gameObject);
        Vector3 v = go.transform.position;
        float size = go.GetComponent<BoxCollider>().bounds.size.z;
        go.transform.position = new Vector3(v.x, v.y, v.z + size);
        Destroy(this.gameObject, 0.35f);
    }
}
