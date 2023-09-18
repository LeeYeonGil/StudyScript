using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellChk : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    public Slot_Item sellitem;
    public Image _Image;
    public int Price;
    [SerializeField] int itemCount;
    [SerializeField] GameObject resellChk;

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
        if (itemCount < sellitem._Item.Count)
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

    public void Sell_chk()
    {
        resellChk.SetActive(true);
        resellChk.transform.SetAsLastSibling();
        itemnameCount.text = $"{sellitem._Item.item_data._Name}" + " " + "<color=#ff0000>" + $"{itemCount}" + "</color>" + "°³";
        sumPrice = sellitem._Item.item_data.price * itemCount;
        SumPrice.text = "$ " + $"{sumPrice}";
        gameObject.SetActive(false);
    }

    public void SellCancel()
    {
        itemCount = 0;
        sumPrice = 0;
        gameObject.SetActive(false);
    }

    public void item_Sell_Cancel()
    {
        resellChk.SetActive(false);
        gameObject.SetActive(true);
    }

    public void item_sell()
    {
        if (itemCount != 0)
        {
            GameManager.Instance.Gold += sumPrice;
            GameManager.Instance.GoldSet();
            if (sellitem._Item.Count == 0)
            {
                sellitem._Item = default;
                sellitem.Item_Set(sellitem._Item);
                sellitem = null;
            }
            else
            {
                sellitem._Item.Count -= itemCount;
                sellitem.Item_Set(sellitem._Item);
                sellitem = null;
            }
        }
        itemCount = 0;
        sumPrice = 0;
        resellChk.SetActive(false);
    }
}
