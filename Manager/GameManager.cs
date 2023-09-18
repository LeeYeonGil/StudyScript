using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool NewGame = true;
    public GameObject[] Stages;
    public Potal[] Potals;
    public GameObject PlayerRespawn;
    public GameObject playerRespawnPoint;
    public GameObject MonsterSpawn;
    public bool bossClearChk = false;
    public bool clearChk = false;
    public int MonsterCount;
    public int currentNum = 0;
    public GameObject ExitPoint;
    [SerializeField] Item_Data[] items;

    public int Gold;

    public bool BossStageChk = false;

    public RPGPlayer player;

    Coroutine StageStarting;

    private void Awake()
    {
        if (DataManager.Instance.loadchk)
        {
            Debug.Log("불러오기");
            DataManager.Instance.Load();
        }
    }
    public void StageStart()
    {
        if(StageStarting != null)
        {
            StopCoroutine(StageStarting);
            StageStarting = null;
        }
        StageStarting = StartCoroutine(Stage_Next());
    }
    IEnumerator Stage_Next() // 플레이어가 포탈을 이용했을때 시작
    {
        while (currentNum < Stages.Length)
        {
            if (currentNum == Stages.Length - 1)
            {
                yield return StartCoroutine(BossStage());
            }
            yield return null;
        }
    }

    IEnumerator BossStage()
    {
        BossMonster boss = Stages[Stages.Length - 1].GetComponent<STAGE>().Boss_Monster;
        UIManager.Instance.BossHp.SetActive(true);
        UIManager.Instance.BossMax.text = boss.myStat.MaxHP.ToString("0.0");
        while (boss.myState != BossMonster.STATE.Dead)
        {
            UIManager.Instance.BossNow.text = boss.myStat.HP.ToString("0.0");
            UIManager.Instance.BossHPbar.value = boss.myStat.HP / boss.myStat.MaxHP;
            yield return null;
        }
        currentNum = 0;
        UIManager.Instance.BossHp.SetActive(false);
    }

    public void GoldSet()
    {
        UIManager.Instance.Gold = Gold;
        UIManager.Instance.playerUI.Gold_Set();
    }

    public void GetItems()
    {
        for(int i = 0; i < items.Length;i++)
        {
            UIManager.Instance.Inventory.GetComponent<Inventory>().Item[i].GetComponent<Slot_Item>()._Item = makeitem(items[i]);
        }
        UIManager.Instance.Inventory.GetComponent<Inventory>().InventReset();
    }

    public Item makeitem(Item_Data data)
    {
        Item item = new Item();
        item.AttackPoint = 5.0f;
        item.DeffencePoint = 5.0f;
        item.MoveSpeed = 0.3f;
        item.AttackSpeed = 0.4f;
        item.Count = 1;
        item.item_data = data;
        return item;
    }
    public void BossHp_1()
    {
        if(Stages[Stages.Length - 1].GetComponent<STAGE>().Boss_Monster != null)
        Stages[Stages.Length - 1].GetComponent<STAGE>().Boss_Monster.myStat.HP = 1;
    }
    public void BossHp_50()
    {
        if (Stages[Stages.Length - 1].GetComponent<STAGE>().Boss_Monster != null)
            Stages[Stages.Length - 1].GetComponent<STAGE>().Boss_Monster.myStat.HP -= Stages[Stages.Length - 1].GetComponent<STAGE>().Boss_Monster.myStat.MaxHP/2.0f - 1.0f;
    }
    public void Dead()
    {
        player.ChangeDead();
    }

    public void playerRespawn()
    {
        Stages[currentNum].SetActive(false);
        currentNum = 0;
        SoundManager.Instance.ChangeMusic();
        UIManager.Instance.BossHp.SetActive(false);
        player.ChangeState(RPGPlayer.STATE.Create);
        player.myStat.HP = player.myStat.MaxHP;
        Stages[currentNum].SetActive(true);
        player.transform.position = playerRespawnPoint.transform.position;
        PlayerRespawn.SetActive(false);
    }
    public void ExitGame()
    {
        Time.timeScale = 1;
        LoadingScene.LoadScene("MainMenu");
    }

}
