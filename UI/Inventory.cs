using DTT.Utils.Extensions;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory : UI_Default
{
    public List<GameObject> Item = new List<GameObject> ();
    public List<Item> items_data = new List<Item> ();
    public Slot_Item[] slots; // GetComponent ȣ�� ���̱��
    [SerializeField]List<Item> sort_items = new List<Item>();

    public override void Awake()
    {
        slots = new Slot_Item[Item.Count]; // ����

        for (int i = 0; i < Item.Count; i++) // ó���� �� ��Ȱ��ȭ
        {
            slots[i] = Item[i].GetComponent<Slot_Item>();
            items_data.Add(slots[i]._Item);
        }
        base.Awake();
    }

    public void Slot_Actice()
    {
        for (int i = 0; i < Item.Count; i++)
        {
            Item[i].SetActive(true);
        }
    }

    public void Slot_inActice()
    {
        for (int i = 0; i < Item.Count; i++)
        {
            if (Item[i].GetComponent<Slot_Item>()._Item.item_data == null)
            {
                Item[i].SetActive(false);
            }
        }
    }
    public void data_set(int i)
    {
        slots[i] = Item[i].GetComponent<Slot_Item>();
        items_data[i] = slots[i]._Item;
    }
    public void InventReset()
    {
        for (int i = 0; i < Item.Count; i++)
        {
            slots[i] = Item[i].GetComponent<Slot_Item>();
            items_data[i] = slots[i]._Item;
            slots[i].Item_Set(items_data[i], i);
        }
    }
    public void Inventory_Sort()
    {
        InventReset();

        for (int i = 0; i < items_data.Count; i++)
        {
            if (items_data[i].item_data != null)
            {
                sort_items.Add(items_data[i]);
            }
        }
        while(sort_items.Count < Item.Count)
        {
            sort_items.Add(default);
        }
        items_data.Clear();
        for (int i = 0; i < Item.Count; i++)
        {
            slots[i] = Item[i].GetComponent<Slot_Item>();
            items_data.Add(sort_items[i]);
            slots[i].Item_Set(items_data[i], i);
            Item[i].SetActive(true);
        }
        sort_items.Clear();
    }


   

}
