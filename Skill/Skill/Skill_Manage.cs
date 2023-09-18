using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Skill_Manage : MonoBehaviour
{
    // Start is called before the first frame update
    public float Skill_Weapon_Cooltime;
    public float Skill_Helmet_Cooltime;
    public float Skill_Armor_Cooltime;

    public TMP_Text Skill_1;
    public TMP_Text Skill_2;
    public TMP_Text Skill_3;

    public Image Skill_Weapon_img;
    public Image Skill_Helmet_img;
    public Image Skill_Armor_img;

    public Image Skill_Weapon_Cooling;
    public Image Skill_Helmet_Cooling;
    public Image Skill_Armor_Cooling;

    public IEnumerator Weapon_Cooling;
    public IEnumerator Helmet_Cooling;
    public IEnumerator Armor_Cooling;

    public bool Skill_Weapon_chk;
    public bool Skill_Helmet_chk;
    public bool Skill_Armor_chk;

    public Equipment playerEquipment;
    void Start()
    {
        Skill_1.gameObject.SetActive(false);
        Skill_2.gameObject.SetActive(false);
        Skill_3.gameObject.SetActive(false);
        Skill_Weapon_Set();
        Skill_Helmet_Set();
        Skill_Armor_Set();
    }


    public IEnumerator CoolTime(TMP_Text skill_text, Image coolimg, float cooltime, int num) // 시간 자연스럽게(수정)
    {
        coolimg.gameObject.SetActive(true);
        skill_text.gameObject.SetActive(true);
        switch(num)
        {
            case 1:
                Skill_Weapon_chk = false;
                break;
            case 2:
                Skill_Helmet_chk = false;
                break;
            case 3:
                Skill_Armor_chk = false;
                break;
        }
        if (cooltime > 0.0f)
        {
            coolimg.fillAmount = 1.0f;
            float speed = 1.0f / cooltime;
            while (coolimg.fillAmount > 0.0f)
            {
                coolimg.fillAmount -= speed * 0.1f;
                skill_text.text = (cooltime -= (cooltime > 0.0f)? 0.1f : 0.0f).ToString("0.0"); // 쿨타임 텍스트 = (스킬 쿨타임 -= 일정속도 * 10.0f).문자열로 변경(소수첫째자리까지 표기);
                yield return new WaitForSeconds(0.1f);
            }
        }
        switch (num)
        {
            case 1:
                Skill_Weapon_chk = true;
                break;
            case 2:
                Skill_Helmet_chk = true;
                break;
            case 3:
                Skill_Armor_chk = true;
                break;
        }
        skill_text.gameObject.SetActive(false);
        coolimg.gameObject.SetActive(false);
    }

    public void Skill_Weapon_Set()
    {
        if (playerEquipment.WeaponSlot.GetComponent<Slot_Item>()._Item.item_data != null)
        {
            if(Weapon_Cooling != null)
            {
                StopCoroutine(Weapon_Cooling);
                Weapon_Cooling = null;
            }
            Skill_Weapon_Cooltime = playerEquipment.WeaponSlot.GetComponent<Slot_Item>()._Item.item_data.skill_Cool;
            Weapon_Cooling = CoolTime(Skill_1, Skill_Weapon_Cooling, Skill_Weapon_Cooltime,1);
            StartCoroutine(Weapon_Cooling);
            Skill_Weapon_img.sprite = playerEquipment.WeaponSlot.GetComponent<Slot_Item>()._Item.item_data.skill_Image;
            Skill_Weapon_Cooling.sprite = playerEquipment.WeaponSlot.GetComponent<Slot_Item>()._Item.item_data.skill_Image;
        }
        else
        {
            Skill_Weapon_img.sprite = null;
            Skill_Weapon_Cooling.sprite = null;
        }
    }

    public void Skill_Helmet_Set()
    {
        if(playerEquipment.HelmetSlot.GetComponent<Slot_Item>()._Item.item_data != null && playerEquipment.HelmetSlot.activeSelf == true)
        {
            if (Helmet_Cooling != null)
            {
                StopCoroutine(Helmet_Cooling);
                Helmet_Cooling = null;
            }
            Skill_Helmet_Cooltime = playerEquipment.HelmetSlot.GetComponent<Slot_Item>()._Item.item_data.skill_Cool;
            Helmet_Cooling = CoolTime(Skill_2, Skill_Helmet_Cooling, Skill_Helmet_Cooltime,2);
            StartCoroutine(Helmet_Cooling);
            Skill_Helmet_img.sprite = playerEquipment.HelmetSlot.GetComponent<Slot_Item>()._Item.item_data.skill_Image;
            Skill_Helmet_Cooling.sprite = playerEquipment.HelmetSlot.GetComponent<Slot_Item>()._Item.item_data.skill_Image;
        }
        else
        {
            Skill_Helmet_img.sprite = null;
            Skill_Helmet_Cooling.sprite = null;
        }
    }

    public void Skill_Armor_Set()
    {
        if(playerEquipment.ArmorSlot.GetComponent<Slot_Item>()._Item.item_data != null && playerEquipment.ArmorSlot.activeSelf == true)
        {
            if (Armor_Cooling != null)
            {
                StopCoroutine(Armor_Cooling);
                Armor_Cooling = null;
            }
            Skill_Armor_Cooltime = playerEquipment.ArmorSlot.GetComponent<Slot_Item>()._Item.item_data.skill_Cool;
            Armor_Cooling = CoolTime(Skill_3, Skill_Armor_Cooling, Skill_Armor_Cooltime,3);
            StartCoroutine(Armor_Cooling);
            Skill_Armor_img.sprite = playerEquipment.ArmorSlot.GetComponent<Slot_Item>()._Item.item_data.skill_Image;
            Skill_Armor_Cooling.sprite = playerEquipment.ArmorSlot.GetComponent<Slot_Item>()._Item.item_data.skill_Image;
        }
        else
        {
            Skill_Armor_img.sprite = null;
            Skill_Armor_Cooling.sprite = null;
        }
    }

}
