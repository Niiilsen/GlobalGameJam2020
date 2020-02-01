using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ApplyMaterial : MonoBehaviour
{
    public Material[] materials;

    public MeshRenderer[] blocks;
    
    // Start is called before the first frame update
    private void OnEnable()
    {
        foreach (MeshRenderer block in blocks)
        {
            block.material = materials[Random.Range(0, materials.Length)];
        }
    }
}
