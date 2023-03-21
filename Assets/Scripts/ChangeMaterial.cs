using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    [SerializeField] MaterialChanger[] materials;
    public static ChangeMaterial i;
    [SerializeField] Material invisMat;

    // Start is called before the first frame update
    void Start()
    {
        i = this;
    }

    public void Change(bool normal)
    {
        if(normal)
        {
            Array.ForEach(materials,((value) =>
            {
                value.ChangeMaterialToNormal();
            }));
        } 
        else
        {
            Array.ForEach(materials, ((value) =>
            {
                value.ChangeMaterialToInvisible(invisMat);
            }));
        }

    }
}
