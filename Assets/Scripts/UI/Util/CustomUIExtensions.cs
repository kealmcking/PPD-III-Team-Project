
using UnityEngine;
using UnityEngine.EventSystems;

public static class CustomUIExtensions
{
    public static bool IsPointerOverCanvas(RectTransform canvas,PointerEventData data)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(canvas, data.position);
    }
}
