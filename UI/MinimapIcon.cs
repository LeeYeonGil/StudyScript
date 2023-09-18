using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapIcon : MonoBehaviour
{
    public void Initialize(Transform target)
    {
        StartCoroutine(Following(target));
    }
    IEnumerator Following(Transform target)
    {
        Vector2 size = transform.parent.GetComponent<RectTransform>().sizeDelta;
        RectTransform rt = GetComponent<RectTransform>();
        while(target != null)
        {
            Vector3 pos = Camera.allCameras[2].WorldToViewportPoint(target.position);
            rt.anchoredPosition = pos * size;
            yield return null;
        }
    }
}
