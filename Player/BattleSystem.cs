using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattle
{
    void OnDamage(float dmg ,GameObject attacker);
    bool IsLive();
    void AddAttacker(IBattle ib);
    void RemoveAttacker(IBattle ib);
    void DeadMessage(Transform tr, float exp);
}

public class BattleSystem : CharacterMovement, IBattle
{
    protected List<IBattle> myAttackers = new List<IBattle>();
    [SerializeField]Transform _target = null;
    public Transform myTarget
    {
        get => _target;
        set
        {
            _target = value;
            if (_target != null)
            {
                _target.GetComponent<IBattle>()?.AddAttacker(this);
            }
        }
    }

    public virtual void OnDamage(float dmg, GameObject attacker)
    {

    }
    public virtual bool IsLive()
    {
        return true;
    }
    public void AddAttacker(IBattle ib)
    {
        myAttackers.Add(ib);
    }
    public virtual void DeadMessage(Transform tr, float exp)
    {

    }
    public void RemoveAttacker(IBattle ib)
    {
        for (int i = 0; i < myAttackers.Count;)
        {
            if (ib == myAttackers[i])
            {
                myAttackers.RemoveAt(i);
                break;
            }
            ++i;
        }
    }
}
