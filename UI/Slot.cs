using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]protected PlayerUI _myUI;
    public Item_Type[] item_Types;
    public bool back_inventory = false;
    public bool get_list_slotchk = false;
    public bool equipchk = false;
    protected Inventory inventory;
    protected Equipment equipment;
    Slot_Item child_slot_item;


    private void Awake()
    {
        _myUI = UIManager.Instance.playerUI.GetComponent<PlayerUI>();//transform.root.GetChild(0).GetChild(1).GetChild(1)
        inventory = UIManager.Instance.Inventory.GetComponent<Inventory>();
        equipment = UIManager.Instance.Equipment.GetComponent<Equipment>();
        child_slot_item = GetComponentInChildren<Slot_Item>();
    }

    //���Կ� ������ ������ �ְ� �κ��丮�� ��� ������ üũ
    //���ĭ�� �� Ư��ĭ��� üũ�Ѵ�.
    public virtual void OnDrop(PointerEventData eventData) // transform = ���� ��ġ, eventData.pointerDrag = �������� �̹���
    {
        Transform drag_item = eventData.pointerDrag.transform;
        Slot_Item slot_item = transform.GetComponentInChildren<Slot_Item>(); // ���� ��ġ
        if (!drag_item.GetComponent<Slot_Item>()) return;
        Slot_Item swap_item = eventData.pointerDrag.transform.GetComponent<Slot_Item>(); // �������� �̹���

        //ItemData
        Item_Data Slot_item_data = slot_item._Item.item_data;
        Item_Data Swap_item_data = swap_item._Item.item_data;

        //Item temp_item;
        if (Swap_item_data != null) // �ٲٴ� ������ ����
        {
            if (Slot_item_data != null) // ���� ��ġ�� ������ ���� ��
            {
                if (Swap_item_data.item_Type == Slot_item_data.item_Type) // Ÿ���� ������ ���߿� ���� �ַ� ��񿡼� ���
                {
                    if (Swap_item_data.item_Type == Item_Type.Poiton && Swap_item_data.name == Slot_item_data.name) // ���� ��ġ��(�̸��� ���� ��)
                    {
                        if (slot_item._Item.Count >= 99)
                        {
                            Swapitem(ref slot_item._Item, ref swap_item._Item);
                        }
                        else
                        {
                            slot_item._Item.Count += swap_item._Item.Count; // ������ 0�̸� ����
                            swap_item._Item.Count = slot_item._Item.Count > 99 ?  slot_item._Item.Count - 99 : 0;
                        }
                    }
                    else if (get_list_slotchk) // ������ ����Ʈ
                    {
                        return; // ������ ����Ʈ ���Կ� ���� �� �ٽ� ��ȯ
                    }
                    else if (equipchk) // ���
                    {
                        for (int i = 0; i < inventory.items_data.Count; i++)
                        {
                            if (inventory.items_data[i].item_data == null) // �κ��丮�� ���� á�� ���
                            {
                                inventory.items_data.RemoveAt(i);
                                inventory.items_data.Insert(i, slot_item._Item);
                                inventory.slots[i].Item_Set(inventory.items_data[i]);

                                Swapitem(ref slot_item._Item, ref swap_item._Item);

                                Swap_item_data = null;
                                swap_item.Item_Set(swap_item._Item);
                                slot_item.Item_Set(slot_item._Item);
                                break;
                            }
                            else
                            {
                                //�۾� ���
                                UIManager.Instance.InfoText.text = "�κ��丮�� ���� á���ϴ�.";
                            }
                        }
                    }
                    else // �κ�
                    {
                        Swapitem(ref slot_item._Item, ref swap_item._Item);
                    }

                    swap_item.Item_Set(swap_item._Item);
                    slot_item.Item_Set(slot_item._Item);
                }
                else
                {
                    UIManager.Instance.InfoText.text = "�� ���� ���� �� �����ϴ�!!";
                }
            }
            else  // ���� ��ġ�� �������� ���� ��
            {
                for (int i = 0; i < item_Types.Length; i++)
                {
                    if (Swap_item_data.item_Type == item_Types[i]) // Ÿ���� ������ >> ���
                    {
                        Swapitem(ref slot_item._Item, ref swap_item._Item);

                        Swap_item_data = null;
                        swap_item.Item_Set(swap_item._Item);
                        slot_item.Item_Set(slot_item._Item);
                        break;
                    }
                    else
                    {
                        UIManager.Instance.InfoText.text = "�� ���� ���� �� �����ϴ�!!";
                    }
                }
            }
            equipment.Equipment_Set();
        }


        slot_item._RectTransform.offsetMin = new Vector2(0, 0);
        slot_item._RectTransform.offsetMax = new Vector2(0, 0);
        swap_item._RectTransform.offsetMin = new Vector2(0, 0);
        swap_item._RectTransform.offsetMax = new Vector2(0, 0);
    }

    public void Swapitem(ref Item item, ref Item swapItem) // ������
    {
        Item temp_item;
        temp_item = item;
        item = swapItem;
        swapItem = temp_item;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (child_slot_item && child_slot_item._Item.item_data != null)
        {
            _myUI.tooltip.Active();
            _myUI.tooltip.gameObject.transform.SetAsLastSibling();
            _myUI.tooltip.transform.position = new Vector3(transform.position.x + 270.0f, transform.position.y - 60.0f, transform.position.z);
            if (child_slot_item._Item.item_data != null)
            {
                _myUI.tooltip.Set_Item_Info(child_slot_item._Item); // ����
            }
            else
            {
                _myUI.tooltip.Set_Item_Info(default);
            }
        }
    }


    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (child_slot_item)
        {
            _myUI.tooltip.InActive();
        }
        _myUI.tooltip.Set_Item_Info(default);
    }

}
