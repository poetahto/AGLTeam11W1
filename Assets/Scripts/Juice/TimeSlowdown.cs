using poetools;
using UnityEngine;

namespace DefaultNamespace
{
    public class TimeSlowdown : PreparedSingleton<TimeSlowdown>
    {
        private float _slowdown;
        private float _duration;
        private float _elapsedTime;
        private bool _manualOverride;

        public void Hit(float amount, float duration)
        {
            _slowdown = amount;
            _duration = duration;
            _elapsedTime = 0;
        }

        public void OverrideTimeScale()
        {
            _manualOverride = true;
        }

        public void ReleaseTimeScale()
        {
            _manualOverride = false;
        }

        private void Update()
        {
            if (_duration != 0 && !_manualOverride)
                Time.timeScale = Mathf.Lerp(_slowdown, 1, _elapsedTime / _duration);
            
            _elapsedTime += Time.unscaledDeltaTime;
        }
    }
}