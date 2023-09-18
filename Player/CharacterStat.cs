using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public struct CharacterStat
{    
    [SerializeField] float hp;
    [SerializeField] float maxHp;
    [SerializeField] float mp;
    [SerializeField] float maxMp;
    [SerializeField] float moveSpeed;
    [SerializeField] float rotSpeed;
    [SerializeField] float ap;
    [SerializeField] float dp;
    [SerializeField] float attackRange;
    [SerializeField] float attackSpeed;
    [SerializeField] float attackDelay;
    [SerializeField] int Level;
    [SerializeField] float exp;
    [SerializeField] float maxExp;

    public UnityAction<float> changeHp;
    public UnityAction<float> changeMp;
    public UnityAction<float> changeExp;
    public float HP 
    { 
        get => hp;
        set
        {
            hp = Mathf.Clamp(value,0.0f, maxHp);
            changeHp?.Invoke(hp/maxHp);
        }
    }
    public float MaxHP
    {
        get => maxHp;
        set => maxHp = value;
    }
    public float MP
    {
        get => mp;
        set
        {
            mp = Mathf.Clamp(value, 0.0f, maxMp);
        }
    }
    public float MaxMP
    {
        get => maxMp;
        set => maxMp = value;
    }
    public float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = value;
    }
    public float RotSpeed
    {
        get => rotSpeed;
    }
    public float AP
    {
        get => ap;
        set => ap = value;
    }
    public float DP
    {
        get => dp;
        set => dp = value;
    }
    public float AttackRange
    {
        get => attackRange;
        set => attackRange = value;
    }
    public float AttackSpeed
    {
        get => attackSpeed;
        set => attackSpeed = value;
    }
    public float AttackDelay
    {
        get => attackDelay;
        set => attackDelay = value;
    }


    public float Exp
    {
        get => exp;
        set => exp = value;
    }
    public float MaxExp
    {
        get => maxExp;
        set => maxExp = value;
    }
    public int Lv
    {
        get => Level;
        set => Level = value;
    }
}
