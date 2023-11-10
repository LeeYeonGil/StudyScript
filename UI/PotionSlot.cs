using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class PotionSlot : Slot 
{
    public Item item_potion;
    public TMP_Text Count;
    public Image item_img;
    public RPGPlayer player;
    public Color orgColor;
    [SerializeField]Slot_Item Potions;

    Coroutine count;
    // Start is called before the first frame update

    private void Awake()
    {
        _myUI = UIManager.Instance.playerUI;
        inventory = UIManager.Instance.Inventory.GetComponent<Inventory>();
        equipment = UIManager.Instance.Equipment.GetComponent<Equipment>();
        orgColor = item_img.color;
    }
    public override void OnDrop(PointerEventData eventData) 
    {
        Potions = eventData.pointerDrag.transform.GetComponent<Slot_Item>();
        item_img.color = Color.white;
        item_potion = Potions._Item;
        item_img.sprite = item_potion.item_data.item_Image;
        Count.SetText(Potions._Item.Count.ToString());
        if (count != null)
        {
            StopCoroutine(count);
            count = null;
        }
        count = StartCoroutine(item_count_chk());
    }

    public void UseItem()
    {
        if (item_potion.item_data != null)
        {
            if (item_potion.item_data.hp != 0)
            {
                player.myStat.HP += item_potion.item_data.hp; // 최대 체력 넘는지 확인
            }
            else
            {
                player.myStat.MP += item_potion.item_data.mp;
            }
            Potions._Item.Count--;
            inventory.InventReset();
            Potions.Item_Set(Potions._Item);
            //Count.SetText(Potions._Item.Count.ToString());
        }
        else
        {
            Debug.Log("아이템이 없습니다.");
        }
    }

    IEnumerator item_count_chk()
    {
        Debug.Log("시작");
        while (Potions != null)
        {
            Count.SetText(Potions._Item.Count.ToString());
            if(Potions._Item.item_data == null)
            {
                break;
            }
            yield return null;
        }
            Debug.Log("종료");
        Potions = null;
        item_img.color = orgColor;
        item_img.sprite = null;
        item_potion = default;
    }

}
