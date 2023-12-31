using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPool : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefeb;
    Queue<GameObject> _questPool = new Queue<GameObject>(); //오브젝트를 담을 큐
    [SerializeField] private int count;
    public static QuestPool instance = null;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            for (int i = 0; i < count; i++)
            {
                CreateObject();
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    GameObject CreateObject() //초기 OR 오브젝트 풀에 남은 오브젝트가 부족할 때, 오브젝트를 생성하기위해 호출되는 함수
    {
        GameObject newObj = Instantiate(objectPrefeb, instance.transform);
        newObj.gameObject.SetActive(false);

        return newObj;
    }
    public GameObject GetObject() //오프젝트가 필요할 때 다른 스크립트에서 호출되는 함수
    {
        if (_questPool.Count > 0) //현재 큐에 남아있는 오브젝트가 있다면,
        {
            GameObject objectInPool = _questPool.Dequeue();

            objectInPool.gameObject.SetActive(true);
            return objectInPool;
        }
        else //큐에 남아있는 오브젝트가 없을 때 새로 만들어서 사용
        {
            GameObject objectInPool = CreateObject();

            objectInPool.gameObject.SetActive(true);
            return objectInPool;
        }
    }
    public void ReturnObjectToQueue(GameObject obj) //사용이 완료 된 오브젝트를 다시 큐에 넣을때 호출 파라미터->비활성화 할 오브젝트
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(instance.transform);
        instance._questPool.Enqueue(obj); //다시 큐에 넣음
    }
}