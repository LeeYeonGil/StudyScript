using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : Hit_Skill
{
    public GameObject hitEffect;
    [SerializeField] protected LayerMask myEnemy;
    [field: SerializeField]
    public float moveSpeed
    {
        get;
        private set;
    }
    protected float Damage
    {
        get; private set;
    }
    protected Monster myTarget = null;
    public void OnFire(Monster target, LayerMask mask, float dmg)
    {
        Damage = dmg;
        myTarget = target;
        /*target.DeathAlarm += (Monster mon) =>
        {
            myTarget = null;
        };*/
        StartCoroutine(Attacking(mask));
    }
    protected abstract IEnumerator Attacking(LayerMask mask);
    protected abstract void OnHit();
}
