using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSlot : Slot
{
    [SerializeField] Item _data;
    [SerializeField] TMP_Text Price;
    [SerializeField] Image item_img;

    [SerializeField] buyChk buychk;

    private void Start()
    {
        item_img.sprite = _data.item_data.item_Image;
        Price.text = (_data.item_data.price * 2).ToString();
    }
    public override void OnDrop(PointerEventData eventData)
    {
        return;
    }


    public void BuyItem()
    {
        buychk.gameObject.SetActive(true);
        buychk.transform.SetAsLastSibling();
        buychk.buyitem.item_data = _data.item_data;
        buychk._Image.sprite = _data.item_data.item_Image;
        buychk.Price = _data.item_data.price;
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (_data.item_data != null)
        {
            _myUI.tooltip.Active();
            _myUI.tooltip.gameObject.transform.SetAsLastSibling();
            _myUI.tooltip.transform.position = new Vector3(transform.position.x + 270.0f, transform.position.y - 60.0f, transform.position.z);
            _myUI.tooltip.Set_Item_Info(_data); // ¼öÁ¤
        }
    }
    public override void OnPointerExit(PointerEventData eventData)
    {

        _myUI.tooltip.InActive();
        _myUI.tooltip.Set_Item_Info(default);
    }
}
