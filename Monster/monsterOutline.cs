using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class monsterOutline : MonoBehaviour
{
    public SkinnedMeshRenderer meshRenderer;
    Color _color;
    // Start is called before the first frame update
    void Start()
    {
        _color = meshRenderer.material.GetColor("_OutLineColor");
        
    }

    // Update is called once per frame
    void Update()
    {
        _color = Color.white;
    }
}
