using System.Collections;
using UnityEngine;

namespace DefaultNamespace.Level
{
    public class DisappearingUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float lifeTime;
        [SerializeField] private float fadeOutTime;
        [SerializeField] private float fadeInTime;

        private IEnumerator Start()
        {
            float elapsedTime = 0;

            while (elapsedTime < fadeInTime)
            {
                elapsedTime += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeInTime);
                yield return null;
            }
            
            yield return new WaitForSeconds(lifeTime);

            elapsedTime = 0;
            
            while (elapsedTime < fadeOutTime)
            {
                elapsedTime += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / fadeOutTime);
                yield return null;
            }

            canvasGroup.alpha = 0;
        }
    }
}