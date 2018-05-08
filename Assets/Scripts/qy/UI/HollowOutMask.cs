using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HollowOutMask : MaskableGraphic, ICanvasRaycastFilter
{
    public RectTransform target;
    public bool isRaycast = true;

    private Vector2 targetMin;
    private Vector2 targetMax;
    public void Update()
    {
        RefreshView();
    }
    private void RefreshView()
    {
        Vector2 newMin;
        Vector2 newMax;
        if (target != null && target.gameObject.activeSelf)
        {
            GetTargetMinMax(out newMin, out newMax, target);
        }
        else
        {
            newMin = Vector2.zero;
            newMax = Vector2.zero;
        }
        if (targetMin != newMin || targetMax != newMax)
        {
            targetMin = newMin;
            targetMax = newMax;
            SetAllDirty();
        }
    }

    private void GetTargetMinMax(out Vector2 min, out Vector2 max, RectTransform targetTF)
    {
        //min = targetTF.offsetMin;
        //max = targetTF.offsetMax;
        Bounds bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(transform, targetTF);
        min = bounds.min;
        max = bounds.max;
    }

    bool ICanvasRaycastFilter.IsRaycastLocationValid(Vector2 screenPos, Camera eventCamera)
    {

        if (null == target) return true; // 将目标对象范围内的事件镂空（使其穿过） 
        if (!isRaycast) return true;
        return !RectTransformUtility.RectangleContainsScreenPoint(target, screenPos, eventCamera);
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        Debug.Log("--");

        Vector2 selfPiovt = rectTransform.pivot;
        Rect selfRect = rectTransform.rect;

        Vector2 outMin = new Vector2(-selfPiovt.x * selfRect.width, -selfPiovt.y * selfRect.height);
        Vector2 outMax = new Vector2((1 - selfPiovt.x) * selfRect.width, (1 - selfPiovt.y) * selfRect.height);
        Vector2 inMin = targetMin;
        Vector2 inMax = targetMax;
        vh.Clear();
        //添加四个顶点  
        vh.AddVert(new Vector3(outMax.x, outMax.y), color, Vector2.zero);
        vh.AddVert(new Vector3(outMax.x, outMin.y), color, Vector2.zero);
        vh.AddVert(new Vector3(outMin.x, outMin.y), color, Vector2.zero);
        vh.AddVert(new Vector3(outMin.x, outMax.y), color, Vector2.zero);

        vh.AddVert(new Vector3(inMax.x, inMax.y), color, Vector2.zero);
        vh.AddVert(new Vector3(inMax.x, inMin.y), color, Vector2.zero);
        vh.AddVert(new Vector3(inMin.x, inMin.y), color, Vector2.zero);
        vh.AddVert(new Vector3(inMin.x, inMax.y), color, Vector2.zero);
        //添加两个三角形  
        vh.AddTriangle(4, 0, 1);
        vh.AddTriangle(4, 1, 5);
        vh.AddTriangle(5, 1, 2);
        vh.AddTriangle(5, 2, 6);
        vh.AddTriangle(6, 2, 3);
        vh.AddTriangle(6, 3, 7);
        vh.AddTriangle(7, 3, 0);
        vh.AddTriangle(7, 0, 4);

    }
}
