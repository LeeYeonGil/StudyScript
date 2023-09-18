using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Around_Skill : Skill
{
    public override void Using(Transform SpellPoint, Vector3 Hit_Point, float Damage, GameObject Target, GameObject Caster)
    {
        GameObject obj = Instantiate(Skilleff, SpellPoint.position, SpellPoint.rotation);
        Hit_Skill hit = obj.GetComponent<Hit_Skill>();
        hit._Damage = Damage;
        hit.Caster = Caster;
    }
}
