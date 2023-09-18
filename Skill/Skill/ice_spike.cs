using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Monster;

public class ice_spike : Hit_Skill
{
    public float CurTime = 0;
    public float DestroyTime = 0;
    public GameObject[] collisions;
    public Collider[] Enemys;

    // Start is called before the first frame update
    public void Start()
    {
        Round_Skill();
    }

    // Update is called once per frame
    void Update()
    {
        CurTime += Time.deltaTime;
        if (CurTime >= DestroyTime)
        {
            Destroy(gameObject);
        }
    }
    public void Round_Skill()
    {
        Enemys = Physics.OverlapSphere(transform.position, 5f);
        for (int i = 0; i < Enemys.Length; i++)
        {
            if (Enemys[i].gameObject.layer == 9)
            {
                if (Enemys[i].gameObject.GetComponent<Monster>())
                {
                    if (Enemys[i].gameObject.GetComponent<Monster>().myState != Monster.STATE.Dead)
                    {
                        if (debuffUse)
                        {
                            int rand = Random.Range(0, 101);
                            if (rand <= debuffPer)
                            {
                                Enemys[i].GetComponent<Monster>().AddDebuff(debuff, debuffvalue, debuffTime, Base_Monster.STATE.Roaming);
                            }
                        }
                        Enemys[i].gameObject.GetComponent<IBattle>()?.OnDamage(_Damage * _Damage_Increase, Caster);
                    }
                }
                else if(Enemys[i].gameObject.GetComponent<BossMonster>())
                {
                    if (Enemys[i].gameObject.GetComponent<BossMonster>().myState != BossMonster.STATE.Dead)
                    {
                        if (debuffUse)
                        {
                            int rand = Random.Range(0, 101);
                            if (rand <= debuffPer)
                            {
                                Enemys[i].GetComponent<BossMonster>().AddDebuff((BossMonster.DeBuff.Type)debuff, debuffvalue, debuffTime,Base_Monster.STATE.Battle);
                            }
                        }
                        Enemys[i].gameObject.GetComponent<IBattle>()?.OnDamage(_Damage * _Damage_Increase, Caster);
                    }
                }
            }
        }
    }
}
