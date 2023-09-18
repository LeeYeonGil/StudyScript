using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField]Inventory inventory;

    private void OnEnable()
    {
        inventory.OC = true;
        inventory.gameObject.SetActive(true);
    }
    private void OnDisable()
    {
        inventory.OC = false;
        inventory.gameObject.SetActive(false);
    }
}
