using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainMenu : Singleton<MainMenu>
{
    public Button Startbtn;
    public Button Loadbtn;
    public Button Settingbtn;
    public Button Endbtn;

    public GameObject LoadWindow;
    [SerializeField] bool loadWindowOC = false;
    public GameObject SettingWindow;
    [SerializeField] bool settingWindowOC = false;

    public GameObject YNPop;
    [SerializeField] Button LoadGamebtn;
    [SerializeField] bool yNPopOC = false;
    public GameObject NoTouch;
    public GameObject fileEmpty;
    [SerializeField] bool fileEmptyOC = false;
    // Start is called before the first frame update
    private void Start()
    {
        LoadGamebtn.onClick.AddListener(DataManager.Instance.ClickLoad);
    }
    public void GameStart()
    {
        DataManager.Instance.loadchk = false;
        LoadingScene.LoadScene("GameScene");
    }
    public void SettingWindowOC()
    {
        settingWindowOC = !settingWindowOC;
        NoTouch.transform.SetAsLastSibling();
        NoTouch.SetActive(settingWindowOC);
        SettingWindow.transform.SetAsLastSibling();
        SettingWindow.SetActive(settingWindowOC);
    }
    public void LoadWindowOC()
    {
        loadWindowOC = !loadWindowOC;
        NoTouch.SetActive(loadWindowOC);
        NoTouch.transform.SetAsLastSibling();
        LoadWindow.SetActive(loadWindowOC);
        LoadWindow.transform.SetAsLastSibling();
    }
    public void YNPopOC()
    {
        yNPopOC = !yNPopOC;
        NoTouch.SetActive(yNPopOC);
        NoTouch.transform.SetAsLastSibling();
        YNPop.SetActive(yNPopOC);
        YNPop.transform.SetAsLastSibling();
    }
    public void FlieEmptyOC()
    {
        if (!File.Exists(DataManager.Instance.save_path + DataManager.Instance.currentfileName(DataManager.Instance.currentnum)))
        {
            fileEmptyOC = !fileEmptyOC;
            NoTouch.SetActive(fileEmptyOC);
            NoTouch.transform.SetAsLastSibling();
            fileEmpty.SetActive(fileEmptyOC);
            fileEmpty.transform.SetAsLastSibling();
        }

    }
    public void GameExit()
    {
        Application.Quit();
    }
}
