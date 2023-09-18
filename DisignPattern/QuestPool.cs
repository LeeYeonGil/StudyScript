using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPool : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefeb;
    Queue<GameObject> _questPool = new Queue<GameObject>(); //������Ʈ�� ���� ť
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
    GameObject CreateObject() //�ʱ� OR ������Ʈ Ǯ�� ���� ������Ʈ�� ������ ��, ������Ʈ�� �����ϱ����� ȣ��Ǵ� �Լ�
    {
        GameObject newObj = Instantiate(objectPrefeb, instance.transform);
        newObj.gameObject.SetActive(false);

        return newObj;
    }
    public GameObject GetObject() //������Ʈ�� �ʿ��� �� �ٸ� ��ũ��Ʈ���� ȣ��Ǵ� �Լ�
    {
        if (_questPool.Count > 0) //���� ť�� �����ִ� ������Ʈ�� �ִٸ�,
        {
            GameObject objectInPool = _questPool.Dequeue();

            objectInPool.gameObject.SetActive(true);
            return objectInPool;
        }
        else //ť�� �����ִ� ������Ʈ�� ���� �� ���� ���� ���
        {
            GameObject objectInPool = CreateObject();

            objectInPool.gameObject.SetActive(true);
            return objectInPool;
        }
    }
    public void ReturnObjectToQueue(GameObject obj) //����� �Ϸ� �� ������Ʈ�� �ٽ� ť�� ������ ȣ�� �Ķ����->��Ȱ��ȭ �� ������Ʈ
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(instance.transform);
        instance._questPool.Enqueue(obj); //�ٽ� ť�� ����
    }
}