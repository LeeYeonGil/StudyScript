using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class itemDropWindow : UI_SetLast_SetActive
{
    public TMP_Text Item_Name;
    Slot_Item slot;
    Item data;

    public void SetText_item(Slot_Item item)
    {
        slot = item;
        data = item._Item;
        switch (item._Item.item_data.item_Grade)
        {
            case ITEMGRADE.Normal:
                Item_Name.text = "<color=#FFFFFF>" + item._Item.item_data._Name + "</color>" + "(를)을";
                break;
            case ITEMGRADE.Magic:
                Item_Name.text = "<color=#0000FF>" + item._Item.item_data._Name + "</color>" + "(를)을";
                break;
            case ITEMGRADE.Unique:
                Item_Name.text = "<color=#FF00FF>" + item._Item.item_data._Name + "</color>" + "(를)을";
                break;
            case ITEMGRADE.Epic:
                Item_Name.text = "<color=#008000>" + item._Item.item_data._Name + "</color>" + "(를)을";
                break;
            case ITEMGRADE.Legend:
                Item_Name.text = "<color=#FF0000>" + item._Item.item_data._Name + "</color>" + "(를)을";
                break;
        }
    }

    public void deleteItem()
    {
        slot._Item = default;
        UIManager.Instance.Inventory.GetComponent<Inventory>().InventReset();
        slot.transform.localPosition = Vector3.zero;
        slot.Item_Set(slot._Item);
    }

    public void CancelDrop()
    {
        slot._Item = data;
        UIManager.Instance.Inventory.GetComponent<Inventory>().InventReset();
        slot.transform.localPosition = Vector3.zero;
        slot.Item_Set(slot._Item);
    }

}
