using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using System;

public class Monster : Base_Monster
{
    public GameObject AI_Per;
   
    public Transform hp;
    //public Slider Hpbar;
    public HPBar hpbar;
    public DamageText damagetext;
    
    protected override void ChangeState(STATE s)
    {
        if (myState == s) return;
        myState = s;
        switch(myState)
        {
            case STATE.reCreate:
                if (myStat.HP != myStat.MaxHP)
                {
                    myStat = orgstat;
                    GetComponent<CapsuleCollider>().enabled = true;
                    AI_Per.SetActive(true);
                }
                ChangeState(STATE.Idle);
                break;
            case STATE.Create:
                break;
            case STATE.Idle:
                if (myTarget != null) ChangeState(STATE.Battle);
                else
                    StartCoroutine(DelayRoaming(2.0f));
                break;
            case STATE.Stun:
                stunchk = true;
                break;
            case STATE.Roaming:
                Vector3 pos = Vector3.zero;
                pos.x = UnityEngine.Random.Range(-3.0f, 3.0f);
                pos.z = UnityEngine.Random.Range(-3.0f, 3.0f);
                pos = startPos + pos;
                MoveToPosition(pos,() => ChangeState(STATE.Idle),true, false);
                break;
            case STATE.Battle:
                AttackTarget(myTarget);
                break;
            case STATE.Dead:
                StopAllCoroutines();
                myAnim.SetTrigger("Dead");
                myTarget = null;
                AI_Per.SetActive(false);
                UIManager.Instance.ReleaseObject_HpBar_DText(hpbar, damagetext);
                GetComponent<CapsuleCollider>().enabled = false;
                Player.GetComponent<RPGPlayer>().DeadMessage(transform, myStat.Exp); // 플레이어
                StartCoroutine(DisApearing(2.0f,4.0f));
                break;
        }
    }


    protected override void StateProcess()
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

    IEnumerator DelayRoaming(float t)
    {
        yield return new WaitForSeconds(t);
        ChangeState(STATE.Roaming);
    }
   
   
    public void FindTarget(Transform target)
    {
        if (stunchk) return;
        if (myState == STATE.Dead) return;
        Player = target;
        myTarget = target; // 플레이어
        StopAllCoroutines();
        ChangeState(STATE.Battle);
    }

    public void LostTarget()
    {
        if (myState == STATE.Dead) return;
        myTarget = null;
        StopAllCoroutines();
        myAnim.SetBool("IsMoving", false);
        ChangeState(STATE.Idle);
    }
    

    public override void OnDamage(float dmg, GameObject attacker)
    {
        if (myTarget == null)
        {
            Player = attacker.transform;
            myTarget = attacker.transform;
        }
        AttackTarget(myTarget);
        myStat.HP -= Mathf.Clamp(dmg - myStat.DP, 0, dmg);
        damagetext.damage_text(dmg - myStat.DP);
        if (Mathf.Approximately(myStat.HP, 0.0f))
        {
            ChangeState(STATE.Dead);
            damagetext.gameObject.SetActive(false);
            hpbar.gameObject.SetActive(false);
        }
        else
        {
            if (!myAnim.GetBool("IsAttacking"))
            {
                myAnim.SetTrigger("Damage");
            }
            StartCoroutine(DamagingColor(Color.red, 0.5f));
        }
    }
   

    public override void DeadMessage(Transform tr, float exp)
    {
        if(tr == myTarget)
        {
            LostTarget();
        }
    }    


    public void HPbar_text_Release()
    {
        UIManager.Instance.ReleaseObject_HpBar_DText(hpbar, damagetext);
    }
    public override void ResetMonster()//w
    {
        UIManager.Instance.Get_HpBar_DText(this);
        hpbar.myTarget = hp;
        hpbar.HPbar_set();
        myStat.changeHp = (float v) => hpbar.myBar.value = v;
        ChangeState(STATE.Idle);
    }


}
