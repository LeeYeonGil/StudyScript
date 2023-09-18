using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Base_Monster;

public class Hit_Skill : MonoBehaviour
{
    /*public LayerMask crashMask;
    public float MoveSpeed = 10.0f;
    public float totalDist = 0.0f;
    public GameObject _myTarget;*/
    public GameObject Caster;
    public Transform _spellPoint;
    public float _Damage;
    public float _Damage_Increase;
    public GameObject _myTarget;
    public bool bosschk = false;
    //ต๐น๖วม
    public DeBuff.Type debuff;
    public float debuffvalue = 1.0f;
    public float debuffTime = 1.0f;
    public bool debuffUse = false;
    public int debuffPer;
    // Start is called before the first frame update


}
