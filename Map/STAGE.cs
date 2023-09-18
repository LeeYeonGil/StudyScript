using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class STAGE : MonoBehaviour
{
    public Transform[] SpawnPoints;
    public GameObject[] Monsters;
    public Transform monstr;
    public int MonsterCount;
    public int SpawnCount;
    public int AllSpawn = 0;
    public bool clear = false;
    public bool startpoint = false;
    public bool bossChk = false;
    public Potal myPotal;
    public Chest chest;

    public GameObject[] MonsterPotal;
    [SerializeField]public Queue<GameObject> monsterPool = new Queue<GameObject>();
    [SerializeField] int MonsterNameCount = 0;

    public List<GameObject> monsterlist = new List<GameObject>();
    public BossMonster Boss_Monster;
    Coroutine spawn;
    // Start is called before the first frame update
    private void OnDisable()
    {
        if (chest != null)
        {
            chest.gameObject.SetActive(false);
        }
        if (monsterlist.Count > 0)
        {
            for (int i = 0; i < monsterlist.Count; i++)
            {
                if(!bossChk)
                monsterlist[i].GetComponent<Monster>().HPbar_text_Release();
                ReleaseMonster(monsterlist[i]);
            }
        }
        myPotal.Potal_reset();

    }

    private void Awake()
    {
        for (int i = 0; i < MonsterCount; i++)
        {
            GameObject obj = Instantiate(Monsters[Random.Range(0, Monsters.Length)], monstr) as GameObject;
            obj.name = "Monster_" + MonsterNameCount++;
            monsterPool.Enqueue(obj);
            obj.SetActive(false);
            if (!bossChk)
            {
                obj.GetComponent<Monster>().mystage = this;
            }
            else
            {
                obj.GetComponent<BossMonster>().mystage = this;
            }
        }
        myPotal.curStage = gameObject;
        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            SpawnPoints[i].gameObject.SetActive(false);
        }
    }
    public void Start_Stage()
    {
        if (startpoint) return;
        SpawnCount = MonsterCount;
        if (spawn != null)
        {
            StopCoroutine(spawn);
            spawn = null;
        }
        spawn = bossChk? StartCoroutine(BossSpawn()) : StartCoroutine(Spawn());
    }

    private void Update()
    {
        if(clear)
        {
            StopAllCoroutines();
        }
    }

    IEnumerator Spawn()
    {
        while (SpawnCount > 0)
        {
            int i = Random.Range(0, SpawnPoints.Length);
            SpawnPoints[i].gameObject.SetActive(true);
            MonsterPotal[0].SetActive(true);
            yield return new WaitForSeconds(1.0f);
            MonsterPotal[0].SetActive(false);
            MonsterPotal[1].SetActive(true);
            Monster mons = GetMonster().GetComponent<Monster>();
            mons.ResetMonster();
            mons.Change_Mons_State(Monster.STATE.reCreate);
            mons.transform.position = SpawnPoints[i].position;
            monsterlist.Add(mons.gameObject);
            SpawnCount--;
            yield return new WaitForSeconds(1.0f);
            MonsterPotal[1].SetActive(false);
            MonsterPotal[2].SetActive(true);
            yield return new WaitForSeconds(1.0f);
            MonsterPotal[2].SetActive(false);
            yield return new WaitForSeconds(5.0f);
        }
        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            SpawnPoints[i].gameObject.SetActive(false);
        }
        StartCoroutine(ClearChk());
    }

    IEnumerator BossSpawn()
    {
        while (SpawnCount > 0)
        {
            int i = Random.Range(0, SpawnPoints.Length);
            SpawnPoints[i].gameObject.SetActive(true);
            BossMonster mons = GetMonster().GetComponent<BossMonster>();
            mons.ResetMonster();
            Boss_Monster = mons;
            CamManager.Instance.ChangeCam();
            gameObject.GetComponent<PlayableDirector>().Play();
            mons.Change_Mons_State(BossMonster.STATE.reCreate);
            mons.transform.position = SpawnPoints[i].position;
            monsterlist.Add(mons.gameObject);
            SpawnCount--;
        }
        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            SpawnPoints[i].gameObject.SetActive(false);
        }
        yield return StartCoroutine(ClearChk());
    }

    IEnumerator ClearChk() // 너무 빨리 잡으면 체크 불가
    {
        int count = monsterlist.Count;

        while (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                if (bossChk ? monsterlist[i].GetComponent<BossMonster>().myState == BossMonster.STATE.Dead : monsterlist[i].GetComponent<Monster>().myState == Monster.STATE.Dead)
                {
                    monsterlist.RemoveAt(i);
                    count--;
                }
            }
            yield return null;
        }
        clear = true;
        if (bossChk && clear) GameManager.Instance.bossClearChk = true; 
        UIManager.Instance.StartStage("Clear");
        chest.gameObject.SetActive(true);
        chest.ChestReset();
        myPotal.nextStage.SetActive(true);
        myPotal.Potal_Active();
    }

    public GameObject GetMonster()
    {
        GameObject mons = monsterPool.Dequeue();
        mons.SetActive(true);
        return mons;
    }
    public void ReleaseMonster(GameObject mons)
    {
        mons.SetActive(false);
        mons.transform.position = Vector3.zero;
        
        monsterPool.Enqueue(mons);
    }

}
