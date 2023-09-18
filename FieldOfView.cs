using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public MeshFilter meshFilter;
    public float ViewAngle = 90.0f;
    public float ViewDistance;
    public int DetailCount = 100;
    public LayerMask crashMask;
    public RPGPlayer _Player;
    public MeshRenderer _myMeshRenderer;
    public SphereCollider EnemyFinder;

    List<Vector3> dirList = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        Vector3[] vb = new Vector3[DetailCount + 1];
        int[] ib = new int[(DetailCount - 1) * 3];
        ViewDistance = _Player.myStat.AttackRange;
        EnemyFinder.radius = _Player.myStat.AttackRange;

        vb[0] = Vector3.zero;
        Vector3 dir = Quaternion.Euler(0, -ViewAngle / 2.0f, 0) * new Vector3(0,0,1) * ViewDistance;        

        float gap = ViewAngle / (float)(DetailCount - 1);
        for (int i = 1; i < vb.Length; ++i)
        {
            dirList.Add(dir.normalized);
            vb[i] = vb[0] + dir;
            dir = Quaternion.Euler(0, gap, 0) * dir;
            
        }

        for(int i = 0, v = 1; i < ib.Length; i += 3, ++v)
        {
            ib[i] = 0;
            ib[i + 1] = v;
            ib[i + 2] = v + 1;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vb;
        mesh.triangles = ib;
        meshFilter.mesh = mesh;
    }

    private void FixedUpdate()
    {
        Vector3[] vb = meshFilter.mesh.vertices;
        //레이캐스트를 통해 벽이랑 부딪히는 지점이 있다면 버텍스의 위치를 바꾼다.
        for(int i = 0; i < dirList.Count; ++i)
        {
            Ray ray = new Ray(transform.position, transform.rotation * dirList[i]);
            if(Physics.Raycast(ray, out RaycastHit hit,ViewDistance, crashMask))
            {
                vb[i + 1] = vb[0] + dirList[i] * hit.distance;
            }
            else
            {
                vb[i + 1] = vb[0] + dirList[i] * ViewDistance;
            }

        }
        meshFilter.mesh.vertices = vb;
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("발견");
        if(other.transform.gameObject.layer == 9)
        {
            _myMeshRenderer.materials[0].color = new Color(255 / 255f, 0 / 255f, 0 / 255f, 3 / 255f);
        }
        else
        {
            _myMeshRenderer.materials[0].color = new Color(48 / 255f, 255 / 255f, 0 / 255f, 3 / 255f);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        _myMeshRenderer.materials[0].color = new Color(48 / 255f, 255 / 255f, 0 / 255f, 3 / 255f);
    }
}
