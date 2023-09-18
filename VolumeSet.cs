using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VolumeSet : MonoBehaviour
{
    public Slider Volume;

    private void Awake()
    {
        Volume = GetComponent<Slider>();
    }

    
}
