using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PanelAction : MonoBehaviour
{

    public bool m_isScale=true;
    private void OnEnable()
    {
        print("Scale:" + m_isScale);//为什么输出来的是false//最后的值是以unity
        if (m_isScale)
        {
            Transform[] child = GetComponentsInChildren<Transform>(true);
            for (int i = 1; i < child.Length; i++)
            {
                child[i].localScale = Vector3.right;
                child[i].DOScale(Vector3.one, 0.7f);
            }
        }
        else
        {
            Vector3 localPos = transform.localPosition;
            transform.localPosition = new Vector3(localPos.x + 600, localPos.y,0);
            transform.DOLocalMove(localPos, 0.7f);
        }
    }
}