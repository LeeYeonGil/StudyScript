using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Transform myTarget;
    public Slider myBar;
    // Update is called once per frame

    public void HPbar_set()
    {
        myBar = GetComponent<Slider>();
        StartCoroutine(Hpbar_set());
    }
    IEnumerator Hpbar_set()
    {
        while (true)
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(myTarget.position);
            if (pos.z < 0.0f)
            {
                transform.position = new Vector3(0, 10000, 0);
            }
            else
            {
                transform.position = pos;
            }
            yield return null;
        }
    }
}
