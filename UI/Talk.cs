using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talk : MonoBehaviour
{
    public RPGPlayer Player;
    public GameObject talker;
    // Start is called before the first frame update

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == 10 || other.gameObject.layer == 19)
        {
            Player.talkchk = true;
            talker = other.gameObject;
            Player.talker = talker.transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Player.talker = null;
        Player.talkchk = false;
    }


}
