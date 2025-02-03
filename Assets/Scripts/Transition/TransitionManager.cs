using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// 描述：切换场景管理。
// 创建者：Aze
// 创建时间：2025-01-21

namespace Sprout.Transition
{
    public class TransitionManager : MonoBehaviour
    {
        [SceneName]
        public string startSceneName = string.Empty;
        private CanvasGroup fadeCanvasGroup;
        private bool isFade;

        private void OnEnable()
        {
            EventHandler.TransitionEvent += OnTransitionEvent;
        }

        private void OnDisable()
        {
            EventHandler.TransitionEvent -= OnTransitionEvent;
        }

        private IEnumerator Start()
        {
            fadeCanvasGroup = FindObjectOfType<CanvasGroup>();
            yield return StartCoroutine(LoadSceneSetActive(startSceneName));
            EventHandler.CallAfterSceneUnloadEvent();
        }

        private void OnTransitionEvent(string sceneToGo, Vector3 positionToGo)
        {
            if (!isFade)
                StartCoroutine(Transition(sceneToGo,positionToGo));
        }

        /// <summary>
        /// 场景切换
        /// </summary>
        /// <param name="sceneName">目标场景</param>
        /// <param name="targetPosition">目标位置</param>
        /// <returns></returns>
        private IEnumerator Transition(string sceneName, Vector3 targetPosition)
        {
            EventHandler.CallBeforeSceneUnloadEvent();
            yield return Fade(1);
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            yield return LoadSceneSetActive(sceneName);
            EventHandler.CallMoveToPosition(targetPosition);
            EventHandler.CallAfterSceneUnloadEvent();
            yield return Fade(0);
        }

        /// <summary>
        /// 加载场景设置为激活
        /// </summary>
        /// <returns></returns>
        private IEnumerator LoadSceneSetActive(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
            SceneManager.SetActiveScene(newScene);
        }


        /// <summary>
        /// 淡入淡出场景
        /// </summary>
        /// <param name="targetAlpha">0白1黑</param>
        /// <returns></returns>
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

