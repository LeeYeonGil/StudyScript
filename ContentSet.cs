using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentSet : MonoBehaviour
{
    [SerializeField] RectTransform contentrect;

    // Update is called once per frame
    void Update()
    {
        contentrect.anchoredPosition = Vector3.zero;
    }
}
