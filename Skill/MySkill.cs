using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySkill : MonoBehaviour
{
    public GameObject _Player;
    public Transform SpellPoint;
    public Vector3 HitPoint;
    public GameObject _myTarget = null;
    public float Damage;
    public GameObject Weapon, Armor, Helmet, Shoes;
    public Skill_Manage skill_Manage;
    public Skill Weapon_Skill, Helmet_Skill, Armor_Skill;
    public Slot_Item weapon_item, armor_item, helmet_item;
    public BattleSystem _mybattle;
    public RPGPlayer _RPGPlayer;

    Coroutine Skill_weapon;
    Coroutine Skill_armor;
    Coroutine Skill_helmet;

    void Awake()
    {
        Weapon_Skill = Weapon.GetComponent<DefaultSkill>();
        Helmet_Skill = Helmet.GetComponent<Around_Skill>(); // Çï¸ä ½ºÅ³
        Armor_Skill = Armor.GetComponent<Range_Skill>(); // °©¿Ê ½ºÅ³
        weapon_item = Weapon_Skill.slot.GetComponent<Slot_Item>();
        armor_item = Armor_Skill.slot.GetComponent<Slot_Item>();
        helmet_item = Helmet_Skill.slot.GetComponent<Slot_Item>();
        _mybattle = GetComponent<BattleSystem>();
        _RPGPlayer = GetComponent<RPGPlayer>();
    }
    void Start()
    {
        Skill_All_Set();
    }
    public void Skill_All_Set()
    {
        Skill_Weapon_Set();
        Skill_Helmet_Set();
        Skill_Armor_Set();
    }
   
    public void Skill_Weapon_Set() // ¹«±â ±³Ã¼½Ã Àû¿ë
    {
        Damage = _RPGPlayer.myStat.AP;
        if (weapon_item._Item.item_data != null)
        {
            Weapon_Skill.Skilleff = weapon_item._Item.item_data.skill_Eff;
        }
    }
    public void Skill_Helmet_Set() // Çï¸Ë ±³Ã¼½Ã Àû¿ë
    {
        Damage = _RPGPlayer.myStat.AP;
        if (helmet_item._Item.item_data != null)
        {
            Helmet_Skill.Skilleff = helmet_item._Item.item_data.skill_Eff;
        }
    }
    public void Skill_Armor_Set() // °©¿Ê ±³Ã¼½Ã Àû¿ë
    {
        Damage = _RPGPlayer.myStat.AP;
        if (armor_item._Item.item_data != null)
        {
            Armor_Skill.Skilleff = armor_item._Item.item_data.skill_Eff;
        }
    }

    public void Weapon_DefaultSkill() // ÆòÅ¸
    {
        _myTarget = _mybattle.myTarget.gameObject;// ¹«±â ½ºÅ³
        Weapon_Skill.Using(SpellPoint, HitPoint, Damage, _myTarget, _Player);
        if(Skill_weapon != null)
        {
            StopCoroutine(Skill_weapon);
            Skill_weapon = null;
        }
        Skill_weapon = skill_Manage.StartCoroutine(skill_Manage.CoolTime(skill_Manage.Skill_1, skill_Manage.Skill_Weapon_Cooling, skill_Manage.Skill_Weapon_Cooltime, 1));
    }

    public void Helmet_AroundSkill() // ÁÖº¯
    {
        if (_mybattle.myTarget != null)
        {
            _myTarget = _mybattle.myTarget.gameObject;// Çï¸ä ½ºÅ³
        }
        Helmet_Skill.Using(_Player.transform, HitPoint, Damage, _myTarget, _Player);
        if (Skill_helmet != null)
        {
            StopCoroutine(Skill_helmet);
            Skill_helmet = null;
        }
        Skill_helmet = skill_Manage.StartCoroutine(skill_Manage.CoolTime(skill_Manage.Skill_2, skill_Manage.Skill_Helmet_Cooling, skill_Manage.Skill_Helmet_Cooltime, 2));
    }
    public void Armor_RangeSkill() // ¹üÀ§
    {
        if (_mybattle.myTarget != null)
        {
            _myTarget = _mybattle.myTarget.gameObject;//  °©¿Ê ½ºÅ³
        }
        Armor_Skill.Using(SpellPoint, HitPoint, Damage, _myTarget, _Player);
        if (Skill_armor != null)
        {
            StopCoroutine(Skill_armor);
            Skill_armor = null;
        }
        Skill_armor = skill_Manage.StartCoroutine(skill_Manage.CoolTime(skill_Manage.Skill_3, skill_Manage.Skill_Armor_Cooling, skill_Manage.Skill_Armor_Cooltime, 3));
    }
}
