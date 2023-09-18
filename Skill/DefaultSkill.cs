using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultSkill : Skill
{
    public override void Using(Transform SpellPoint, Vector3 Hit_Point, float Damage, GameObject Target, GameObject Caster)
    {
        base.Using(SpellPoint, Hit_Point, Damage, Target, Caster);
    }
}
