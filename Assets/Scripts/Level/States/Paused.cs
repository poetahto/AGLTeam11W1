using System;
using UnityEngine;

namespace DefaultNamespace.Level
{
    [Serializable]
    public class Paused : LevelState
    {
        [SerializeField] private GameObject pauseMenu;

        private IDisposable _timeOverride;
        
        public override void OnEnter()
        {
            base.OnEnter();
            pauseMenu.SetActive(true);
            _timeOverride = TimeSlowdown.Instance.OverrideTimeScale(0);
        }

        public override void OnExit()
        {
            base.OnExit();
            pauseMenu.SetActive(false);
            _timeOverride.Dispose();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            if (Input.GetKeyDown(KeyCode.Escape))
                StateMachine.TransitionTo(StateMachine.RunningState);
        }
    }
}