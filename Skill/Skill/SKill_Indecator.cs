using DTT.AreaOfEffectRegions;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SKill_Indecator : MonoBehaviour
{
    [SerializeField] ArcRegionProjector Arc;
    [SerializeField] CircleRegionProjector Circle;
    public LayerMask mousePos;
    public bool arcCheck = false;
    public bool stay = false;
    [SerializeField] Transform Playerpos;
    
    // Start is called before the first frame update
    void Start()
    {
       /* if (Arc != null)
        {
            if (!gameObject.GetComponent<ArcRegionProjector>())
                Circle = gameObject.GetComponent<CircleRegionProjector>();
            else
                Arc = gameObject.GetComponent<ArcRegionProjector>();
        }
        if (Circle != null)
        {
            if (!gameObject.GetComponent<CircleRegionProjector>())
                Arc = gameObject.GetComponent<ArcRegionProjector>();
            else
                Circle = gameObject.GetComponent<CircleRegionProjector>();
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (arcCheck)
            ArcChase();
        else
            Circle_indecator();
    }

    private void ArcChase() // 스킬 범위 값 적용
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        //레이어마스크에 해당하는 오브젝트가 선택 되었는지 확인 한다.
        if (Physics.Raycast(ray, out hit, 1000.0f, mousePos))
        {
            Vector3 dirVec = (hit.point - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, dirVec);
            //Angle = Mathf.Floor(Angle);
            float rotDir = 1.0f;
            if (Vector3.Dot(transform.right, dirVec) < 0.0f)
            {
                rotDir = -rotDir;
            }

            if (Mathf.Approximately(angle, 0.0f)) return;
            float delta = 10.0f;
            if (delta > angle)
            {
                delta = angle;
            }
            angle -= delta;
            transform.Rotate(Vector3.up * rotDir * angle, Space.World);
            //Debug.Log("delta :" + delta);
        }
    }

    void Circle_indecator()
    {
        if (!stay)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //레이어마스크에 해당하는 오브젝트가 선택 되었는지 확인 한다.
            if (Physics.Raycast(ray, out hit, 1000.0f, mousePos))
            {
                transform.position = new Vector3(hit.point.x, 0.1f, hit.point.z);
            }
        }
        else
        {
            transform.position = new Vector3(Playerpos.position.x, 0.1f, Playerpos.position.z);
        }
    }
    
}
