using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Get_Item_List : UI_Default
{
    public List<Item> itemData = new List<Item>();
    [SerializeField] GameObject[] Item_Slot;
    [SerializeField] Slot[] slots;
    [SerializeField] Slot_Item[] item_datas;
    [SerializeField] TMP_Text[] item_info;
    [SerializeField] Transform[] slot_pos;
    [SerializeField] Chest _chest;
    public GameObject NonTouch;
    public GameObject DontMove;

    Coroutine chk_item;
    public override void Awake()
    {
        for(int i = 0; i < Item_Slot.Length; i++)
        {
            Item_Slot[i].SetActive(false);
        }
        base.Awake();
    }
    public override void UI_OC()
    {
        gameObject.transform.localPosition = orgPos;
        OC = !OC;
        gameObject.SetActive(OC);
    }

    public void Get_Item(Chest chest)
    {
        if(chk_item != null)
        {
            StopCoroutine(chk_item);
            chk_item = null;
        }
        for (int i = 0; i < Item_Slot.Length; i++)
        {
            Item_Slot[i].SetActive(false);
        }
        _chest = chest;
        for (int i = 0; i < itemData.Count; i++)
        {
            Item_Slot[i].SetActive(true);
            item_datas[i]._Item = itemData[i];
            item_info[i].text = itemData[i].item_data._Name;
            item_datas[i].Item_Set(itemData[i]);
        }
        chk_item = StartCoroutine(Check_Item());
    }
    public void hide()
    {
        gameObject.SetActive(false);
        DontMove.SetActive(false);
        _chest.ChangeState(Chest.STATE.Idle);
        if(!_chest.get_chk) StopCoroutine(chk_item);
    }

    IEnumerator Check_Item() // 아이템 사라지기
    {
        int count = itemData.Count;
        int i = 0;
        while (_chest.items.Count > 0)
        {
            if(_chest.isPlayerEnter)
            {
                NonTouch.SetActive(false);
                DontMove.SetActive(true);
            }
            else
            {
                DontMove.SetActive(false);
                NonTouch.SetActive(true);
            }
            if (item_datas[i]._Item.item_data == null)
            {
                Item_Slot[i].SetActive(false);
                _chest.items.RemoveAt(i);
                itemData.Clear();

                _chest.Chest_Get_Item();
                count = itemData.Count;
            }
            i = (++i >= count) ? 0 : i;

            yield return null;
        }
        _chest.get_chk = true;
    }
}
