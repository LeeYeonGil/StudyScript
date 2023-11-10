using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using JetBrains.Annotations;
using System.Globalization;

public class UIManager : Singleton<UIManager>
{
    public PlayerUI playerUI;
    public TMP_Text InfoText;
    Coroutine stagetext;
    public GameObject DontTouch;
    public GameObject ESCWindow;
    [SerializeField] bool esccheck = false;

    public GameObject Inventory;
    public GameObject Equipment;
    public GameObject Tester;

    public GameObject PlayerUI;

    public GameObject Get_Item_UI;

    public GameObject TalkBox;
    public GameObject talkbuttons;
    public GameObject YesNobuttons;
    [SerializeField] Sprite[] talker;
    [SerializeField] Image talkerset;
    [SerializeField] NPC npc;
    public TMP_Text Talk;
    [SerializeField] string[] talks;
    public int talkNum = 0;
    //bool talkchk = false;

    public GameObject SaveWindow;
    public GameObject SettingWindow;
    public GameObject SaveCheck;
    public GameObject ExitCheck;
    public GameObject NoticeWindow;
    public GameObject DropItemWindow;
    public SaveSlot[] slots;


    UI_Default inventory;
    UI_Default equipment;
    UI_Default tester;
    UI_Default shop;

    UI_SetLast_SetActive settingWindow;
    UI_SetLast_SetActive savewindow;
    UI_SetLast_SetActive savecheckwindow;
    UI_SetLast_SetActive exitcheckwindow;
    UI_SetLast_SetActive noticewindow;
    UI_SetLast_SetActive dropitemwindow;

    bool DontTouchCheck;

    PlayerUI _playerUI;

    public GameObject BossHp;
    public Slider BossHPbar;
    public TMP_Text BossNow;
    public TMP_Text BossMax;

    public TMP_Text Timetext;

    public TMP_Text Info;

    public Canvas UI_Player;
    public Canvas UI_Change;

    public Transform MonsterHp_tr;
    public Transform DamageText_tr;

    public GameObject Shop;
    bool shopchk = false;

    public GameObject Hpbar;
    public GameObject DamageText;

    

    public Queue<HPBar> hpbars = new Queue<HPBar>();
    public Queue<DamageText> DamageTexts = new Queue<DamageText>();

    public LinkedList<UI_Default> items = new LinkedList<UI_Default>(); 
    [SerializeField] int count;


