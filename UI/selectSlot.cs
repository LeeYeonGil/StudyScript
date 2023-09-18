using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class selectSlot : MonoBehaviour
{
    [SerializeField] SaveSlot slot_1;
    [SerializeField] SaveSlot slot_2;
    [SerializeField] SaveSlot slot_3;

    private void Awake()
    {
        slot_1.selectslot = this;
        slot_2.selectslot = this;
        slot_3.selectslot = this;
        slot_1.savedata = DataManager.Instance.save_data_1;
        slot_2.savedata = DataManager.Instance.save_data_2;
        slot_3.savedata = DataManager.Instance.save_data_3;
    }

    public void Check(int i)
    {
        if (i == 1)
        {
            if (slot_2.select == true) slot_2.NonCurrent();
            if (slot_3.select == true) slot_3.NonCurrent();
        }
        else if (i == 2)
        {
            if (slot_1.select == true) slot_1.NonCurrent();
            if (slot_3.select == true) slot_3.NonCurrent();
        }
        else if (i == 3)
        {
            if (slot_2.select == true) slot_2.NonCurrent();
            if (slot_1.select == true) slot_1.NonCurrent();
        }
        else
        {
            slot_1.NonCurrent();
            slot_2.NonCurrent();
            slot_3.NonCurrent();
        }
    }

}
