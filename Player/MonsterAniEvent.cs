using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterAniEvent : MonoBehaviour
{
    public UnityEvent OnAttack;
    public UnityEvent OnSkill_1;
    // Start is called before the first frame update
    public void OnAttacking()
    {
        OnAttack?.Invoke();
    }
    public void OnSkill_1ing()
    {
        OnSkill_1?.Invoke();
    }
}
