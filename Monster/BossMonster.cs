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

public class BossMonster : Base_Monster
{
    public Transform myHeadTop;
    public LayerMask enemyMask;

    public CapsuleCollider body;


    public GameObject Shild;
    public GameObject attack1;

    public bool level2 = false;

    public ArcRegion attack1range;
    public LineRegion attack2range;
    public Transform attack2tr;
    Coroutine corTarget;


    protected override void ChangeState(STATE s)
    {
        if (myState == s) return;
        myState = s;
        switch (myState)
        {
            case STATE.reCreate:
                myTarget = Player;
                if (myStat.HP != myStat.MaxHP)
                {
                    myStat = orgstat;
                    body.enabled = true;
                }
                StartCoroutine(BossSpawn());
                break;
            case STATE.Create:
                break;
            case STATE.Idle:
                if (!level2 && myStat.MaxHP / 2 >= myStat.HP) 
                {
                    myStat.MoveSpeed *= 1.5f;
                    myStat.AttackDelay *= 0.5f;
                    level2 = true; 
                }
                DelayRoaming(4.0f,STATE.Battle);
                break;
            case STATE.Stun:
                break;
            case STATE.Roaming:
                break;
            case STATE.Battle:
                if(myTarget == null) myTarget = Player;
                ChasingTarget(myTarget);
                break;
            case STATE.Dead:
                StopAllCoroutines();
                myAnim.SetTrigger("Dead");
                myTarget = null;
                body.enabled = false;
                StartCoroutine(DisApearing(2.0f, 4.0f));
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

   

    IEnumerator BossSpawn()
    {
        yield return new WaitForSeconds(1.0f);
        myAnim.SetTrigger("Spawn");
        yield return new WaitForSeconds(3.0f);
        CamManager.Instance.ChangeCam();
        yield return DelayRoaming(3.0f,STATE.Battle);
    }
    
    public void Attack_1()
    {
        GameObject obj = Instantiate(attack1, AttackPoint.position, AttackPoint.rotation) as GameObject;
        Hit_Skill _hit = obj.GetComponent<Hit_Skill>();
        _hit._Damage = myStat.AP;
        _hit.Caster = gameObject;
    }

    public void Attack_2()
    {
        Collider[] list = Physics.OverlapBox(attack2tr.position, new Vector3(2, 2, 5) * 0.5f, transform.rotation);
        foreach (Collider col in list)
        {
            if (col.gameObject.layer == 6)
            {
                col.GetComponent<IBattle>()?.OnDamage(myStat.AP * 1.5f, gameObject);
            }
        }
    }


    public override void OnDamage(float dmg, GameObject Attacker)
    {
        myStat.HP -= Mathf.Clamp(dmg - myStat.DP, 0, dmg);
        if (Mathf.Approximately(myStat.HP, 0.0f))
        {
            ChangeState(STATE.Dead);
        }
        else
        {
            if (!myAnim.GetBool("IsDefense"))
            {
                if (!myAnim.GetBool("IsAttacking")) myAnim.SetTrigger("Damage");
            }
            else
            {
                myAnim.SetTrigger("DefenseDamage");
            }
            StartCoroutine(DamagingColor(Color.red, 0.5f));
        }
    }

    IEnumerator Damage_Text(TMP_Text text, float dmg)
    {
        float i = 0.1f;
        while (i < 200.0f)
        {
            text.gameObject.SetActive(true);
            text.transform.position = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(0, i++);
            text.text = Mathf.Clamp(dmg - myStat.DP, 0, dmg).ToString("0.0");
            yield return null;
        }
        text.gameObject.SetActive(false);
    }



    public override void DeadMessage(Transform tr, float exp)
    {

    }

    IEnumerator Pattern()
    {
        yield return new WaitForSeconds(1.0f);
        int RandomPattern = UnityEngine.Random.Range(0, 4);
        switch (RandomPattern)
        {
            case 0:
                StartCoroutine(NormalAttack());
                break;
            case 1:
                StartCoroutine(Skill_1());
                break;
            case 2:
                StartCoroutine(Skill_2());
                break;
            case 3:
                StartCoroutine(Defense());
                break;
        }
    }
    void ChasingTarget(Transform target)
    {
        if(corTarget != null)
        {
            StopCoroutine(corTarget);
            corTarget = null;
        }
        corTarget = StartCoroutine(AttckingPlayer(target, myStat.AttackRange, myStat.AttackDelay / myStat.AttackSpeed));
    }

    IEnumerator AttckingPlayer(Transform target, float AttackRange, float AttackDelay)
    {
        float playTime = 0.0f;
        float delta = 0.0f;
        bool running = false;
        while (target != null)
        {
            if (!myAnim.GetBool("IsAttacking") && !myAnim.GetBool("IsDefense")) playTime += Time.deltaTime;
            //이동
            Vector3 dir = target.position - transform.position;
            float dist = dir.magnitude;
            dir.Normalize();
            if (dist > AttackRange)
            {
                if (playTime >= 10.0f)
                {
                    myAnim.SetBool("IsMoving", false);
                    myAnim.SetBool("IsRunning", true);
                    if (!running)
                    {
                        myStat.MoveSpeed *= 2.0f;
                        running = true;
                    }
                }
                else
                {
                    myAnim.SetBool("IsMoving", true);
                }
                delta = myStat.MoveSpeed * modifyMoveSpeed * Time.deltaTime;
                if (delta > dist)
                {
                    delta = dist;
                }
                if (!myAnim.GetBool("IsAttacking")&&!myAnim.GetBool("IsDefense"))
                {
                    transform.Translate(dir * delta, Space.World);
                }
            }
            else
            {
                if (playTime >= 10.0f)
                {
                    myAnim.SetBool("IsMoving", false);
                    myAnim.SetBool("IsRunning", false);
                    if (running)
                    {
                        myStat.MoveSpeed *= 0.5f;
                        running = false;
                    }
                }
                else
                {
                    myAnim.SetBool("IsMoving", false);
                }
                if (playTime >= AttackDelay)
                {
                    //공격
                    playTime = 0.0f;
                    yield return StartCoroutine(Pattern());
                }
            }
            //회전
            delta = myStat.RotSpeed * modifyMoveSpeed * Time.deltaTime;
            float Angle = Vector3.Angle(dir, transform.forward);
            float rotDir = 1.0f;
            if (Vector3.Dot(transform.right, dir) < 0.0f)
            {
                rotDir = -rotDir;
            }
            if (delta > Angle)
            {
                delta = Angle;
            }
            transform.Rotate(Vector3.up * delta * rotDir, Space.World);

            yield return null;
        }
        myAnim.SetBool("IsMoving", false);
    }


    IEnumerator NormalAttack()
    {
        myAnim.SetTrigger("Default");
        yield return new WaitForSeconds(level2 ? 1.5f : 2.5f);

        ChangeState(STATE.Idle);
    }

    IEnumerator Skill_1()
    {
        float time = 0.0f;
        myAnim.SetTrigger("Attack1Casting");
        attack1range.gameObject.SetActive(true);
        while (time <= 1.0f)
        {
            attack1range.FillProgress += level2 ? 0.2f : 0.1f;
            time += 0.1f;
            yield return new WaitForSeconds(level2 ? 0.1f : 0.2f);
        }
        myAnim.SetTrigger("Attack1CastEnd");
        attack1range.FillProgress = 0.0f;
        attack1range.gameObject.SetActive(false);
        yield return new WaitForSeconds(level2 ? 1.5f : 2.5f);

        ChangeState(STATE.Idle);
    }

    IEnumerator Skill_2()
    {
        float time = 0.0f;
        myAnim.SetTrigger("Attack2Casting");
        attack2range.gameObject.SetActive(true);
        while (time <= 1.0f)
        {
            attack2range.FillProgress += level2 ? 0.2f : 0.1f;
            time += 0.1f;
            yield return new WaitForSeconds(level2 ? 0.1f : 0.2f);
        }
        myAnim.SetTrigger("Attack2CastEnd");
        Attack_2();
        attack2range.FillProgress = 0.0f;
        attack2range.gameObject.SetActive(false);
        yield return new WaitForSeconds(level2 ? 1.5f : 2.5f);

        ChangeState(STATE.Idle);
    }

    IEnumerator Defense()
    {
        myAnim.SetBool("Defense", true);
        myStat.DP *= 1.5f;
        Shild.SetActive(true);
        yield return new WaitForSeconds(level2 ? 3.0f : 5.0f);
        myStat.DP /= 1.5f;
        Shild.SetActive(false);
        myAnim.SetBool("Defense", false);

        ChangeState(STATE.Idle);
    }

    public override void ResetMonster()
    {
        Player = GameManager.Instance.player.transform;
        ChangeState(STATE.reCreate);
        startPos = transform.position;
        StartCoroutine(RotatingToPosition(Player.position, true, false));
    }
}
