using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ui
{
    public class DragHandle : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private RectTransform m_RectTransform;
        private RectTransform RectTransform => m_RectTransform ? m_RectTransform : m_RectTransform = GetComponent<RectTransform>();
        
        private Vector2 StartMousePosition { get; set; }
        private Vector2 StartTransformPosition { get; set; }
        private int? DraggingPointerId { get; set; }
        private bool IsDragging => DraggingPointerId.HasValue;

        private void OnValidate()
        {
            RectTransform.pivot = new Vector2(0.5f, 0.5f);
        }

        private void Awake()
        {
            OnValidate();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (IsDragging)
            {
                return;
            }
            
            DraggingPointerId = eventData.pointerId;
            StartMousePosition = eventData.position;
            StartTransformPosition = RectTransform.anchoredPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!IsDragging)
            {
                return;
            }
            
            RectTransform parent = (RectTransform)RectTransform.parent;
            Rect parentRect = parent.rect;
            Rect rect = RectTransform.rect;
            
            Vector2 offset = eventData.position - StartMousePosition;
            Vector2 anchoredPosition = StartTransformPosition + offset;
            
            for (int i = 0; i < 2; i++)
            {
                float halfSize = 0.5f * (parentRect.size[i] - rect.size[i]);
                anchoredPosition[i] = Mathf.Clamp(anchoredPosition[i], -halfSize, halfSize);
            }
            
            RectTransform.anchoredPosition = anchoredPosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!IsDragging || DraggingPointerId.GetValueOrDefault() == eventData.pointerId)
            {
                return;
            }

            DraggingPointerId = null;
        }
    }
}
