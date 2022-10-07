using System;
using System.Collections.Generic;
using poetools;
using UnityEngine;

namespace DefaultNamespace
{
    public class TimeSlowdown : LazySingleton<TimeSlowdown>
    {
        private class OverrideDisposable : IDisposable
        {
            public LinkedList<OverrideDisposable> List;
            public Func<float> TargetTimeScale;
            
            public void Dispose()
            {
                List.Remove(this);
            }
        }
        
        private float _slowdown;
        private float _duration;
        private float _elapsedTime;
        private LinkedList<OverrideDisposable> _overrides = new LinkedList<OverrideDisposable>();
        private ObjectPool<OverrideDisposable> _overrideDisposablePool;

        protected override void Awake()
        {
            base.Awake();
            _overrideDisposablePool = new ObjectPool<OverrideDisposable>(() => new OverrideDisposable());
        }

        public void Hit(float amount, float duration)
        {
            _slowdown = amount;
            _duration = duration;
            _elapsedTime = 0;
        }

        public IDisposable OverrideTimeScale(Func<float> value)
        {
            OverrideDisposable result = _overrideDisposablePool.Get();
            result.TargetTimeScale = value;
            result.List = _overrides;
            
            _overrides.AddFirst(result);
            return result;
        }

        private void Update()
        {
            if (_overrides.Count > 0)
            {
                Time.timeScale = _overrides.First.Value.TargetTimeScale();
                return;
            }
            
            if (_duration != 0)
                Time.timeScale = Mathf.Lerp(_slowdown, 1, _elapsedTime / _duration);
            
            _elapsedTime += Time.unscaledDeltaTime;
        }
    }
}