using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellChkPop : MonoBehaviour
{
    public Slot_Item sell_item;

    public void Sell_Item()
    {
        Debug.Log(sell_item._Item.item_data.price * sell_item._Item.Count );
        GameManager.Instance.Gold += sell_item._Item.item_data.price * sell_item._Item.Count;
        UIManager.Instance.playerUI.Gold_Set();
        sell_item._Item = default;
        sell_item.Item_Set(sell_item._Item);
        sell_item = null;
        gameObject.SetActive(false);

    }

    public void NonSell_Item()
    {
        sell_item = null;
        gameObject.SetActive(false);
    }
}
