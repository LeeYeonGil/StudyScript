using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : Singleton<ScreenManager>
{
    FullScreenMode screenMode;
    public TMP_Dropdown dropdown;
    public Toggle windowMode;
    public List<Resolution> resolutions = new List<Resolution>();
    [SerializeField]int resolutionNum;
    // Start is called before the first frame update
    void Start()
    {
        initScreen();
        windowMode.isOn = false;
    }

    void initScreen()
    {
        for (int i = 0; i<Screen.resolutions.Length;i++)
        {
            if (Screen.resolutions[i].refreshRate == 60)
                resolutions.Add(Screen.resolutions[i]);
        }
        resolutions.AddRange(Screen.resolutions);
        dropdown.options.Clear();

        int optionNum = 0;
        foreach(Resolution resolution in resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = resolution.width + "x" + resolution.height + " " + resolution.refreshRate + "hz";
            dropdown.options.Add(option);

            if (resolution.width == Screen.width && resolution.height == Screen.height) dropdown.value = optionNum;
            optionNum++;
        }
        dropdown.RefreshShownValue();

        windowMode.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;    
    }

    public void OptionChange(int i)
    {
        resolutionNum = i;
    }

    public void SetScreen()
    {
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode);
    }

    public void FullScreen(bool chk)
    {
        screenMode = chk ? FullScreenMode.Windowed :  FullScreenMode.FullScreenWindow;
    }
}
