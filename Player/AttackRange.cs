using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackRange : MonoBehaviour
{
    public RPGPlayer player;
    public SphereCollider Range;
    public Projector Projector;
    public Color orgColor;
    public Color FindColor;
    // Start is called before the first frame update
    void Start()
    {
        Range = GetComponent<SphereCollider>();
        Projector = GetComponent<Projector>();
        orgColor = Projector.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        Range.radius = player.myStat.AttackRange;
        Projector.orthographicSize = player.myStat.AttackRange;
    }

   
    private void OnTriggerEnter(Collider other)
    {
        if (player.myTarget != null) return;
        IBattle target;
        if (other.transform.gameObject.layer == 9)
        {
            if (other.transform.GetComponent<IBattle>() == null)
                target = other.transform.parent.GetComponent<IBattle>();
            else
                target = other.transform.GetComponent<IBattle>();

            //타겟을 처음 발견했을때
            if (target.IsLive())
            {
                player.myTarget = other.transform;
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.transform.gameObject.layer == 9)
        {
            Projector.material.color = FindColor;
        }
        if (player.myTarget != null) return;
        if (other.transform.gameObject.layer == 9)
        {
            player.myTarget = other.transform;
            Projector.material.color = FindColor;
            return;
        }
        Projector.material.color = orgColor;
    }
    private void OnTriggerExit(Collider other)
    {
        Projector.material.color = orgColor;
        if (player.myTarget == other.transform)
        {
            player.myTarget = null;
        }
    }

}
