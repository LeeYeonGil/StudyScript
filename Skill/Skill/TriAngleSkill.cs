using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Monster;

public class TriAngleSkill : Hit_Skill
{
    public float destroyTime = 0;
    public GameObject[] collisions;
    public Collider[] Enemys;
    public List<GameObject> gameObjects = new List<GameObject> ();

    // Start is called before the first frame update
    public void Start()
    {
        Enemys = Physics.OverlapSphere(transform.position, 9f);
        TriAngle_Skill();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, EulerToVector(60.0f / 2) * 9.0f);
        Debug.DrawRay(transform.position, EulerToVector(-60.0f / 2) * 9.0f);
        destroyTime += Time.deltaTime;
        if (destroyTime >= 1.0f)
        {
            Destroy(gameObject);
        }
    }

    Vector3 EulerToVector(float i)
    {
        i += transform.eulerAngles.y;
        i *= Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(i), 0, Mathf.Cos(i));
    }
    public void TriAngle_Skill()
    {
        Enemys = Physics.OverlapSphere(transform.position, 8f);
        gameObjects.Clear();
        float RadianRange = Mathf.Cos((60.0f / 2) * Mathf.Deg2Rad);
        Debug.Log(Enemys.Length);
        for (int i = 0; i < Enemys.Length; i++) 
        {
            float targetRadian = Vector3.Dot(transform.forward, (Enemys[i].transform.position - transform.position).normalized);
            if (targetRadian > RadianRange)
            {
                Debug.DrawLine(transform.position, Vector3.forward * 9.0f, Color.green);
                if (Enemys[i].gameObject.layer == 9)
                {
                    gameObjects.Add(Enemys[i].transform.gameObject);
                    if (Enemys[i].gameObject?.GetComponent<Monster>().myState != Monster.STATE.Dead) // 플레이어
                    {
                        if (debuffUse)
                        {
                            int rand = Random.Range(0, 101);
                            if (rand <= debuffPer)
                            {
                                Enemys[i].GetComponent<Monster>().AddDebuff(debuff, debuffvalue, debuffTime, Base_Monster.STATE.Roaming);
                            }
                        }
                        Enemys[i].gameObject.GetComponent<IBattle>()?.OnDamage(_Damage * _Damage_Increase, Caster);
                        gameObjects.Add(Enemys[i].transform.gameObject);
                    }
                }
                else if(Enemys[i].gameObject.layer == 6) // 보스
                {
                    gameObjects.Add(Enemys[i].transform.gameObject);
                    if (Enemys[i].gameObject?.GetComponent<RPGPlayer>().myState != RPGPlayer.STATE.Death)
                    {
                        Enemys[i].gameObject.GetComponent<IBattle>()?.OnDamage(_Damage * _Damage_Increase, Caster);
                        Debug.Log(_Damage * _Damage_Increase + "데미지");
                        gameObjects.Add(Enemys[i].transform.gameObject);
                        break;
                    }
                }
                Debug.DrawLine(transform.position, Enemys[i].transform.position, Color.red);
            }
        }
        if (!Enemys[0].gameObject.GetComponent<RPGPlayer>()) return;
        for(int i = 0; i < gameObjects.Count; i++)
        {
            Caster.GetComponent<RPGPlayer>().myTarget = gameObjects[i].transform;
        }
        
    }
}
