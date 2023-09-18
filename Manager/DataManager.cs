using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Unity.VisualScripting;

public class Data
{
    public string date;

    public CharacterStat playerstat;

    public int Gold;

    public Item helmet;
    public Item armor;
    public Item shoes;
    public Item weapon;

    public List<Item> inventory = new List<Item>();
}
public class DataManager : Singleton<DataManager>
{
    public Data save_data_1 = new Data();
    public Data save_data_2 = new Data();
    public Data save_data_3 = new Data();

    public Data Select_data = new Data();

    public int currentnum;

    public string save_path;
    public string save_name_1 = "/SaveFile_1.txt";
    public string save_name_2 = "/SaveFile_2.txt";
    public string save_name_3 = "/SaveFile_3.txt";
    int index_num;

    RPGPlayer player;
    Inventory inventory;
    Equipment equipment;

    public bool loadchk = false;
    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    // Start is called before the first frame update

    void Start()
    {
        save_path = Application.dataPath + $"/SavePath/";
        if (!Directory.Exists(save_path)) // 해당 경로가 존재하지 않는다면
        {
            Directory.CreateDirectory(save_path); // 폴더 생성
        }
    }

    public void Save()
    {
        if (GameManager.Instance.currentNum != 0)
        {
            UIManager.Instance.NoticeOC();
            return; // 시작지점에서만 저장 가능 
        }
        Data data;
        player = GameManager.Instance.player;
        inventory = UIManager.Instance.Inventory.GetComponent<Inventory>();
        equipment = UIManager.Instance.Equipment.GetComponent<Equipment>();
        data = currentData(currentnum);
        if (data == null)
        {
            Debug.Log("선택한 데이터가 없습니다.");
            return;
        }

        data.date = DateTime.Now.ToString(("yyyy-MM-dd HH:mm:ss tt"));
        data.playerstat = player.orgstat;
        data.weapon = equipment.WeaponSlot.GetComponent<Slot_Item>()._Item;
        data.armor = equipment.ArmorSlot.GetComponent<Slot_Item>()._Item;
        data.helmet = equipment.HelmetSlot.GetComponent<Slot_Item>()._Item;
        data.shoes = equipment.ShoesSlot.GetComponent<Slot_Item>()._Item;
        data.inventory = inventory.items_data;
        data.Gold = GameManager.Instance.Gold;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(save_path + currentfileName(currentnum), json);
        UIManager.Instance.slots[currentnum - 1].DataSet();
    }
    public void ClickLoad()
    {
        //Select_data = currentData(currentnum);
        if (File.Exists(save_path + currentfileName(currentnum)))
        {
            string loadJson = File.ReadAllText(save_path + currentfileName(currentnum));
            if (loadJson == null) return;
            loadchk = true;
            Select_data = JsonUtility.FromJson<Data>(loadJson);
            LoadingScene.LoadScene("GameScene");
        }
    }
    public void Load()
    {
        if (File.Exists(save_path + currentfileName(currentnum)))
        {
            player = GameManager.Instance.player;
            inventory = UIManager.Instance.Inventory.GetComponent<Inventory>();
            equipment = UIManager.Instance.Equipment.GetComponent<Equipment>();
            Debug.Log("불러오기");
            if (Select_data == null)
            {
                Debug.Log("선택한 데이터가 없습니다.");
                return;
            }
            player.orgstat = Select_data.playerstat;
            equipment.WeaponSlot.GetComponent<Slot_Item>().Item_Set(Select_data.weapon);
            equipment.ArmorSlot.GetComponent<Slot_Item>().Item_Set(Select_data.armor);
            equipment.HelmetSlot.GetComponent<Slot_Item>().Item_Set(Select_data.helmet);
            equipment.ShoesSlot.GetComponent<Slot_Item>().Item_Set(Select_data.shoes);
            for (int i = 0; i<inventory.Item.Count; i++)
                inventory.Item[i].GetComponent<Slot_Item>()._Item = Select_data.inventory[i];
            inventory.InventReset();
            GameManager.Instance.Gold = Select_data.Gold;
            GameManager.Instance.NewGame = false;
        }
        else
        {
            Debug.Log("세이브 파일이 없습니다.");
        }
    }

    public Data currentData(int i)
    {
        if (i == 1)
        {
            return save_data_1;
        }
        else if (i == 2)
        {
            return save_data_2;
        }
        else if (i == 3)
        {
            return save_data_3;
        }
        else
        {
            return null;
        }
    }

    public string currentfileName(int i)
    {
        if (i == 1)
        {
            return save_name_1;
        }
        else if (i == 2)
        {
            return save_name_2;
        }
        else if (i == 3)
        {
            return save_name_3;
        }
        else
        {
            return null;
        }
    }
}

