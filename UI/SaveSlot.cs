using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SaveSlot : MonoBehaviour
{
    public selectSlot selectslot;
    public int num;
    public Color orgColor;
    public Color selectColor;
    public Image image;
    [SerializeField]Button btn;
    public bool select = false;
    public TMP_Text text;
    public Data savedata;
    // Start is called before the first frame update
    private void Awake()
    {
        DataSet();
    }
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(CurrentNum);
        savedata = findData(num);
    }

    public void CurrentNum()
    {
        DataManager.Instance.currentnum = num;
        image.color = selectColor;
        select = true;
        selectslot.Check(num);
    }
    public void NonCurrent()
    {
        image.color = orgColor;
        select = false;
    }

    public Data findData(int i)
    {
        if (i == 1)
        {
            return DataManager.Instance.save_data_1;
        }
        else if (i == 2)
        {
            return DataManager.Instance.save_data_2;
        }
        else if(i == 3)
        {
            return DataManager.Instance.save_data_3;
        }
        else
        {
            Debug.Log("데이터가 없습니다.");
            return null;
        }
    }

    public void DataSet()
    {
        if (File.Exists(DataManager.Instance.save_path + DataManager.Instance.currentfileName(num)))
        {
            string loadJson = File.ReadAllText(DataManager.Instance.save_path + DataManager.Instance.currentfileName(num));
            text.text = JsonUtility.FromJson<Data>(loadJson).date;
        }else
        {
            text.text = "비어있음";
        }
    }

}
