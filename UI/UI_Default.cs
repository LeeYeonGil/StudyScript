using DTT.Utils.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Default : MonoBehaviour
{
    public Vector3 orgPos;
    public RectTransform rectTransform;
    public bool OC = false;
    public virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        orgPos = rectTransform.localPosition;
        OC = true;
        UI_OC();
    }
    public virtual void UI_OC()
    {
        gameObject.transform.localPosition = orgPos;
        OC = !OC;
        if (UIManager.Instance.items.Count > 0) UIManager.Instance.UIOC(this);
        gameObject.SetActive(OC);
    }

    public void CloseButton()
    {
        OC = false;
    }
}
