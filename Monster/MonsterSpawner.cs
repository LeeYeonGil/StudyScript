using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public Transform[] SpawnPoints;
    public GameObject[] Monsters;
    public int MonsterCount;

    // Start is called before the first frame update
    void Start()
    {

    }

    IEnumerator Spawn()
    {
        while(MonsterCount>0)
        {
            GameObject mons =Instantiate( Monsters[Random.Range(0, Monsters.Length)]) as GameObject;
            mons.transform.position = SpawnPoints[Random.Range(0, SpawnPoints.Length)].position;
            MonsterCount--;
            yield return null;
        }
    }


}

