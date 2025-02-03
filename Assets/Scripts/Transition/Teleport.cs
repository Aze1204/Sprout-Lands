using UnityEngine;
// 描述：场景切换参数。
// 创建者：Aze
// 创建时间：2025-01-21
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
