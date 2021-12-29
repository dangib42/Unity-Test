using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ResizePanel : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
{

    public Vector2 minSize;
    public Vector2 maxSize;

    private RectTransform rectTransform;
    private Vector2 currentPointerPosition;
    private Vector2 previousPointerPosition;

    public SideVal curSV = SideVal.TopRight;
    private Vector2 StartMousePosition { get; set; }
    private Vector2 StartTransformPosition { get; set; }

    void Awake()
    {
        rectTransform = transform.parent.GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        rectTransform.SetAsLastSibling();
        StartMousePosition = eventData.position;
        StartTransformPosition = rectTransform.anchoredPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out previousPointerPosition);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (rectTransform == null)
            return;

        Vector2 sizeDelta = rectTransform.sizeDelta;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out currentPointerPosition);
        Vector2 resizeValue = currentPointerPosition - previousPointerPosition;


        if (curSV == SideVal.TopLeft)
        {
            resizeValue.x *= -1;
        }
        else if (curSV == SideVal.Left)
        {
            resizeValue *= -1;
        }
        else if (curSV == SideVal.BottomLeft)
        {
            resizeValue *= -1;
        }
        else if (curSV == SideVal.Bottom)
        {
            resizeValue *= -1;
        }
        else if(curSV == SideVal.BottomRight)
        {
            resizeValue.y *= -1;
        }

        if (curSV == SideVal.Right || curSV == SideVal.Left)
            resizeValue.y = 0;
        else if (curSV == SideVal.Bottom || curSV == SideVal.Top)
            resizeValue.x = 0;

        sizeDelta += new Vector2(resizeValue.x, resizeValue.y);
        
        sizeDelta = new Vector2(
                Mathf.Clamp(sizeDelta.x, minSize.x, maxSize.x),
                Mathf.Clamp(sizeDelta.y, minSize.y, maxSize.y)
                );

        rectTransform.sizeDelta = sizeDelta;
        previousPointerPosition = currentPointerPosition;

       
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
    }

    public enum SideVal
    {
        TopRight,
        TopLeft,
        BottomRight,
        BottomLeft,
        Top,
        Bottom,
        Right,
        Left
    }
}