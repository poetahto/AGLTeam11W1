using UnityEngine;

namespace DefaultNamespace
{
    public class ScaleAnimator : MonoBehaviour
    {
        private Vector3 _originalScale;
        private Vector3 _currentScale;

        public void Hit(float amount)
        {
            _currentScale += _originalScale * amount;
        }
        
        private void Awake()
        {
            _originalScale = transform.localScale;
            _currentScale = _originalScale;
        }
        
        private void Update()
        {
            _currentScale = Vector3.Lerp(_currentScale, _originalScale, 15 * Time.deltaTime);
            transform.localScale = _currentScale;
        }
    }
}