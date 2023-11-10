using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Shop : UI_Default
{
    [SerializeField]Inventory inventory;

    public override void UI_OC()
    {
       base.UI_OC();
       inventory.gameObject.SetActive(OC);
       if (OC) UIManager.Instance.TalkBoxOC(!OC);
    }
}
