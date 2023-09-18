using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class MoveUI : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    public GameObject _moveUI;
    Vector2 dragOffset = Vector2.zero;
    /* Vector2 halfSize = Vector2.zero;*/
    // Start is called before the first frame update
    public void OnBeginDrag(PointerEventData eventData)
    {
        dragOffset = (Vector2)_moveUI.transform.position - eventData.position;
        _moveUI.transform.SetAsLastSibling();

    }
    public void OnDrag(PointerEventData eventData)
    {
        _moveUI.transform.position = eventData.position + dragOffset;
    }

    
}
