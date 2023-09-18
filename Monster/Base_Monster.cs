using DTT.AreaOfEffectRegions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Base_Monster : BattleSystem
{
    public STAGE mystage;//=
    protected Color orgColor = Color.white;//=
    protected Vector3 startPos = Vector3.zero;//=
    public event UnityAction<Base_Monster> DeathAlarm;//=
    public List<DeBuff> debufList = new List<DeBuff>();//=
    [SerializeField] protected  TMP_Text damage_text;//=
    public Transform Player;//=


    [SerializeField]protected CharacterStat orgstat;//=

    [field: SerializeField]//=
    public Transform AttackPoint
    {
        get;
        private set;
    }
    [Serializable]
    public struct DeBuff
    {
        public enum Type
        {
            Slow, Restraint, Stun
        }
        public Type type;
        public float value;
        public float keepTime;
    }
    protected bool stunchk = false;
    public enum STATE
    {
        Create, Idle, Stun, Roaming, Battle, Dead, reCreate
    }

    public STATE myState = STATE.Create;

    protected virtual void ChangeState(STATE s)//w
    {
        if (myState == s) return;
        myState = s;
        switch (myState)
        {
            case STATE.reCreate:
                break;
            case STATE.Create:
                break;
            case STATE.Idle:
                break;
            case STATE.Stun:
                break;
            case STATE.Roaming:
                break;
            case STATE.Battle:
                break;
            case STATE.Dead:
                break;
        }
    }

    protected virtual void StateProcess()
    {
        switch (myState)
        {
            case STATE.reCreate:
                break;
            case STATE.Create:
                break;
            case STATE.Idle:
                break;
            case STATE.Stun:
                break;
            case STATE.Roaming:
                break;
            case STATE.Battle:
                break;
            case STATE.Dead:
                break;
        }
    }
    //=

    // Start is called before the first frame update
    void Start()//=
    {
        orgstat = myStat;
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()//=
    {
        StateProcess();
        debuff();
    }

    public void debuff()//=
    {
        for (int i = 0; i < debufList.Count;)
        {
            DeBuff temp = debufList[i];
            temp.keepTime -= Time.deltaTime;
            if (temp.keepTime <= 0.0f)
            {
                switch (temp.type)
                {
                    case DeBuff.Type.Slow:
                        modifyMoveSpeed /= temp.value;
                        break;
                    case DeBuff.Type.Stun:
                        ChangeState(STATE.Idle);
                        modifyMoveSpeed = 1.0f;
                        break;
                    case DeBuff.Type.Restraint:
                        modifyMoveSpeed = 1.0f;
                        break;
                }
                debufList.RemoveAt(i);
                continue;
            }
            debufList[i] = temp;
            ++i;
        }
    }
    public void AddDebuff(DeBuff.Type type, float value, float keep, STATE state)
    {
        for (int i = 0; i < debufList.Count; ++i)
        {
            if (type == debufList[i].type)
            {
                DeBuff temp = debufList[i];
                temp.keepTime = keep;
                debufList[i] = temp;
                return;
            }
        }

        DeBuff def = new DeBuff();
        def.type = type;
        def.value = value;
        def.keepTime = keep;

        switch (type)
        {
            case DeBuff.Type.Slow:
                modifyMoveSpeed *= value;
                break;
            case DeBuff.Type.Restraint:
                AttackTarget(myTarget);
                modifyMoveSpeed = 0.0f;
                break;
            case DeBuff.Type.Stun:
                AttackTarget(myTarget);
                StartCoroutine(DelayRoaming(keep, state));
                modifyMoveSpeed = 0.0f;
                break;
        }

        Color color = type == DeBuff.Type.Slow ? Color.cyan : type == DeBuff.Type.Restraint ? Color.green : Color.gray;
        StartCoroutine(DamagingColor(color, keep));
        debufList.Add(def);
    }
    //=
    protected IEnumerator DelayRoaming(float t, STATE state) //=
    {
        yield return new WaitForSeconds(t);
        ChangeState(state);
    }

    public void AttackTarget()//=
    {
        if (myTarget != null && Vector3.Distance(myTarget.position, transform.position) <= myStat.AttackRange + 0.5f)
        {
            myTarget.GetComponent<IBattle>()?.OnDamage(myStat.AP, gameObject); // 플레이어가 공격을 반사하는 스킬이 있을 시 타겟을 넘겨줌
        }
    }


    protected IEnumerator DamagingColor(Color color, float t)//=
    {
        GetComponentInChildren<Renderer>().material.color = color;
        yield return new WaitForSeconds(t);
        GetComponentInChildren<Renderer>().material.color = orgColor;
    }

    public override bool IsLive()//=
    {
        return myState != STATE.Dead;
    }


    protected IEnumerator DisApearing(float d, float t)//=
    {
        yield return new WaitForSeconds(t);
        float dist = d;
        while (dist > 0.0f)
        {
            float delta = 0.5f * Time.deltaTime;
            if (delta > dist)
            {
                delta = dist;
            }
            dist -= delta;
            transform.Translate(Vector3.down * delta, Space.World);
            yield return null;
        }
        DeathAlarm?.Invoke(this);
        mystage.ReleaseMonster(this.gameObject);
    }

    

    public void Change_Mons_State(STATE state) //=
    {
        ChangeState(state);
    }

    public virtual void ResetMonster()//w
    {
    }
}
