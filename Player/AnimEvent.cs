using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimEvent : MonoBehaviour
{
    [SerializeField] RPGPlayer player;
    public UnityEvent Skill_Weapon = default;
    public UnityEvent Skill_Helmet = default;
    public UnityEvent Skill_Armor = default;

    public void Default_Skill()
    {
        Skill_Weapon?.Invoke();
    }

    public void Helmet_Skill()
    {
        Skill_Helmet?.Invoke();
    }

    public void Armor_Skill()
    {
        Skill_Armor?.Invoke();
    }

    public void Helmet_Cast()
    {
        player.HelmetCasting = !player.HelmetCasting;
    }

    public void Armor_Cast()
    {
        player.ArmorCasting = !player.ArmorCasting;
    }

}


