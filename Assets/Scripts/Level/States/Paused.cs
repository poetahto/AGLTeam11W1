using System;
using Player;
using Player.Grapple;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DefaultNamespace.Level
{
    [Serializable]
    public class Paused : LevelState
    {
        [SerializeField] private GameObject pauseMenu;

        private IDisposable _timeOverride;

        private void SetInput(bool enabled)
        {
            Object.FindObjectOfType<PlayerJumping2d>().enabled = enabled;
            Object.FindObjectOfType<PlayerMovement2d>().enabled = enabled;
            Object.FindObjectOfType<GrappleStateMachine>().enabled = enabled;
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            SetInput(false);
            pauseMenu.SetActive(true);
            _timeOverride = TimeSlowdown.Instance.OverrideTimeScale(0);
        }

        public override void OnExit()
        {
            base.OnExit();
            SetInput(true);
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