    public int Gold;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i< 20;i++)
        {
            GameObject obj = Instantiate(Hpbar, MonsterHp_tr) as GameObject;
            HPBar hpbar = obj.GetComponent<HPBar>();
            hpbar.gameObject.SetActive(false);
            hpbars.Enqueue(hpbar);

            GameObject obj2 = Instantiate(DamageText, DamageText_tr) as GameObject;
            DamageText dtext = obj2.GetComponent<DamageText>();
            dtext.text = dtext.GetComponent<TMP_Text>();
            dtext.gameObject.SetActive(false);
            DamageTexts.Enqueue(dtext);
        }
        GameManager.Instance.GoldSet();
        inventory = Inventory.GetComponent<UI_Default>();
        equipment = Equipment.GetComponent<UI_Default>();
        tester = Tester.GetComponent<UI_Default>();
        shop = Shop.GetComponent<UI_Default>();
        _playerUI = PlayerUI.GetComponent<PlayerUI>();

        settingWindow = SettingWindow.GetComponent<UI_SetLast_SetActive>();
        savewindow = SaveWindow.GetComponent<UI_SetLast_SetActive>();
        savecheckwindow = SaveCheck.GetComponent<UI_SetLast_SetActive>();
        exitcheckwindow = ExitCheck.GetComponent<UI_SetLast_SetActive>();
        noticewindow = NoticeWindow.GetComponent<UI_SetLast_SetActive>();
        dropitemwindow = DropItemWindow.GetComponent<UI_SetLast_SetActive>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            Hot_Key();
        }
        count= items.Count;
    }

    public void Hot_Key()
    {
        DontTouchCheck = savewindow.OC || settingWindow.OC || savecheckwindow.OC || exitcheckwindow.OC || noticewindow.OC || dropitemwindow.OC || npc.talkchk;
        if (DontTouchCheck) return;
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventory.UI_OC();
            UIOC(inventory);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            equipment.UI_OC();
            UIOC(equipment);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            tester.UI_OC();
            UIOC(tester);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(items.Count > 0)
            {
                items.First.Value.UI_OC();
                return;
            }
            ESCWindowOC();
        }
    }

    public void UIOC(UI_Default ui)
    {
        if (ui.OC != false)
        {
            if (items.Find(ui) == null)
            {
                items.AddFirst(ui);
            }
        }
        else
        {
            if (items.Find(ui) != null)
            {
                items.Remove(items.Find(ui));
            }
            _playerUI.tooltip.InActive();
        }
    }

    public void Gold_Up()
    {
        Gold += 1000000;
        GameManager.Instance.Gold = Gold;
    }
    public void Gold_Down()
    {
        Gold -= 1000000;
        GameManager.Instance.Gold = Gold;
    }
    public void ESCWindowOC()
    {
        esccheck = !esccheck;
        if (esccheck)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        
        DontTouch.SetActive(esccheck);
        ESCWindow.SetActive(esccheck);
    }
    public void TalkboxClose()
    {
        DontTouch.SetActive(false);
        npc.talkchk = false;
        TalkBox.SetActive(false);
        talkbuttons.SetActive(false);
        YesNobuttons.SetActive(false);
        npc.talk_num = 0;
        npc.buttonOnOff = 0;
    }

    public void ShopOC()
    {
        shop.UI_OC();
        if (npc.talkchk) TalkBoxOC(!shop.OC);
    }

    public void TalkBoxOC(bool chk)
    {
        TalkBox.SetActive(chk);
    }

   
    public void StartStage(string stage)
    {
        if (stagetext != null)
        {
            StopCoroutine(stagetext);
            stagetext = null;
        }
        stagetext = StartCoroutine(StageText(stage));
    }

    public IEnumerator StageText(string info)
    {
        Info.gameObject.SetActive(true);
        Info.text = info;
        Info.color = new Color(Info.color.r, Info.color.g, Info.color.b, 0);
        while (Info.color.a < 1.0f)
        {
            Info.color = new Color(Info.color.r, Info.color.g, Info.color.b, Info.color.a + 0.01f);
            yield return null;
        }
        while (Info.color.a > 0.0f)
        {
            Info.color = new Color(Info.color.r, Info.color.g, Info.color.b, Info.color.a - 0.01f);
            yield return null;
        }
        Info.gameObject.SetActive(false);
    }
    public IEnumerator NPCTalk(TextAsset text, int i)
    {
        Talk.text = null;
        StringReader reader = new StringReader(text.text);
        string talk = reader.ReadLine();
        talks = talk.Split('/');

        foreach (char letter in talks[talks.Length > talkNum ? talkNum++ : talks.Length - 1].ToCharArray())
        {
            Talk.text += letter;
            yield return new WaitForSeconds(0.1f);
        }
        if (talks.Length <= talkNum)
        {
            switch (i)
            {
                case 0:
                    talkbuttons.SetActive(true);
                    break;
                case 1:
                    YesNobuttons.SetActive(true);
                    break;
                default:
                    TalkboxClose();
                    break;
            }
            talkNum = 0;
        }
    }

    public void Get_HpBar_DText(Monster mons) // 
    {
        HPBar _hpbar;
        DamageText damage;
        if (hpbars.Count != 0)
        {
           _hpbar = hpbars.Dequeue();
        }
        else
        {
            GameObject obj = Instantiate(Hpbar, MonsterHp_tr) as GameObject;
            _hpbar = obj.GetComponent<HPBar>();

        }
        _hpbar.gameObject.SetActive(true);
        if (DamageTexts.Count != 0)
        {
            damage = DamageTexts.Dequeue();
        }
        else
        {
            GameObject obj = Instantiate(DamageText, DamageText_tr) as GameObject;
            damage = obj.GetComponent<DamageText>();
        }
        mons.damagetext = damage;
        mons.hpbar = _hpbar;
        _hpbar.myTarget = mons.transform;
        damage.myTarget = mons.transform;
    }

    public void ReleaseObject_HpBar_DText(HPBar hpbar, DamageText text)
    {
        hpbar.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
        hpbar.StopAllCoroutines();
        text.StopAllCoroutines();
        hpbar.myTarget = null;
        hpbars.Enqueue(hpbar);
        DamageTexts.Enqueue(text);
    }
    public void DontTouchOC(bool chk)
    {
        if (DontTouchCheck) return;
        DontTouch.transform.SetAsLastSibling();
        DontTouch.SetActive(chk);
    }

    public void SettingWindowOC()
    {
        settingWindow.UI_SetLast_OC(true);
    }
    public void SaveWindowOC()
    {
        savewindow.UI_SetLast_OC(true);
    }
    public void SaveCheckOC()
    {
        savecheckwindow.UI_SetLast_OC(true);
    }
    public void ExitCheckOC()
    {
        exitcheckwindow.UI_SetLast_OC(true);
    }

    public void NoticeOC()
    {
        noticewindow.UI_SetLast_OC(false);
    }

    public void DropItemOC()
    {
        dropitemwindow.UI_SetLast_OC(false);
    }
}
