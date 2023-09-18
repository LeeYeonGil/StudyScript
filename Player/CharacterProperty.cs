using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterProperty : MonoBehaviour
{
    public CharacterStat myStat;
    Animator _anim = null;
    public Animator myAnim;

    Rigidbody _rigid = null;
    protected Rigidbody myRigid
    {
        get
        {
            if(_rigid == null)
            {
                _rigid = GetComponent<Rigidbody>();
            }
            return _rigid;
        }
    }
}
