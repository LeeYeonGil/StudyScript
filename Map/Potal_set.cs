using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potal_set : MonoBehaviour
{
    public GameObject[] Potal;
    public float delay = 0.0f;
    public int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Potal_Manager());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Potal_Manager()
    {
        while (count < 3)
        {
            yield return new WaitForSeconds(0.1f);
            Potal[count].SetActive(true);
            if (count == 0)
            {
                if (delay >= 0.8f)
                {
                    count++;
                    delay = 0.0f;
                }
            }
            if (count == 1)
            {
                Potal[count - 1].SetActive(false);
                if (delay >= 10.0f)
                {
                    count++;
                    delay = 0.0f;
                }
            }
            if (count == 2)
            {
                Potal[count - 1].SetActive(false);
                if (delay >= 1.0f)
                {
                    count++;
                    delay = 0.0f;
                    Destroy(gameObject, 5.0f);
                }
            }
            delay += 0.1f;
        }
    }

   
}
