using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class buyChk : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    public Item buyitem;
    public Image _Image;
    public int Price;
    [SerializeField] int itemCount;
    [SerializeField] GameObject rebuyChk;

    [SerializeField] TMP_Text item_Count;

    [SerializeField] TMP_Text itemnameCount;
    [SerializeField] TMP_Text SumPrice;

    public int sumPrice = 0;

    private void OnEnable()
    {
        item_Count.text = $"{itemCount}";
    }

    public void UpCount()
    {
        if (itemCount < 100)
        {
            itemCount++;
        }
        item_Count.text = $"{itemCount}";
    }
    public void DownCount()
    {
        if (itemCount > 0) itemCount--;
        item_Count.text = $"{itemCount}";
    }

    public void Buy_chk()
    {
        rebuyChk.SetActive(true);
        rebuyChk.transform.SetAsLastSibling();
        itemnameCount.text = $"{buyitem.item_data._Name}" + " " + "<color=#ff0000>" + $"{itemCount}" + "</color>" + "개";
        sumPrice = buyitem.item_data.price * 2 * itemCount;
        SumPrice.text = "$ " + $"{sumPrice}";
        gameObject.SetActive(false);
    }

    public void BuyCancel()
    {
        itemCount = 0;
        sumPrice = 0;
        gameObject.SetActive(false);
    }

    public void item_buy_Cancel()
    {
        rebuyChk.SetActive(false);
        gameObject.SetActive(true);
    }

    public void item_buy()
    {
        if(GameManager.Instance.Gold >= sumPrice)
        {
            GameManager.Instance.Gold -= sumPrice;
            GameManager.Instance.GoldSet();
            for (int i = 0; i < inventory.items_data.Count; i++)
            {
                if (inventory.items_data[i].item_data == null) // 인벤토리가 가득 찼을 경우
                {
                    buyitem.Count = itemCount;
                    inventory.items_data.RemoveAt(i);
                    inventory.items_data.Insert(i,buyitem);
                    inventory.slots[i].Item_Set(inventory.items_data[i]);
                    break;
                }
                else
                {
                    //작업 취소
                    Debug.Log($"{i}번째 아이템 존재");
                }
            }
        }
        itemCount = 0;
        sumPrice = 0;
        rebuyChk.SetActive(false);
    }
}
