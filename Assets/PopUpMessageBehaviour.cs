using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class PopUpMessageBehaviour : MonoBehaviour
{
    TMP_Text text;
    void Start()
    {
        text = GetComponent<TMP_Text>();
        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, Time.deltaTime * 3f, 0);
        text.color -= new Color(0, 0, 0, Time.deltaTime / 2);
    }
}
