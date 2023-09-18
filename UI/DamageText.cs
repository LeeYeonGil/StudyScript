using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public Transform myTarget;
    public TMP_Text text;
    Coroutine dm;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }
    public void damage_text(float dmg)
    {
        text.gameObject.SetActive(true);
        if (dm != null)
        {
            StopCoroutine(dm);
            dm = null;
        }
        dm = StartCoroutine(Damage_Text(dmg));
    }
    IEnumerator Damage_Text(float dmg)
    {
        float i = 0.1f;
        while (i < 200.0f)
        {
            transform.position = Camera.main.WorldToScreenPoint(myTarget.position) + new Vector3(0, i++, 0);
            text.text = Mathf.Clamp(dmg, 0, dmg).ToString("0.0");
            yield return null;
        }
        text.gameObject.SetActive(false);
    }
}
