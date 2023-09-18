using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Potal : MonoBehaviour
{
    public Transform NextPotal;
    public GameObject[] Potals;
    [SerializeField] bool potal_ONOFF = false;
    [SerializeField] GameObject player;
    [SerializeField] TMP_Text text;
    public GameObject curStage;
    public GameObject nextStage;

    public bool clearChk = false;
    public bool used = false;

    public STATE myState = STATE.Create;
    Coroutine cor = null;

    public enum STATE
    {
        Create, Active, inActive, reCreate
    }

    void ChangeState(STATE s)
    {
        if (myState == s) return;
        myState = s;
        switch (myState)
        {
            case STATE.reCreate:
                used = false;
                clearChk = false;
                Potals[0].SetActive(true);
                if(cor != null)
                {
                    StopCoroutine(cor);
                    cor = null;
                }
                cor = StartCoroutine(Potal_ChangeState(0.8f, STATE.Active));
                break;
            case STATE.Create:
                break;
            case STATE.Active:
                Potals[0].SetActive(false);
                Potals[1].SetActive(true);
                break;
            case STATE.inActive:
                Potals[1].SetActive(false);
                Potals[2].SetActive(true);
                if (cor != null)
                {
                    StopCoroutine(cor);
                    cor = null;
                }
                cor = StartCoroutine(Potal_End(0.4f));
                break;
        }
    }
    void StateProcess()
    {
        switch (myState)
        {
            case STATE.reCreate:
                break;
            case STATE.Create:
                break;
            case STATE.Active:
                if (potal_ONOFF)
                {
                    text.gameObject.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        used = true;
                        StartCoroutine(PlayerMove());
                    }
                }
                else
                {
                    text.gameObject.SetActive(false);
                }
                break;
            case STATE.inActive:
                if (clearChk)
                {
                    ChangeState(STATE.Create);
                }
                break;
        }
    }


    // Update is called once per frame
    void Update()
    {
       // otherPotal.used = used;
        clearChk = GameManager.Instance.clearChk;
        StateProcess();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6) // player
        {
            player = other.gameObject;
            potal_ONOFF = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6) // player
        {
            potal_ONOFF = false;
        }
    }

    IEnumerator Potal_ChangeState(float sec, STATE state) // 0.8 / 2 / 0.4
    {
        WaitForSeconds s = new WaitForSeconds(sec);
        yield return s;

        ChangeState(state);
    }
    IEnumerator Potal_End(float sec) // 0.8 / 2 / 0.4
    {
        WaitForSeconds s = new WaitForSeconds(sec);
        yield return s;
        used = false;
        //otherPotal.used = used;
        Potals[2].SetActive(false);
    }


    IEnumerator PlayerMove()
    {
        nextStage.SetActive(true);
        player.transform.position = NextPotal.transform.position;
        UIManager.Instance.StartStage(nextStage.name);
        GameManager.Instance.StageStart();
        nextStage.GetComponent<STAGE>().Start_Stage();
        if(!GameManager.Instance.BossStageChk) GameManager.Instance.currentNum++;
        SoundManager.Instance.ChangeMusic();
        ChangeState(STATE.inActive);
        yield return new WaitForSeconds(10.0f);
        curStage.GetComponent<STAGE>().clear = false;
        curStage.SetActive(false);
        yield return null;
    }

    public void Potal_Active()
    {
        ChangeState(STATE.reCreate);
    }

    public void Potal_reset()
    {
        ChangeState(STATE.Create);
    }

}
