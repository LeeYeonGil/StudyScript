using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot_Item : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public GameObject Slot_root;

    public Item _Item;
    public Sprite Item_Image;
    public Color color;
    public Color orgColor;

    [SerializeField] PlayerUI playerUI;
    [SerializeField] Image image;
    Vector2 dragOffset = Vector2.zero;

    [SerializeField] bool chk = true;
    [SerializeField] bool equip_chk = false;
    [SerializeField] bool get_list_chk = false;
    [SerializeField] bool sellchk = false;
    [SerializeField] Transform orgPtransform;
    [SerializeField] Transform srollview;
    [SerializeField] TextMeshProUGUI _Count;
    public RectTransform _RectTransform;
    [SerializeField] Inventory inventory;
    [SerializeField]Vector3 orgPos = Vector3.zero;

    void Awake()
    {
        image = GetComponent<Image>();
        inventory = UIManager.Instance.Inventory.GetComponent<Inventory>();
        orgPtransform = transform.parent.parent;
        _RectTransform = GetComponent<RectTransform>();
        if (!equip_chk && !sellchk) _Count = transform.GetChild(0).GetComponent<TextMeshProUGUI>(); // GetComponentInChildren<TextMeshProUGUI>() 오브젝트 활성화
        orgColor = color;

        playerUI = UIManager.Instance.playerUI.GetComponent<PlayerUI>();
    }

    public void Item_Set(Item Data, int index = 0)
    {
        _Item = Data;

        if (_Item.item_data != null && _Item.Count > 0)
        {
            gameObject.SetActive(true);
            Item_Image = _Item.item_data.item_Image;
            image.sprite = _Item.item_data.item_Image;
            image.color = Color.white;
            if (!equip_chk)
            {
                _Count.gameObject.SetActive(true);
                _Count.text = $"x{_Item.Count}";
            }
        }
        else
        {
            Item_Image = null;
            image.sprite = null;
            image.color = orgColor;
            _Item.AttackSpeed = 0;
            _Item.AttackPoint = 0;
            _Item.DeffencePoint = 0;
            _Item.MoveSpeed = 0;
            if (!equip_chk)
            {
                _Count.gameObject.SetActive(false);
                _Count.text = $"x{0}";
            }
        }
        inventory.data_set(index);
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_Item.Count == 0) return;
        get_list_chk = GetComponentInParent<Slot>().get_list_slotchk;
        if(get_list_chk)
        {
            transform.parent.SetParent(srollview);
            transform.SetAsLastSibling();
        }
        image.raycastTarget = false;
        dragOffset = (Vector2)transform.position - eventData.position;
        if (chk)
        {
            transform.parent.SetAsLastSibling();
            transform.parent.parent.SetAsLastSibling();
            Slot_root.transform.SetAsLastSibling();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_Item.Count == 0) return;
        transform.position = eventData.position + dragOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!EventSystem.current.IsPointerOverGameObject()) // 아이템 버리기
        {
            UIManager.Instance.DropItemOC();
            UIManager.Instance.DropItemWindow.GetComponent<itemDropWindow>().SetText_item(this);
        }
        if (get_list_chk)
        {
            transform.parent.SetParent(orgPtransform);
        }
        inventory.InventReset();
        transform.localPosition = Vector3.zero;
        Item_Set(_Item);

        image.raycastTarget = true;
    }

}
