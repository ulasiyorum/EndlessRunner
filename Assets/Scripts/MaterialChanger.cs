using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    private Renderer[] materialRenderers;
    private Material[] materials;
    void Start()
    {
        materialRenderers = GetComponents<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeMaterialToInvisible(Material invisMat)
    {
        materials = new Material[materialRenderers.Length];
        for (int i = 0; i < materialRenderers.Length; i++)
        {
            materials[i] = materialRenderers[i].material;
            materialRenderers[i].material = invisMat;
        }
    }
    public void ChangeMaterialToNormal()
    {
        if (materials == null)
            return;

        for (int i = 0; i < materials.Length; i++)
        {
            materialRenderers[i].material = materials[i];
        }

    }
}

