using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using System.IO;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class NPC : MonoBehaviour
{
    [SerializeField] PlayableDirector npcdirector;
    public TextAsset[] talking;
    Coroutine Cor_text;
    public RPGPlayer player;
    [SerializeField] bool playerchk = false;
    public bool QuestChk = true;
    public bool ClearChk = false;
    public bool talkchk = false;
    public bool questtalkchk = false;

    public GameObject pushText;
    public GameObject talkText;
    public GameObject buttons;
    public GameObject YesNo;
    public GameObject DontTouch;


    public GameObject QuestIcon;
    public GameObject ClearIcon;

    public TMP_Text Talk;
    [SerializeField] string[] talks;
    public int talkNum = 0;
    IEnumerator currenttalk;
    public int talk_num = 0;
    public int buttonOnOff = 0;

    public Potal potal;

    //public playable[] timelines;

    private void Awake()
    {
        npcdirector = GetComponent<PlayableDirector>();
        talkText = UIManager.Instance.TalkBox;
        DontTouch = UIManager.Instance.DontTouch;
    }
    private void Start()
    {
        QuestIcon.SetActive(false);
        ClearIcon.SetActive(false);
    }

    void Update()
    {
        ClearChk = GameManager.Instance.bossClearChk;
        if (playerchk)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (!talkchk)
                {
                    //대화창 + 상점
                    DontTouch.SetActive(true);
                    talkText.SetActive(true);
                    npcdirector.Play();
                    talkchk = true;
                    if (!ClearChk)
                    {
                        talk_num = 0;
                        buttonOnOff = 0;
                    
                    }
                    else
                    {
                        talk_num = 5;
                        buttonOnOff = 3;
                        GameManager.Instance.ExitPoint.SetActive(true);
                    }

                }

                if (Cor_text != null)
                {
                    StopCoroutine(Cor_text);
                    Cor_text = null;
                }
                Cor_text = StartCoroutine(UIManager.Instance.NPCTalk(talking[talk_num], buttonOnOff));
            }
            if (!talkchk)
            {
                npcdirector.Stop();
            }
        }
        if(QuestChk)
        {
            if(!ClearChk) QuestIcon.SetActive(true);
        }
        else
        {
            QuestIcon.SetActive(false);
            if (ClearChk)
            {
                ClearIcon.SetActive(true);
            }
            else
            {
                ClearIcon.SetActive(false);
            }
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            player = other.GetComponent<RPGPlayer>();
            talkchk = false;
            playerchk = true;
            pushText.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        playerchk = false;
        pushText.SetActive(false);
    }

    public void Quest()
    {
        UIManager.Instance.talkbuttons.SetActive(false);
        talk_num = 1;
        buttonOnOff = 1;
        if (Cor_text != null)
        {
            StopCoroutine(Cor_text);
            Cor_text = null;
        }
        Cor_text = StartCoroutine(UIManager.Instance.NPCTalk(talking[1], 1));
    }

    public void QuestStart() // 퀘스트 시작
    {
        talk_num = 2;
        buttonOnOff = 3;
        UIManager.Instance.YesNobuttons.SetActive(false);
        if (GameManager.Instance.player.GetComponent<MySkill>().weapon_item.GetComponent<Slot_Item>()._Item.item_data != null)
        {
            QuestChk = false;
            if (Cor_text != null)
            {
                StopCoroutine(Cor_text);
                Cor_text = null;
            }
            Cor_text = StartCoroutine(UIManager.Instance.NPCTalk(talking[2], 3)); // 퀘스트 진행
            potal.gameObject.SetActive(true);
            potal.Potal_Active();
            GameManager.Instance.StageStart();
        }
        else
        {
            talk_num = 3;
            buttonOnOff = 3;
            if (Cor_text != null)
            {
                StopCoroutine(Cor_text);
                Cor_text = null;
            }
            Cor_text = StartCoroutine(UIManager.Instance.NPCTalk(talking[4], 3)); // 퀘스트 진행
        }
        
    }
    public void QuestRefuse() // 퀘스트 거절
    {
        talk_num = 3;
        buttonOnOff = 3;
        UIManager.Instance.YesNobuttons.SetActive(false);
        if (Cor_text != null)
        {
            StopCoroutine(Cor_text);
            Cor_text = null;
        }
        Cor_text = StartCoroutine(UIManager.Instance.NPCTalk(talking[3],3)); // 퀘스트 거절
    }



}
