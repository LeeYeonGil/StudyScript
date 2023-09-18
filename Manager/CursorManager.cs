using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : Singleton<CursorManager>
{
    [SerializeField] Texture2D Cursor_Arrow;
    [SerializeField] Texture2D Cursor_Attack;
    [SerializeField] Texture2D Cursor_Non_Attack_target;
    [SerializeField] LayerMask Mask;
    [SerializeField] bool mainchk = false;

    enum CursorType
    {
        None, Attack, Non_Target
    }
    [SerializeField]CursorType _cursorType = CursorType.None;
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
    // Update is called once per frame
    void Update()
    {
        UpdateMouseCursor();
    }
    void UpdateMouseCursor()
    {
        if(Input.GetKey(KeyCode.A) && !mainchk)
        {
            if (_cursorType != CursorType.Non_Target)
            {
                Cursor.SetCursor(Cursor_Non_Attack_target, new Vector2(80, 0), CursorMode.Auto);
                _cursorType = CursorType.Non_Target;
            }
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        //레이어마스크에 해당하는 오브젝트가 선택 되었는지 확인 한다.
        if (Physics.Raycast(ray, out hit, 1000.0f, Mask))
        {
            if (hit.collider.gameObject.layer == 9)
            {
                if (_cursorType != CursorType.Attack)
                {
                    Cursor.SetCursor(Cursor_Attack, new Vector2(80, 0), CursorMode.Auto);
                    _cursorType = CursorType.Attack;
                }
            }
            else
            {
                if (_cursorType != CursorType.None)
                {
                    Cursor.SetCursor(Cursor_Arrow, new Vector2(80, 0), CursorMode.Auto);
                    _cursorType = CursorType.None;
                }
            }
        }
    }
}
