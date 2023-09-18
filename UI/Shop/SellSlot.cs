using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SellSlot : MonoBehaviour, IDropHandler
{
    [SerializeField]SellChk Sellchk;
    public void OnDrop(PointerEventData eventData)
    {
        Transform drag_item = eventData.pointerDrag.transform;
        if (!drag_item.GetComponent<Slot_Item>()) return;
        Slot_Item swap_item = eventData.pointerDrag.transform.GetComponent<Slot_Item>(); // 가져가는 이미지
        if (swap_item._Item.Count != 0)
        {
            Sellchk.gameObject.SetActive(true);
            Sellchk.transform.SetAsLastSibling();
            Sellchk.sellitem = swap_item;
            Sellchk._Image.sprite = swap_item._Item.item_data.item_Image;
            Sellchk.Price = swap_item._Item.item_data.price;
        }
    }


   


}
