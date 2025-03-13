using Event;
using UnityEngine;

namespace Transition
{
    public class Teleport : MonoBehaviour
    {
        [SceneName]
        public string sceneToGo;
        public Vector3 positionToGo;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                SceneEvent.CallTransitionEvent(sceneToGo,positionToGo);
            }
        }
    }
}
