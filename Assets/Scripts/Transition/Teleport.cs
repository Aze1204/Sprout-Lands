using UnityEngine;
// �����������л�������
// �����ߣ�Aze
// ����ʱ�䣺2025-01-21
public class Teleport : MonoBehaviour
{
    [SceneName]
    public string sceneToGo;
    public Vector3 positionToGo;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            EventHandler.CallTransitionEvent(sceneToGo,positionToGo);
        }
    }
}
