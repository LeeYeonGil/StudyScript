using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    public Transform myTarget;
    Vector3 myDir = Vector3.zero;
    float targetDist = 0.0f;
    float dist = 0.0f;
    public Vector2 ZoomRange;
    public float Height = 1.0f;
    public float ZoomSpeed = 2.0f;
    public bool miniMap = false;

    public Vector2 Xpos;
    public Vector2 Zpos;
    // Start is called before the first frame update
    void Start()
    {
        myDir = transform.position - myTarget.position;
        targetDist = dist = myDir.magnitude;
        myDir.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        if (!miniMap)
        {
            targetDist += -Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
            targetDist = Mathf.Clamp(targetDist, ZoomRange.x, ZoomRange.y);

            dist = Mathf.Lerp(dist, targetDist, Time.deltaTime * 5.0f);

           /* float x = Mathf.Clamp(transform.position.x, Xpos.x, Xpos.y); // 가로 제한 값
            float z = Mathf.Clamp(transform.position.z, Zpos.x, Zpos.y); // 세로 제한 값

            transform.position = new Vector3(x, transform.position.y, z); // 가로,세로 제한 적용*/
        }
        transform.position = myTarget.position + myDir * dist + Vector3.up * Height;
    }
}
