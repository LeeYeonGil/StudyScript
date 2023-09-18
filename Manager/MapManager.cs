using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapManager : MonoBehaviour
{
    public static MapManager Inst = null;
    public GameObject Tile;
    public LayerMask layermask;

    [SerializeField] Vector2 mapSize = Vector2.one;
    [ContextMenu("맵생성")]
    void CreateMap()
    {        
        while(transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        for(int y = 0; y < (int)mapSize.y; ++y) // 세로
        {
            for(int x = 0; x < (int)mapSize.x; ++x) // 가로
            {
                GameObject obj = Instantiate(Tile,transform)as GameObject;
                obj.layer = layermask;
                obj.transform.localPosition = new Vector3(x, 0, y);
                obj.name = $"Tile[{x},{y}]";
            }
        }
        transform.localPosition = new Vector3(-mapSize.x / 2.0f + 0.5f, 0, -mapSize.y / 2.0f + 0.5f);
    }

    private void Awake()
    {
        Inst = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
