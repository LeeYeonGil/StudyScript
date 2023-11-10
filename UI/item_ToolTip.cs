using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class item_ToolTip : MonoBehaviour
{
    public TMP_Text Item_Name;
    public Image Item_Img;

    public TMP_Text Item_Grade;
    public TMP_Text Item_Type;
    public TMP_Text Item_AD, Item_DP, Item_AS, Item_SP;
    public TMP_Text Item_Info;

    public Image Item_Skill_Img;
    public TMP_Text Item_Skill_Info;
    public TMP_Text Item_Skill_Cool;
    public TMP_Text Item_Skill_Cost;

    public TMP_Text Item_Price;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            InActive();
        }
    }

    public void Active()
    {
        gameObject.SetActive(true);
    }

    public void InActive()
    {
        gameObject.SetActive(false);
    }

    public void Set_Item_Info(Item data)
    {
        if (data.item_data != null)
        {
            Item_Name.text = data.item_data._Name; // 등급 별 텍스트 색상 변경
            if(data.item_data.item_Type != global::Item_Type.Poiton && data.item_data.item_Type != global::Item_Type.etc) 
            Item_Name.text += $"(+{data.Upgrade})";

            Item_Img.sprite = data.item_data.item_Image;

            switch (data.item_data.item_Grade)
            {
                case global::ITEMGRADE.Normal:
                    Item_Grade.text = "일반";
                    Item_Name.color = Color.white;
                    break;
                case global::ITEMGRADE.Magic:
                    Item_Grade.text = "매직";
                    Item_Name.color = Color.blue;
                    break;
                case global::ITEMGRADE.Unique:
                    Item_Grade.text = "유니크";
                    Item_Name.color = Color.magenta;
                    break;
                case global::ITEMGRADE.Epic:
                    Item_Grade.text = "에픽";
                    Item_Name.color = Color.green;
                    break;
                case global::ITEMGRADE.Legend:
                    Item_Grade.text = "레전드";
                    Item_Name.color = Color.red;
                    break;
                default:
                    break;
            }
            switch (data.item_data.item_Type)
            {
                case global::Item_Type.Weapon:
                    Item_Type.text = "무기";
                    break;
                case global::Item_Type.Helmet:
                    Item_Type.text = "헬멧";
                    break;
                case global::Item_Type.Shoes:
                    Item_Type.text = "신발";
                    break;
                case global::Item_Type.Armor:
                    Item_Type.text = "갑옷";
                    break;
                case global::Item_Type.Poiton:
                    Item_Type.text = "포션";
                    break;
                case global::Item_Type.etc:
                    Item_Type.text = "기타";
                    break;
                default:
                    break;
            }


            if (data.item_data.item_Type == global::Item_Type.Helmet || data.item_data.item_Type == global::Item_Type.Weapon ||
                data.item_data.item_Type == global::Item_Type.Shoes || data.item_data.item_Type == global::Item_Type.Armor)
            {
                Item_AD.text = data.AttackPoint.ToString("0.0");
                Item_DP.text = data.DeffencePoint.ToString("0.0");
                Item_AS.text = data.AttackSpeed.ToString("0.0");
                Item_SP.text = data.MoveSpeed.ToString("0.0");
            }

            Item_Info.text = data.item_data.item_Info;

            if (data.item_data.skill_Eff != null)
            {
                Item_Skill_Img.sprite = data.item_data.skill_Image;
                Item_Skill_Info.text = data.item_data.skill_Info;
                Item_Skill_Cool.text = data.item_data.skill_Cool.ToString();
                Item_Skill_Cost.text = data.item_data.skill_Cost.ToString();
            }
            else
            {
                Item_Skill_Img.sprite = null;
                Item_Skill_Info.text = null;
                Item_Skill_Cool.text = null;
                Item_Skill_Cost.text = null;
            }
            Item_Price.text = data.item_data.price.ToString();
        }
        else
        {
            Item_Name.text = null;
            Item_Grade.text = null;
            Item_Img.sprite = null;
            Item_Name.color = Color.white;
            Item_AD.text = "0.0";
            Item_DP.text = "0.0";
            Item_AS.text = "0.0";
            Item_SP.text = "0.0";
            Item_Type.text = null;
            Item_Info.text = null;
            Item_Skill_Img.sprite = null;
            Item_Skill_Info.text = null;
            Item_Skill_Cool.text = null;
            Item_Skill_Cost.text = null;
            Item_Price.text = null;
        }
    }
}
