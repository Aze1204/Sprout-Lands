using Cinemachine;
using UnityEngine;
// 描述：设置摄像机边界。
// 创建者：Aze
// 创建时间：2025-01-14
public class SwitchBounds : MonoBehaviour
{
    private void OnEnable()
    {
        EventHandler.BeforeSceneUnloadEvent += SwitchConfinerShape;
        EventHandler.AfterSceneUnloadEvent += SwitchConfinerShape;
    }

    private void OnDisable()
    {
        EventHandler.BeforeSceneUnloadEvent -= SwitchConfinerShape;
        EventHandler.AfterSceneUnloadEvent -= SwitchConfinerShape;
    }

    private void SwitchConfinerShape()
    {
        PolygonCollider2D confinerShape = GameObject.FindGameObjectWithTag("BoundsConfiner").GetComponent<PolygonCollider2D>();

        CinemachineConfiner confiner = GetComponent<CinemachineConfiner>();

        confiner.m_BoundingShape2D = confinerShape;

        //清除缓存
        confiner.InvalidatePathCache();
    }
}
