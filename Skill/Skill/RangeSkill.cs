using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSkill : Hit_Skill
{
    public float curTime;
    public float destroyTime;
    public Collider[] Enemys;
    public float Range;
    public int repeatTime;
    int i = 0;
    public ParticleCollisionInstance part;
    // Start is called before the first frame update
    void Start()
    {
        part._Damage = _Damage * _Damage_Increase;
        part.Caster = Caster; 
        part.DestroyTimeDelay = destroyTime;
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
}
