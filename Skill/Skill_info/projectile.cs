using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : Hit_Skill
{
    /*public float MoveSpeed = 10.0f;
    public float totalDist = 0.0f;*/
    public float curTime;
    public float destroyTime;
    public float Range;
    public int repeatTime;
    int i = 0;

    public ParticleCollisionInstance part;
    // Start is called before the first frame update

    public void Start()
    {
        part = GetComponent<ParticleCollisionInstance>();
        part._Damage = _Damage * _Damage_Increase;
        part.Caster = Caster;
    }

    private void Update()
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
