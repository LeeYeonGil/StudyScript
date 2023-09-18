using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Item
{
    [SerializeField] Item_Data Item_data;
    [SerializeField] float AP;
    [SerializeField] float DP;
    [SerializeField] float AS;
    [SerializeField] float SP;
    [SerializeField] int count;
    public Item_Data item_data
    {
        get => Item_data;
        set => Item_data = value;
    }
    public float AttackPoint
    {
        get => AP;
        set => AP = value;
    }
    public float DeffencePoint
    {
        get => DP;
        set => DP = value;
    }
    public float AttackSpeed
    {
        get => AS;
        set => AS = value;
    }
    public float MoveSpeed
    {
        get => SP;
        set => SP = value;
    }
    public int Count
    {
        get
        {
            //return count;
            if (count < 1)
            {
                item_data = null;
                return 0;
            }
            else
            {
                return count;
            }
        }
        set => count = value;
    }
}



