using DTT.AreaOfEffectRegions;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FinTarget : MonoBehaviour
{
    public CircleRegionProjector _CRP;
    public RPGPlayer _player;
    public SphereCollider EnemyFinder;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RangeSet();
    }

    public void RangeSet()
    {
        _CRP.Radius = _player.myStat.AttackRange;
        EnemyFinder.radius = _player.myStat.AttackRange;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.layer == 9)
        {
            if(_CRP.FillProgress >= 0.0f && _CRP.FillProgress < 1.0f)
            {
                Debug.Log("발견" + _CRP.FillProgress);
                _CRP.FillProgress += 0.01f;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("놓침");
    }
}
