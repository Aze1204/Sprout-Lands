using Cinemachine;
using Event;
using UnityEngine;

public class SwitchBounds : MonoBehaviour
{
    private void OnEnable()
    {
        SceneEvent.BeforeSceneUnloadEvent += SwitchConfinerShape;
        SceneEvent.AfterSceneUnloadEvent += SwitchConfinerShape;
    }

    private void OnDisable()
    {
        SceneEvent.BeforeSceneUnloadEvent -= SwitchConfinerShape;
        SceneEvent.AfterSceneUnloadEvent -= SwitchConfinerShape;
    }

    private void SwitchConfinerShape()
    {
        PolygonCollider2D confinerShape = GameObject.FindGameObjectWithTag("BoundsConfiner").GetComponent<PolygonCollider2D>();

        CinemachineConfiner confiner = GetComponent<CinemachineConfiner>();

        confiner.m_BoundingShape2D = confinerShape;
        
        confiner.InvalidatePathCache();
    }
}
