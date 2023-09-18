using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManager : Singleton<CamManager>
{
    public Camera MainCam;
    public Camera BossSceneCam;
    [SerializeField]bool change = false;
    private void Awake()
    {
        MainCam = Camera.main;
    }

    public void ChangeCam()
    {
        if (!change)
        {
            BossSceneCam.depth = 2;
        }
        else
        {
            BossSceneCam.depth = -2;
        }
        change = !change;
    }
}
