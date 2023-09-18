using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    public Dictionary<string, Queue<GameObject>> myPool = new Dictionary<string, Queue<GameObject>>();
    [SerializeField] protected int Count;
    [SerializeField] protected GameObject objectPrefeb;

    
    public GameObject CreateObject()
    {
        GameObject newObj = Instantiate(objectPrefeb, transform);
        newObj.gameObject.SetActive(false);
        return newObj;
    }

    public T GetObject<T>(GameObject org, Vector3 pos, Quaternion rot)
    {
        string Name = typeof(T).ToString();
        if (myPool.ContainsKey(Name))
        {
            if (myPool[Name].Count > 0)
            {
                GameObject obj = myPool[Name].Dequeue();
                obj.SetActive(true);
                obj.transform.SetParent(null);
                obj.transform.position = pos;
                obj.transform.rotation = rot;
                return obj.GetComponent<T>();
            }
            else
            {
                GameObject obj = CreateObject();
                obj.gameObject.SetActive(true);
                return obj.GetComponent<T>();
            }
        }
        else
        {
            myPool[Name] = new Queue<GameObject>();
        }
        return Instantiate(org, pos, rot).GetComponent<T>();
    }

    public void ReleaseObject<T>(GameObject obj)
    {
        obj.transform.SetParent(transform);
        obj.SetActive(false);
        Debug.Log(typeof(T).ToString());
        myPool[typeof(T).ToString()].Enqueue(obj);
    }
    public void startPool<T>(int count)
    {
       /* for (int i = 0; i < count; i++)
        {
            myPool.Add(typeof(T).ToString(), new Queue<GameObject>());*/

            for (int j = 0; j < count; j++)
            {
                myPool[typeof(T).ToString()].Enqueue(CreateObject());
            }
        //}
    }

}
