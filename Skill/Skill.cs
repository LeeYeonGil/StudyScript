using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Skill : MonoBehaviour
{
    public GameObject Skilleff;
    public Hit_Skill _hit;
    public Slot_Item slot;

    private void Awake()
    {
        slot = GetComponent<Slot_Item>();
    }
    private void OnDisable()
    {
        if (slot._Item.item_data != null)
        {
            Skilleff = slot._Item.item_data.skill_Eff;
        }
    }


    // 기본공격과 범위공격 두개로 구분하기
    public virtual void Using(Transform SpellPoint, Vector3 Hit_Point, float Damage, GameObject Target = null, GameObject Caster = null)
    {
        GameObject obj = Instantiate(Skilleff, SpellPoint.position, SpellPoint.rotation);
        _hit = obj.GetComponent<Hit_Skill>();
        _hit._myTarget = Target;
        _hit._Damage = Damage;
        _hit.Caster = Caster;
    }
}
