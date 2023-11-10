using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SetLast_SetActive : MonoBehaviour
{
    public bool OC = false;
    // Start is called before the first frame update
    public void UI_SetLast_OC(bool donttouchActive)
    {
        OC = !OC;
        if (donttouchActive) UIManager.Instance.DontTouchOC(OC);
        gameObject.transform.SetAsLastSibling();
        gameObject.SetActive(OC);
    }
}
