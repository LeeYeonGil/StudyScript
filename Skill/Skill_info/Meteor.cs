using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Meteor : Hit_Skill
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
        while (i <= repeatTime)
        {
            Invoke("Range_Skill", 0.7f);
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (i >= repeatTime) {
            curTime += Time.deltaTime;
            if (curTime >= destroyTime) // 3.0f
            {
                Destroy(gameObject);
            }
        }
    }

    public void Range_Skill()
    {
        Enemys = Physics.OverlapSphere(transform.position, Range);
        for (int i = 0; i < Enemys.Length; i++)
        {
            if (Enemys[i].gameObject.layer == 9)
            {
                if (Enemys[i].gameObject?.GetComponent<Monster>().myState != Monster.STATE.Dead)
                {
                    Enemys[i].gameObject.GetComponent<IBattle>()?.OnDamage(_Damage * _Damage_Increase, Caster);
                }
            }
        }
    }


}
