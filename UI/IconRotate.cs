using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconRotate : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(-90, 0, 180);
    }
}
