using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIPerception : MonoBehaviour
{
    public UnityEvent<Transform> FindTarget = default;
    public UnityEvent LostTarget = default;
    public LayerMask enemyMask = default;
    public Transform myTarget = null;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if (myTarget != null) return;
        if (other.gameObject.layer == 6)
        {
            //타겟을 처음 발견했을때
            if (other.transform.GetComponent<IBattle>().IsLive())
            {
                myTarget = other.transform;
                FindTarget?.Invoke(myTarget);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (myTarget != null) return;
        if (enemyMask == other.gameObject.layer)
        {
            myTarget = other.transform;
            FindTarget?.Invoke(myTarget);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if(myTarget == other.transform)
        {
            myTarget = null;
            LostTarget?.Invoke();
        }
    }
}
