using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ITEMGRADE
{
    Normal, Magic, Unique, Epic, Legend
}
public enum Item_Type
{
    Helmet, Weapon, Armor, Shoes, Poiton, etc
}


[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjcts/ItemData", order = 1)]
public class Item_Data : ScriptableObject
{
    [SerializeField] ITEMGRADE Item_Grade;
    [SerializeField] string _name;
    [SerializeField] Item_Type Item_type;
    [SerializeField] int Price;
    [SerializeField] Sprite Item_Image;
    [SerializeField] string Item_Info;
    [SerializeField] Sprite Skill_Image;
    [SerializeField] string Skill_Info;
    [SerializeField] GameObject skill_eff;
    [SerializeField] float skill_cost;
    [SerializeField] float skill_cool;
    [SerializeField] float HP;
    [SerializeField] float MP;

    public ITEMGRADE item_Grade
    {
        get => Item_Grade;
        set => item_Grade = value;
    }
    public Item_Type item_Type
    {
        get => Item_type;
    }
    public float hp
    {
        get => HP;
        set => HP = value;
    }
    public float mp
    {
        get => MP;
        set => MP = value;
    }
    public string _Name
    {
        get => _name;
    }
    
    public int price
    {
        get => Price;
    }
    public Sprite item_Image
    {
        get => Item_Image;
    }
    public Sprite skill_Image
    {
        get => Skill_Image;
    }
    public string item_Info
    {
        get => Item_Info;
    }
    public string skill_Info
    {
        get => Skill_Info;
    }
    public GameObject skill_Eff
    {
        get => skill_eff;
    }
    public float skill_Cost
    {
        get => skill_cost;
    }
    public float skill_Cool
    {
        get => skill_cool;
    }
}
