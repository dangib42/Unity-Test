using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResizableObj : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform m_RectTransform;
    private RectTransform rectTransform => m_RectTransform ? m_RectTransform : m_RectTransform = transform.parent.GetComponent<RectTransform>();

    private Vector2 StartMousePosition { get; set; }
    private Vector2 StartTransformSize { get; set; }
    private int? DraggingPointerId { get; set; }
    private bool IsDragging => DraggingPointerId.HasValue;

    public Vector2 minSize;
    public Vector2 maxSize;

    private void OnValidate()
    {
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
    }

    private void Awake()
    {
        OnValidate();
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (IsDragging)
        {
            return;
        }
        DraggingPointerId = eventData.pointerId;
        StartMousePosition = eventData.position;
        // We want the start size of the rectangle.
        StartTransformSize = rectTransform.transform.localScale;
        //StartTransformPosition = RectTransform.anchoredPosition;

    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (!IsDragging)
        {
            return;
        }
       
        Rect parentRect = rectTransform.rect;

        Vector2 offset = eventData.position - StartMousePosition;
        Vector2 newSize = parentRect.size + offset;
        Vector2 position = (Vector2)rectTransform.position + (offset / 2);
        //rectTransform.sizeDelta = newSize;

        Vector2 sizeDelta = rectTransform.sizeDelta;

        print(offset);

        sizeDelta += new Vector2(newSize.x, -newSize.y);
        sizeDelta = new Vector2(
            Mathf.Clamp(sizeDelta.x, minSize.x, maxSize.x),
            Mathf.Clamp(sizeDelta.y, minSize.y, maxSize.y)
            );

        if(sizeDelta.x <= maxSize.x && sizeDelta.x >= minSize.x)
        {
            rectTransform.position = new Vector3(position.x, rectTransform.position.y, rectTransform.position.z);
        }
        else if (sizeDelta.y <= maxSize.y && sizeDelta.y >= minSize.y)
        {
            rectTransform.position = new Vector3(rectTransform.position.x, position.y, rectTransform.position.z);
        }

        rectTransform.sizeDelta = sizeDelta;

        //StartMousePosition = eventData.position;

    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (!IsDragging)
            return;

        DraggingPointerId = null;
    }
}
