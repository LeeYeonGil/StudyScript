using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class Chest : MonoBehaviour
{
    [SerializeField]Animator anim;
    public bool isPlayerEnter;
    bool OC_Check;
    bool creat_chk = false;
    int set_grade;
    GameObject Set_eff;
    [SerializeField] TMP_Text text;
    public bool get_chk=false;
    public bool startchest = false;

    public enum STATE
    {
        Create, Active, Idle, inActive
    }

    public STATE myState = STATE.Create;
    public Item item;
    public List<Item> items = new List<Item>();
    int item_count = 0;

    public GameObject[] Grade_eff;

    public Item_Data[] Normal_Data;
    public Item_Data[] Magic_Data;
    public Item_Data[] Unique_Data;
    public Item_Data[] Epic_Data;
    public Item_Data[] Legend_Data;
    public Item_Data[] Potion_Data;
    [SerializeField] Get_Item_List get_Item_List; // 획득한 아이템

    Coroutine ActiveChest;

    void Awake()
    {
        anim = GetComponent<Animator>();
        isPlayerEnter = false;
        ChestReset();
        get_Item_List = UIManager.Instance.Get_Item_UI.GetComponent<Get_Item_List>();
        if (startchest && !GameManager.Instance.NewGame) ChangeState(STATE.inActive);
    }


    public void ChestReset()
    {
        get_chk = false;
        OC_Check = false;
        creat_chk = false;
        isPlayerEnter = false;
        set_grade = startchest ? 0 : Random_Grade();
        Set_eff = Grade_eff[set_grade];
    }

    public void ChangeState(STATE s)
    {
        if (myState == s) return;
        myState = s;
        switch (myState)
        {
            case STATE.Create:
                break;
            case STATE.Active:
                if(ActiveChest != null)
                {
                    StopCoroutine(ActiveChest);
                    ActiveChest = null;
                }
                ActiveChest = StartCoroutine(Chest_Active());
                break;
            case STATE.Idle:
                //Invoke("InActive_GetList", 1.2f);
                anim.SetBool("Chest_OC", false);
                StopCoroutine(Open_Get_List());
                OC_Check = false;
                Set_eff.SetActive(OC_Check);
                break;
            case STATE.inActive:
                anim.SetBool("Chest_OC", false);
                StopAllCoroutines();
                gameObject.SetActive(false);
                ChangeState(STATE.Idle);
                break;
        }
    }

    void InActive_GetList()
    {
        UIManager.Instance.Get_Item_UI.SetActive(false);
    }
    void StateProcess()
    {
        switch (myState)
        {
            case STATE.Create:
                if(isPlayerEnter)
                {
                    ChangeState(STATE.Active);
                }
                break;
            case STATE.Active:
                if (!isPlayerEnter)
                {
                    if (get_chk)
                    {
                        ChangeState(STATE.inActive);
                    }
                    else
                    {
                        ChangeState(STATE.Idle);
                    }
                }
                if(get_chk)
                {
                    ChangeState(STATE.inActive);
                }
                break;
            case STATE.Idle:
                if (isPlayerEnter)
                {
                    ChangeState(STATE.Active);
                }
                break;
            case STATE.inActive:
                break;
        }
    }


    void Update()
    {
        StateProcess();
    }

    IEnumerator Chest_Active()
    {
        while(true)
        {
            if (isPlayerEnter)
            {
                text.gameObject.SetActive(true);
                if (Input.GetKeyDown(KeyCode.F))
                {
                    if (!OC_Check)
                    {
                        //열었을때 랜덤 아이템 생성
                        anim.SetBool("Chest_OC", true);
                        StartCoroutine(Open_Get_List());
                        OC_Check = true;
                    }
                    else
                    {
                        StopCoroutine(Open_Get_List());
                        anim.SetBool("Chest_OC", false);
                        UIManager.Instance.Get_Item_UI.SetActive(false);
                        UIManager.Instance.Get_Item_UI.GetComponent<Get_Item_List>().DontMove.SetActive(false);
                        OC_Check = false;
                    }
                    Set_eff.SetActive(OC_Check);
                }
            }
            else
            {
                text.gameObject.SetActive(false);
            }
            yield return null;

        }
    }

    IEnumerator Open_Get_List()
    {
        WaitForSeconds sec = new WaitForSeconds(1.3f);
        yield return sec;
        UIManager.Instance.Get_Item_UI.SetActive(true);
        if (!creat_chk) // 아이템을 생성했는지
        {
            creat_chk = true;
            if (startchest)
            {
                Item_Create(set_grade, 0); // 처음 열었을때만 적용
                Item_Create(set_grade, 1); // 처음 열었을때만 적용
                Item_Create(set_grade, 2); // 처음 열었을때만 적용
                Item_Create(set_grade, 3); // 처음 열었을때만 적용
            }
            else
            {
                Item_Create(set_grade, 0); // 처음 열었을때만 적용
            }
        }
        else if(!get_chk) // 전부 다 아이템을 가져갔는지
        {
            Chest_Get_Item();
        }
    }
    public void Chestin_active()
    {
        ChangeState(STATE.inActive);
    }



    public void Chest_Get_Item()
    {
        get_Item_List.itemData.Clear();
        foreach (var item in items)
        {
            get_Item_List.itemData.Add(item);
        }
        get_Item_List.Get_Item(this);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6) // player
        {
            isPlayerEnter = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            isPlayerEnter = false;
        }
    }

    void Item_Create(int grade, int StartChestNum)
    {
        // 무기, 방어구 스크립트에 등급 전달
        // 무기 방어구 스크립트에서 전달된 등급을 통해 스탯 설정
        switch (grade)
        {
            case 0:
                items.Add(Stat_Set(Normal_Data[startchest ? StartChestNum : Random.Range(0, Normal_Data.Length)]));
                break;
            case 1:
                items.Add(Stat_Set(Magic_Data[Random.Range(0, Magic_Data.Length)]));
                break;
            case 2:
                items.Add(Stat_Set(Unique_Data[Random.Range(0, Unique_Data.Length)]));
                break;
            case 3:
                items.Add(Stat_Set(Epic_Data[Random.Range(0, Epic_Data.Length)]));
                break;
            case 4:
                items.Add(Stat_Set(Legend_Data[Random.Range(0, Legend_Data.Length)]));
                break;
        }
        if (get_Item_List.itemData != null) // 새로운 상자
        {
            get_Item_List.itemData.Clear();
        }
        foreach (var item in items)
        {
            get_Item_List.itemData.Add(item);
        }
        get_Item_List.Get_Item(this);

    }

    public int Random_Grade()
    {
        int rand = Random.Range(0, 101);
        int[] _grade = { 60, 25, 11, 3, 1 }; // 60% 노말 / 25% 레어 / 11% 유니크 / 3% 에픽 / 1% 레전드 
        int totalNum = 0;
        int gradeNum = 0;

        for(int i = 0; i < _grade.Length; i++)
        {
            totalNum += _grade[i];
            if(totalNum >= rand)
            {
                gradeNum = i;
                break;
            }
        }
        return gradeNum;
    }

    public Item Stat_Set(Item_Data item)
    {
        Item _item = new Item();
        switch (item.item_Grade)
        {
            case ITEMGRADE.Normal:
                _item = Type_set(item, 1.0f, 5.0f);
                break;
            case ITEMGRADE.Magic:
                _item = Type_set(item, 5.0f, 10.0f);
                break;
            case ITEMGRADE.Unique:
                _item = Type_set(item, 10.0f, 20.0f);
                break;
            case ITEMGRADE.Epic:
                _item = Type_set(item, 20.0f, 30.0f);
                break;
            case ITEMGRADE.Legend:
                _item = Type_set(item, 30.0f, 50.0f);
                break;
        }
        _item.item_data = item;
        _item.Count = 1;
        return _item;
    }

    public Item Type_set(Item_Data Item_data, float min, float max)
    {
        Item _item = new Item();
        switch (Item_data.item_Type)
        {
            case Item_Type.Weapon: // 공격력, 공속
                _item.AttackPoint = Mathf.Round((Random.Range(min, max) * 10.0f)) * 0.1f;
                _item.DeffencePoint = 0.0f;
                _item.AttackSpeed = Mathf.Round((Random.Range(min / 10, max / 10) * 10.0f)) * 0.1f;
                _item.MoveSpeed = 0.0f;
                break;
            case Item_Type.Helmet: // 방어력, HP, MP
                _item.AttackPoint = 0.0f;
                _item.DeffencePoint = Mathf.Round((Random.Range(min / 2, max / 2) * 10.0f)) * 0.1f;
                _item.AttackSpeed = 0.0f;
                _item.MoveSpeed = 0.0f;
                break;
            case Item_Type.Armor: // 방어력, HP, MP
                _item.AttackPoint = 0.0f;
                _item.DeffencePoint = Mathf.Round((Random.Range(min / 2, max / 2) * 10.0f)) * 0.1f;
                _item.AttackSpeed = 0.0f;
                _item.MoveSpeed = 0.0f;
                break;
            case Item_Type.Shoes: // 이속, 공속
                _item.AttackPoint = 0.0f;
                _item.DeffencePoint = 0.0f;
                _item.AttackSpeed = Mathf.Round((Random.Range(min / 10, max / 10) * 10.0f)) * 0.1f;
                _item.MoveSpeed = Mathf.Round((Random.Range(min / 10, max / 10) * 10.0f)) * 0.1f;
                break;
        }
        return _item;
    }
}
