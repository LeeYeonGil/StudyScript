using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Equip_Stat_set
{
    public float AP;
    public float DP;
    public float AS;
    public float MS;
}
public class Equipment : UI_Default
{
    public GameObject WeaponSlot, ArmorSlot, HelmetSlot, ShoesSlot; // 아이템 타입에 맞게 슬롯에 들어가게함
    public RPGPlayer _player;
    public Skill_Manage skill_manager;
    public MySkill playerSkill;
    public AnimEvent myAniEvent;
    [SerializeField]CharacterStat orgstat;
    [SerializeField]Slot_Item weapon_slot, armor_slot, helmet_slot, shoes_slot;
    public Equip_Stat_set equipstat;
    public override void Awake()
    {
        orgstat = _player.myStat;
        weapon_slot = WeaponSlot.GetComponent<Slot_Item>();
        armor_slot = ArmorSlot.GetComponent<Slot_Item>();
        helmet_slot = HelmetSlot.GetComponent<Slot_Item>();
        shoes_slot = ShoesSlot.GetComponent<Slot_Item>();
        base.Awake();
    }
    public void Orgstat_set()
    {
        orgstat = _player.orgstat;
    }

    public void Equipment_Set()
    {
        Weapon_Set();
        Helmet_Set();
        Armor_Set();
        Stat_Set(_player, weapon_slot._Item, helmet_slot._Item,
           armor_slot._Item, shoes_slot._Item, orgstat);
    }


    public void Weapon_Set()
    {
        myAniEvent.Skill_Weapon.RemoveAllListeners();
        if (weapon_slot._Item.item_data != null)
        {
            myAniEvent.Skill_Weapon.AddListener(playerSkill.Weapon_DefaultSkill);
            Stat_Set(_player, weapon_slot._Item, helmet_slot._Item,
           armor_slot._Item, shoes_slot._Item, orgstat);
        }
        playerSkill.Skill_Weapon_Set();
        skill_manager.Skill_Weapon_Set();
    }
    public void Helmet_Set()
    {
        myAniEvent.Skill_Helmet.RemoveAllListeners();
        if (helmet_slot._Item.item_data != null)
        {
            myAniEvent.Skill_Helmet.AddListener(playerSkill.Helmet_AroundSkill);
            Stat_Set(_player, weapon_slot._Item, helmet_slot._Item,
           armor_slot._Item, shoes_slot._Item, orgstat);
        }
        playerSkill.Skill_Helmet_Set();
        skill_manager.Skill_Helmet_Set();
    }

    public void Armor_Set()
    {
        myAniEvent.Skill_Armor.RemoveAllListeners();
        if (armor_slot._Item.item_data != null)
        {
            myAniEvent.Skill_Armor.AddListener(playerSkill.Armor_RangeSkill);
            Stat_Set(_player, weapon_slot._Item, helmet_slot._Item,
           armor_slot._Item, shoes_slot._Item, orgstat);
        }
        playerSkill.Skill_Armor_Set();
        skill_manager.Skill_Armor_Set();
    }

    public void Shoes_Set()
    {
        if (shoes_slot._Item.item_data == null)
        {
            ShoesSlot.SetActive(false);
        }
        else
        {
            ShoesSlot.SetActive(true);
        }
    }

    Equip_Stat_set Item_Data_Nullchk(Item item)
    {
        Equip_Stat_set _stat;
        _stat.AP = 0.0f;
        _stat.DP = 0.0f;
        _stat.AS = 0.0f;
        _stat.MS = 0.0f;
        if (item.item_data != null)
        {
            _stat.AP = item.AttackPoint;
            _stat.DP = item.DeffencePoint;
            _stat.AS = item.AttackSpeed;
            _stat.MS = item.MoveSpeed;
        }
        return _stat;
    }

    void Stat_Set(RPGPlayer player, Item weapon, Item helmet, Item armor, Item shoes, CharacterStat orgStat)
    {
        Equip_Stat_set _stat, Weapon_stat, Helmet_stat, Armor_stat, Shoes_stat;

        Weapon_stat = Item_Data_Nullchk(weapon);
        Helmet_stat = Item_Data_Nullchk(helmet);
        Armor_stat = Item_Data_Nullchk(armor);
        Shoes_stat = Item_Data_Nullchk(shoes);
        _stat.AP = Weapon_stat.AP + Helmet_stat.AP + Armor_stat.AP + Shoes_stat.AP;
        _stat.DP = Weapon_stat.DP + Helmet_stat.DP + Armor_stat.DP + Shoes_stat.DP;
        _stat.AS = Weapon_stat.AS + Helmet_stat.AS + Armor_stat.AS + Shoes_stat.AS;
        _stat.MS = Weapon_stat.MS + Helmet_stat.MS + Armor_stat.MS + Shoes_stat.MS;

        player.myStat.AP = orgStat.AP + _stat.AP;
        player.myStat.DP = orgStat.DP + _stat.DP;
        player.myStat.AttackSpeed = orgStat.AttackSpeed + _stat.AS;
        player.myStat.MoveSpeed = orgStat.MoveSpeed + _stat.MS;

        equipstat.AP = _stat.AP;
        equipstat.DP = _stat.DP;
        equipstat.AS = _stat.AS;
        equipstat.MS = _stat.MS;
        player.equip_Stat = equipstat;
    }
}
