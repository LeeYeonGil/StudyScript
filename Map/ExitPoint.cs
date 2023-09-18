using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExitPoint : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] bool playerChk = false;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if(playerChk)
        {
            text.gameObject.SetActive(true);
            if(Input.GetKeyDown(KeyCode.F))
            {
                LoadingScene.LoadScene("MainMenu");
            }
        }
        else
        {
            text.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            playerChk = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            playerChk = false;
        }
    }
}
