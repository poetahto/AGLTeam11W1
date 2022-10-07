using System;
using Player;
using Player.Grapple;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DefaultNamespace.Level
{
    [Serializable]
    public class Completed : LevelState
    {
        [SerializeField] private GameObject victoryMenu;
        [SerializeField] private float slowdownSpeed = 5;

        private IDisposable _timeOverride;
        private float _timeScale;
        
        public override void OnEnter()
        {
            base.OnEnter();
            victoryMenu.SetActive(true);
            _timeScale = Time.timeScale;
            _timeOverride = TimeSlowdown.Instance.OverrideTimeScale(() => _timeScale);
            SetInput(false);
        }
        
        private void SetInput(bool enabled)
        {
            Object.FindObjectOfType<PlayerJumping2d>().enabled = enabled;
            Object.FindObjectOfType<PlayerMovement2d>().enabled = enabled;
            Object.FindObjectOfType<GrappleStateMachine>().enabled = enabled;
        }

        public override void OnExit()
        {
            base.OnExit();
            SetInput(true);
            victoryMenu.SetActive(false);
            _timeOverride.Dispose();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            _timeScale = Mathf.Lerp(_timeScale, 0, slowdownSpeed * Time.deltaTime);
        }
    }
}