using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot_Reset : MonoBehaviour
{
    public Image skill;
    public Image Cool;
    private void OnDisable()
    {
        if (GetComponent<Slot_Item>()._Item.item_data == null)
        {
            skill.sprite = null;
            Cool.sprite = null;
        }
    }
}
