using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Monster;
public class MagicProjectile : Hit_Skill
{
    public float MoveSpeed = 10.0f;
    //public float totalDist = 0.0f;
    public float curTime;
    public float destroyTime;
    public float Range;
    public int repeatTime;
    int i = 0;

    public Vector3 disVec;

    public bool PlayerOrBoss; // P : true, B : false;
    // Start is called before the first frame update


    private void Update()
    {
        if (i >= repeatTime)
        {
            curTime += Time.deltaTime;
            if (curTime >= destroyTime)
            {
                Destroy(gameObject);
            }
            transform.position += transform.forward * (MoveSpeed * Time.deltaTime);
            //transform.forward = disVec;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!PlayerOrBoss)
        {
            if (collision.gameObject.layer == 9)
            {
                if (collision.gameObject.GetComponent<Monster>())
                {
                    if (collision.gameObject.GetComponent<Monster>().myState != Monster.STATE.Dead)
                    {
                        if (debuffUse)
                        {
                            int rand = Random.Range(0, 101);
                            if (rand <= debuffPer)
                            {
                                collision.gameObject.GetComponent<Monster>().AddDebuff(debuff, debuffvalue, debuffTime, Base_Monster.STATE.Roaming);
                            }
                        }
                        if (collision.gameObject.GetComponent<Monster>().myTarget == null) collision.gameObject.GetComponent<Monster>().FindTarget(Caster.transform);
                        collision.gameObject.GetComponent<IBattle>()?.OnDamage(_Damage, Caster);
                        Destroy(gameObject);
                    }
                }
                else
                {
                    if (collision.gameObject.GetComponent<BossMonster>().myState != BossMonster.STATE.Dead)
                    {
                        if (debuffUse)
                        {
                            int rand = Random.Range(0, 101);
                            if (rand <= debuffPer)
                            {
                                collision.gameObject.GetComponent<BossMonster>().AddDebuff((BossMonster.DeBuff.Type)debuff, debuffvalue, debuffTime, Base_Monster.STATE.Battle);
                            }
                        }
                        collision.gameObject.GetComponent<IBattle>()?.OnDamage(_Damage, Caster);
                        Destroy(gameObject);
                    }
                }
            }
        }
        else
        {
            Debug.Log("플레이어 공격");
            if (collision.gameObject.gameObject.layer == 6)
            {
                if (collision.gameObject?.GetComponent<RPGPlayer>().myState != RPGPlayer.STATE.Death)
                {
                    collision.gameObject.GetComponent<IBattle>()?.OnDamage(_Damage, Caster);
                }
            }
        }
    }
   
}
