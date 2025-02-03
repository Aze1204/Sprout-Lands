using Cinemachine;
using UnityEngine;
// ����������������߽硣
// �����ߣ�Aze
// ����ʱ�䣺2025-01-14
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

        //�������
        confiner.InvalidatePathCache();
    }
}
