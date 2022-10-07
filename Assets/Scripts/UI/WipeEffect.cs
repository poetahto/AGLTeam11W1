using System.Collections;
using poetools;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class WipeEffect : PreparedSingleton<WipeEffect>
    {
        [SerializeField] private RectTransform uiTransform;
        [SerializeField] private int padding = 15;

        private void Start()
        {
            transform.SetParent(null);
            DontDestroyOnLoad(this);
        }

        public IEnumerator ShowEffect(float time)
        {
            int startOffset = -Screen.width - padding;
            yield return EffectHelper(time, startOffset, 0);
        }

        public IEnumerator HideEffect(float time)
        {
            int endOffset = Screen.width + padding;
            yield return EffectHelper(time, 0, endOffset);
        }

        private float EaseOutQuint(float value)
        {
            return 1 - Mathf.Pow(1 - value, 3);
        }

        private IEnumerator EffectHelper(float time, float start, float end)
        {
            float elapsedTime = 0;

            while (elapsedTime < time)
            {
                float t = Mathf.Clamp01(elapsedTime / time);
                float xPos = Mathf.Lerp(start, end, EaseOutQuint(t));
                elapsedTime += Time.deltaTime;
                uiTransform.anchoredPosition = new Vector2(xPos, 0);
                yield return null;
            }

            uiTransform.anchoredPosition = new Vector2(end, 0);
        }
    }
}