using DTT.AreaOfEffectRegions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class RPGPlayer : BattleSystem
{
    public AnimEvent _myAnimEvent;
    public MySkill _mySkill;
    public GameObject _player;
    public AttackRange attackRange;
    public GameObject Skill_Arc;
    public GameObject Skill_Circle;

    public GameObject mousePoint;

    public GameObject LeveLUpEff;

    public Transform talker;


    public bool RangeActive = false;
    public bool talkchk = false;

    Slot_Item Helmet_skill;
    Slot_Item Armor_skill;
    Slot_Item Weapon_skill;

    public bool WeaponCasting = true;
    public bool ArmorCasting = true;
    public bool HelmetCasting = true;

    public PotionSlot Slot1;
    public PotionSlot Slot2;

    Coroutine Info;
    Coroutine AutoT;
    Coroutine mouseP;
    Coroutine AutoTarget;

    public CharacterStat orgstat;
    public Equip_Stat_set equip_Stat;
    
    public enum STATE
    {
        Create, Play, Death
    }
    public STATE myState = STATE.Create;

    public LayerMask pickMask = default;
    public LayerMask enemyMask = default;


    private void Awake()
    {
        Helmet_skill = UIManager.Instance.playerUI.equipment.HelmetSlot.GetComponent<Slot_Item>();
        Armor_skill = UIManager.Instance.playerUI.equipment.ArmorSlot.GetComponent<Slot_Item>();
        Weapon_skill = UIManager.Instance.playerUI.equipment.WeaponSlot.GetComponent<Slot_Item>();
        _myAnimEvent.Skill_Weapon.AddListener(_mySkill.Weapon_DefaultSkill);
        _myAnimEvent.Skill_Helmet.AddListener(_mySkill.Helmet_AroundSkill);
        _myAnimEvent.Skill_Armor.AddListener(_mySkill.Armor_RangeSkill);
        orgstat = myStat;
    }

    public void ChangeState(STATE s)
    {
        if (myState == s) return;
        myState = s;
        switch(myState)
        {
            case STATE.Create:
                myAnim.SetTrigger("Respawn");
                Invoke("Respawn", 8.0f);
                break;
            case STATE.Play:
                StartCoroutine(Autotherapy());
                break;
            case STATE.Death:
                StopAllCoroutines();
                myAnim.SetTrigger("Dead");
                Invoke("DeadAlram", 3.0f);
                foreach (IBattle ib in myAttackers) 
                {
                    ib.DeadMessage(transform, 0.0f);
                }
                break;
        }
    }
    void StateProcess()
    {
        switch (myState)
        {
            case STATE.Create:
                break;
            case STATE.Play:
                _player.transform.position = transform.position;
                myAnim.SetFloat("AttackSpeed", myStat.AttackSpeed);
                if (!EventSystem.current.IsPointerOverGameObject() &&
                    Input.GetMouseButtonDown(1))
                {
                    _player.transform.localEulerAngles = Vector3.zero;
                    MouseToAction(false);
                    if (AutoTarget == null) return;
                    StopCoroutine(AutoTarget);
                }
                if (Input.GetKey(KeyCode.Alpha1)) // 누른 위치에 좌표를 구해서 그 위치에 소환
                {
                    if(Helmet_skill._Item.item_data != null)
                    {
                        if (Helmet_skill.gameObject.GetComponent<Around_Skill>().Skilleff.GetComponent<ArcCheck>().check)
                        {
                            Skill_Arc.SetActive(true);
                            Skill_Arc.GetComponent<SKill_Indecator>().arcCheck = true;
                        }
                        else
                        {
                            Skill_Circle.SetActive(true);
                            Skill_Circle.GetComponent<SKill_Indecator>().stay = true;
                        }
                    }
                    UnityEngine.Cursor.visible = false;
                    if (Input.GetMouseButtonUp(0))
                    {
                        if(AutoTarget != null) StopCoroutine(AutoTarget);
                        if (Helmet_skill._Item.item_data != null)
                        {
                            if (HelmetCasting)
                            {
                                Skill_Command(Helmet_skill, _mySkill.skill_Manage.Skill_Helmet_chk);
                            }
                            else
                            {
                                UIManager.Instance.InfoText.text = "스킬을 시전 중입니다.";
                            }
                        }
                        else
                        {
                            if (Info != null)
                            {
                                StopCoroutine(Info);
                                Info = null;
                            }
                            Info = StartCoroutine(Information(Helmet_skill, 0));
                        }
                    }
                }
                else if (Input.GetKey(KeyCode.Alpha2)) // 누른 위치에 좌표를 구해서 그 위치에 소환
                {
                    UnityEngine.Cursor.visible = false;
                    if (Armor_skill._Item.item_data != null)
                    {
                        Skill_Circle.SetActive(true);
                        Skill_Circle.GetComponent<SKill_Indecator>().arcCheck = false;
                        Skill_Circle.GetComponent<SKill_Indecator>().stay = false;
                    }
                    if (Input.GetMouseButtonUp(0))
                    {
                        if (AutoTarget != null) StopCoroutine(AutoTarget);
                        if (Armor_skill._Item.item_data != null)
                        {
                            if (ArmorCasting)
                            {
                                Skill_Command(Armor_skill, _mySkill.skill_Manage.Skill_Armor_chk);
                            }
                            else
                            {
                                UIManager.Instance.InfoText.text = "스킬을 시전 중입니다.";
                            }
                        }
                        else
                        {
                            if (Info != null)
                            {
                                StopCoroutine(Info);
                                Info = null;
                            }
                            Info = StartCoroutine(Information(Armor_skill, 0));
                        }
                    }
                }
                else
                {
                    Skill_Arc.SetActive(false);
                    Skill_Arc.GetComponent<SKill_Indecator>().arcCheck = false;
                    Skill_Circle.SetActive(false);
                    UnityEngine.Cursor.visible = true;
                }
                if(Input.GetKey(KeyCode.A))
                {
                    attackRange.Projector.enabled = true;
                    if (Input.GetMouseButtonUp(0))
                    {
                        AutoTargetSet();
                    }
                    RangeActive = true;
                }
                else
                {
                    if (RangeActive)
                    {
                        attackRange.Projector.enabled = false;
                        RangeActive = false;
                    }
                }
                if(Input.GetKeyDown(KeyCode.F1))
                {
                    Slot1.UseItem();
                }
                if (Input.GetKeyDown(KeyCode.F2))
                {
                    Slot2.UseItem();
                }
                if (Input.GetKey(KeyCode.F))
                {
                    if(talkchk) MoveToPosition(talker.position, null, true, true);
                }
                break;
            case STATE.Death:
                break;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        ChangeState(STATE.Play);
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();      
    }    

    void AutoTargetSet()
    {
        if(AutoTarget != null)
        {
            StopCoroutine(AutoTarget);
            AutoTarget = null;
        }
        AutoTarget = StartCoroutine(autoTarget());
    }

    IEnumerator autoTarget()
    {
        bool auto = true;
        MouseToAction(false);
        while (auto) // 목표지점으로 도착할 때 까지
        {
            if (myTarget != null) // 타겟을 찾은 경우  attacktarget으로 변경;
            {
                AttackTarget(myTarget);
                auto = false;
            }
            yield return null;
        }
    }
   

    public void MouseToAction(bool skill)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        //레이어마스크에 해당하는 오브젝트가 선택 되었는지 확인 한다.
        if (Physics.Raycast(ray, out hit, 1000.0f, enemyMask))
        {
            myTarget = hit.transform;
            AttackTarget(myTarget);
        }
        else if (Physics.Raycast(ray, out hit, 1000.0f, pickMask))
        {
            if (!skill)
            {
                if (mouseP != null)
                {
                    StopCoroutine(mouseP);
                    mouseP = null;
                }
                mouseP = StartCoroutine(mousepointer(hit.point));
                MoveToPosition(hit.point, null, true, false);
            }
            else
            {
                MoveToPosition(hit.point, null, true, true);
            }
            myTarget = null;
        }
            
    }
    public override void OnDamage(float dmg, GameObject attacker)
    {
        if (attacker.GetComponent<BattleSystem>().myTarget == null)
        {
            attacker.GetComponent<BattleSystem>().myTarget = gameObject.transform;
        }
        myStat.HP -= Mathf.Clamp(dmg - myStat.DP, 0, dmg);
        if (Mathf.Approximately(myStat.HP, 0.0f))
        {
            ChangeState(STATE.Death);
        }
        else
        {
            myAnim.SetTrigger("Damage");
        }
    }    

    public override bool IsLive()
    {
        return !Mathf.Approximately(myStat.HP, 0.0f);
    }

    public override void DeadMessage(Transform tr, float exp) // 몬스터의 데드메시지
    {
        myStat.Exp += tr.GetComponent<Monster>().myStat.Exp;
        LevelUp();
        StopAllCoroutines();
        AutoT = StartCoroutine(Autotherapy());
    }

   

    IEnumerator Information(Slot_Item Item, int CaseNum)
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        float delayTime = 0.0f;
        string[] name = Item.name.Split('_');
        UIManager.Instance.InfoText.gameObject.SetActive(true);
        switch (CaseNum)
        {
            case 0: // 스킬 X 
                UIManager.Instance.InfoText.text = $"{name[1]} 스킬이 없습니다!";
                break;
            case 1: // 마나
                UIManager.Instance.InfoText.text = $"{name[1]} 스킬의 마나가 부족합니다!";
                break;
            case 2: // 쿨타임
                UIManager.Instance.InfoText.text = $"{name[1]} 스킬의 쿨타임중 입니다!";
                break;
        }
        while (delayTime < 1.0f) // Mathf.Approximately(myStat.HP, 0.0f) delayTime >= 0.0f
        {
            delayTime += 0.1f;
            yield return wait;
        }
        UIManager.Instance.InfoText.gameObject.SetActive(false);
        yield return null;
    }

    IEnumerator Autotherapy()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        while (true)
        {
            if (myStat.HP != myStat.MaxHP)
            {
                myStat.HP += myStat.MaxMP * 0.0001f;
            }
            if (myStat.MP != myStat.MaxMP)
            {
                myStat.MP += myStat.MaxMP * 0.0001f;
            }
            yield return wait;
        }
    }
    IEnumerator Skill_indecator(GameObject indecator, Vector3 mousePos)
    {
        indecator.SetActive(true);
        while (!myAnim.GetBool("IsSkill"))
        {

            yield return null;
        }
        indecator.SetActive(false);
    }

    IEnumerator mousepointer(Vector3 pos)
    {
        float i = 0.0f;
        mousePoint.SetActive(true);
        mousePoint.transform.position = pos;
        while(i < 1.0f)
        {
            i += 0.1f;
            yield return new WaitForSeconds(0.1f);
            mousePoint.SetActive(false);
        }
    }

    public void Skill_Command(Slot_Item slot, bool Cool)
    {
        bool Case_1 = (slot._Item.item_data.skill_Eff != null)? true : false;
        bool Case_2 = (slot._Item.item_data.skill_Cost < myStat.MP) ? true : false;
        bool Case_3 = Cool;
        if (Case_1 && Case_2 && Case_3)
        {
            string[] name = slot.name.Split('_');
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000.0f, pickMask))
            {
                gameObject.GetComponent<MySkill>().HitPoint = hit.point;
                Skill_Casting(hit.point, myStat.AttackRange, $"{name[1]}_Skill", false);
            }
                myStat.MP -= slot._Item.item_data.skill_Cost;
        }
        else
        {
            if (Info != null)
            {
                StopCoroutine(Info);
                Info = null;
            }
            if (!Case_1) // 쿨타임
            {
                Info = StartCoroutine(Information(slot, 0));
            }
            else if(!Case_2) // 마나
            {
                Info = StartCoroutine(Information(slot, 1));
            }
            else // 스킬 X
            {
                Info = StartCoroutine(Information(slot, 2));
            }
        }
        
    }

    public void LevelUp()
    {
        if (myStat.Exp >= myStat.MaxExp)
        {
            LeveLUpEff.SetActive(true);
            float remainExp = 0.0f;
            myStat.Lv += 1;
            remainExp = myStat.Exp - myStat.MaxExp;
            myStat.Exp = 0.0f;
            myStat.Exp += remainExp;
            myStat.MaxExp = myStat.Lv * myStat.MaxExp;
            myStat.AP += 2.0f;
            myStat.DP += 1.0f;
            myStat.AttackDelay += 0.1f;
            myStat.MaxHP += 100.0f;
            myStat.MaxMP += 50.0f;
            _mySkill.Damage = myStat.AP;

            orgstat = myStat;
            orgstat.AP -= equip_Stat.AP;
            orgstat.DP -= equip_Stat.DP;
            orgstat.AttackSpeed -= equip_Stat.AS;
            orgstat.MoveSpeed -= equip_Stat.MS;
            UIManager.Instance.Equipment.GetComponent<Equipment>().Orgstat_set();
            Invoke("LeveL_Eff_Hide", 3.0f);
        }
    }
    public void LeveL_Eff_Hide()
    {
        LeveLUpEff.SetActive(false);
    }

    public void EXP_UP()
    {
        myStat.Exp += 10.0f;
        LevelUp();
    }

    public void Respawn()
    {
        _player.transform.localEulerAngles = Vector3.zero;
        ChangeState(STATE.Play);
    }

    public void DeadAlram()
    {
        GameManager.Instance.PlayerRespawn.SetActive(true);
    }

    public void ChangeDead()
    {
        ChangeState(STATE.Death);
    }

   
}
