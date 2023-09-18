using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Monster;

public class Lighting : Hit_Skill
{
    public float curTime;
    public float destroyTime;
    public Collider[] Enemys;
    public float Range;
    public int repeatTime;
    int i = 0;
    public List<GameObject> gameObjects = new List<GameObject>();
    // Start is called before the first frame update
    public void Start()
    {
        Invoke("Round_Skill", 2.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (i >= repeatTime)
        {
            curTime += Time.deltaTime;
            if (curTime >= destroyTime)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Round_Skill()
    {
        Enemys = Physics.OverlapSphere(transform.position, 5f);
        for (int i = 0; i < Enemys.Length; i++)
        {
            if (Enemys[i].gameObject.layer == 9)
            {
                if (Enemys[i].gameObject?.GetComponent<Monster>().myState != Monster.STATE.Dead)
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
        }
    }
}
