using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.XR;
using UnityEngine.UI;

public class FireBall : Hit_Skill
{
    public LayerMask crashMask;
    public float MoveSpeed = 10.0f;
    public float totalDist = 0.0f;
    // Start is called before the first frame update

    void Start()
    {
        totalDist = 0.0f;
        crashMask = LayerMask.GetMask("Enemy");/*
        StartCoroutine(TargetChase());*/
    }

    /*private void Update()
    {
        Debug.Log(_myTarget);
    }*//*

    IEnumerator TargetChase()
    {
        while(_myTarget != null)
        {

            yield return null;
        }
        Destroy(gameObject);
    }*/


}
