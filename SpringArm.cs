using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringArm : MonoBehaviour
{
    public LayerMask crashMask;
    public Transform myCam;
    public float LookupSpeed = 10.0f;
    public float ZoomSpeed = 3.0f;
    public float Offset = 0.5f;
    Vector3 curRot = Vector3.zero;
    public Vector2 LookupRange = new Vector2(-60.0f, 80.0f);
    public Vector2 ZoomRange = new Vector2(-8, -1);

    Vector3 camPos = Vector3.zero;
    float desireDistance = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        curRot.x = transform.localRotation.eulerAngles.x;
        curRot.y = transform.parent.localRotation.eulerAngles.y;

        camPos = myCam.localPosition;

        desireDistance = camPos.z;
    }

    // Update is called once per frame
    void Update()
    {

        curRot.x -= Input.GetAxisRaw("Mouse Y") * LookupSpeed;
        curRot.x = Mathf.Clamp(curRot.x, LookupRange.x, LookupRange.y);

        curRot.y += Input.GetAxisRaw("Mouse X") * LookupSpeed;

        transform.localRotation = Quaternion.Euler(curRot.x, 0, 0);
        transform.parent.localRotation = Quaternion.Euler(0, curRot.y, 0);


        desireDistance += Input.GetAxisRaw("Mouse ScrollWheel") * ZoomSpeed;
        desireDistance = Mathf.Clamp(desireDistance, ZoomRange.x, ZoomRange.y);

        if (Physics.Raycast(transform.position, -transform.forward, out RaycastHit hit, -camPos.z + Offset + 0.1f, crashMask))
        {
            camPos.z = -hit.distance + Offset;
        }
        else
        {
            camPos.z = Mathf.Lerp(camPos.z, desireDistance, Time.deltaTime * 3.0f);
        }
        myCam.localPosition = camPos;
    }
}