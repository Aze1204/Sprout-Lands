using System.Collections;
using Event;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Transition
{
    public class TransitionManager : MonoBehaviour
    {
        [SceneName]
        public string startSceneName = string.Empty;
        private CanvasGroup fadeCanvasGroup;
        private bool isFade;

        private void OnEnable()
        {
            SceneEvent.TransitionEvent += OnTransitionEvent;
        }

        private void OnDisable()
        {
            SceneEvent.TransitionEvent -= OnTransitionEvent;
        }

        private IEnumerator Start()
        {
            fadeCanvasGroup = FindObjectOfType<CanvasGroup>();
            yield return StartCoroutine(LoadSceneSetActive(startSceneName));
            SceneEvent.CallAfterSceneUnloadEvent();
        }

        private void OnTransitionEvent(string sceneToGo, Vector3 positionToGo)
        {
            if (!isFade)
                StartCoroutine(Transition(sceneToGo,positionToGo));
        }
        
        private IEnumerator Transition(string sceneName, Vector3 targetPosition)
        {
            SceneEvent.CallBeforeSceneUnloadEvent();
            yield return Fade(1);
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            yield return LoadSceneSetActive(sceneName);
            SceneEvent.CallMoveToPosition(targetPosition);
            SceneEvent.CallAfterSceneUnloadEvent();
            yield return Fade(0);
        }
        
        private IEnumerator LoadSceneSetActive(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
            SceneManager.SetActiveScene(newScene);
        }
        
        private IEnumerator Fade(float targetAlpha)
        {
            isFade = true;

            fadeCanvasGroup.blocksRaycasts = true;
            float speed = Mathf.Abs(fadeCanvasGroup.alpha-targetAlpha)/Settings.fadeDuration;

            while (!Mathf.Approximately(fadeCanvasGroup.alpha,targetAlpha))
            {
                fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha,targetAlpha,speed*Time.deltaTime);
                yield return null;
            }
            fadeCanvasGroup.blocksRaycasts = false;
            isFade = false;
        }
    }
}

