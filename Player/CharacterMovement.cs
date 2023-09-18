using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
//public delegate void MyAction();

public class CharacterMovement : CharacterProperty
{
    Coroutine moveCo = null;
    Coroutine rotCo = null;
    Coroutine attackCo = null;
    Coroutine SkillCo = null;
    public float modifyMoveSpeed = 1.0f;
    protected bool rotate = false;
    protected void AttackTarget(Transform target)
    {
        if (attackCo != null)
        {
            StopCoroutine(attackCo);
            attackCo = null;
        }
        if (moveCo != null)
        {
            StopCoroutine(moveCo);
            moveCo = null;
        }
        if (rotCo != null)
        {
            StopCoroutine(rotCo);
            rotCo = null;
        }
        //StopAllCoroutines();
        attackCo = StartCoroutine(AttckingTarget(target, myStat.AttackRange, myStat.AttackDelay / myStat.AttackSpeed));
    }

    protected void MoveToPosition(Vector3 pos, UnityAction done = null, bool Rot = true, bool talk = false)
    {
        
        if(attackCo != null)
        {
            StopCoroutine(attackCo);
            attackCo = null;
        }
        if (moveCo != null)
        {
            StopCoroutine(moveCo);
            moveCo = null;
        }
        if (!talk)
        {
            moveCo = StartCoroutine(MovingToPostion(pos, done));
        }

        if (Rot)
        {
            if (rotCo != null)
            {
                StopCoroutine(rotCo);
                rotCo = null;
            }
            rotCo = StartCoroutine(RotatingToPosition(pos, false, talk));
        }
    }

    protected void Skill_Casting(Vector3 pos, float skillRange, string skill_name, bool stay)
    {
        if(SkillCo != null)
        {
            StopCoroutine(SkillCo);
            SkillCo = null;
        }
        SkillCo = StartCoroutine(SKill_Cast(pos, skillRange, skill_name, stay));
    }

    protected IEnumerator RotatingToPosition(Vector3 pos, bool bosschk, bool talk)
    {
        rotate = true;
        Vector3 dir = (pos - transform.position).normalized;
        float Angle = Vector3.Angle(transform.forward, dir);
        float rotDir = 1.0f;
        if (Vector3.Dot(transform.right, dir) < 0.0f)
        {
            rotDir = -rotDir;
        }
        if(talk) myAnim.SetBool("IsMoving", false);
        while (Angle > 0.0f) // bosschk? Angle > 0.0f : true
        {
            if (!myAnim.GetBool("IsAttacking") && !myAnim.GetBool("IsSkill"))
            {
                float delta = myStat.RotSpeed * Time.deltaTime;
                if (delta > Angle)
                {
                    delta = Angle;
                }
                Angle -= delta;
                transform.Rotate(Vector3.up * rotDir * delta, Space.World);
            }
            yield return null;
        }
        rotate = false;
    }


    IEnumerator MovingToPostion(Vector3 pos, UnityAction done)
    {
        Vector3 dir = pos - transform.position;
        float dist = dir.magnitude;
        dir.Normalize();

        //달리기 시작
        myAnim.SetBool("IsMoving", true);
        while (dist > 0.0f)
        {
            if (myAnim.GetBool("IsAttacking"))
            {
                myAnim.SetBool("IsMoving", false);
                yield break;
            }

            if (!myAnim.GetBool("IsAttacking") && !myAnim.GetBool("IsSkill"))
            {
                float delta = myStat.MoveSpeed * modifyMoveSpeed  * Time.deltaTime;
                if (delta > dist)
                {
                    delta = dist;
                }

                dist -= delta;
                transform.Translate(dir * delta, Space.World);
            }
            yield return null;
        }
        //달리기 끝 - 도착
        myAnim.SetBool("IsMoving", false);
        done?.Invoke();
    }

    IEnumerator AttckingTarget(Transform target, float AttackRange, float AttackDelay)
    {
        float playTime = 0.0f;
        float delta = 0.0f;
        while (target != null)
        {
            if(!myAnim.GetBool("IsAttacking")) playTime += Time.deltaTime;
            //이동
            Vector3 dir = target.position - transform.position;
            float dist = dir.magnitude;
            dir.Normalize();
            if (dist > AttackRange)
            {
                myAnim.SetBool("IsMoving", true);
                delta = myStat.MoveSpeed * modifyMoveSpeed  * Time.deltaTime;
                if (delta > dist)
                {
                    delta = dist;
                }
                if (!myAnim.GetBool("IsAttacking") && !myAnim.GetBool("IsSkill"))
                {
                    transform.Translate(dir * delta, Space.World);
                }
            }
            else
            {
                myAnim.SetBool("IsMoving", false);
                if (playTime >= AttackDelay)
                {
                    //공격
                    playTime = 0.0f;
                    myAnim.SetTrigger("Default");
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
            if (!myAnim.GetBool("IsAttacking") && !myAnim.GetBool("IsSkill"))
            {
                transform.Rotate(Vector3.up * delta * rotDir, Space.World);
            }
            yield return null;
        }
        myAnim.SetBool("IsMoving", false);
    }
    IEnumerator SKill_Cast(Vector3 pos, float AttackRange, string Skill_name, bool stay)
    {
        float playTime = 0.0f;
        float delta = 0.0f;
        //이동
        while (!myAnim.GetBool("IsSkill"))
        {
            if (!myAnim.GetBool("IsSkill")) playTime += Time.deltaTime;
            Vector3 dir = pos - transform.position;
            float dist = dir.magnitude;
            dir.Normalize();
            if (!stay)
            {
                if (dist > AttackRange)
                {
                    myAnim.SetBool("IsMoving", true);
                    delta = myStat.MoveSpeed * Time.deltaTime;
                    if (delta > dist)
                    {
                        delta = dist;
                    }
                    dist -= delta;
                    if (!myAnim.GetBool("IsSkill"))
                    {
                        transform.Translate(dir * delta, Space.World);
                    }
                }
                else
                {
                    myAnim.SetBool("IsMoving", false);
                    if (playTime >= 0.3f)
                    {
                        //공격
                        playTime = 0.0f;
                        myAnim.SetTrigger(Skill_name);
                    }
                }
            }
            else
            {
                myAnim.SetBool("IsMoving", false);
                if (playTime >= 0.3f)
                {
                    //공격
                    playTime = 0.0f;
                    myAnim.SetTrigger(Skill_name);
                }
            }
            //회전
            delta = myStat.RotSpeed * Time.deltaTime;
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
            if (!myAnim.GetBool("IsAttacking") && !myAnim.GetBool("IsSkill"))
            {
                transform.Rotate(Vector3.up * delta * rotDir, Space.World);
            }
            yield return null;
        }
        myAnim.SetBool("IsMoving", false);
    }

  
}
