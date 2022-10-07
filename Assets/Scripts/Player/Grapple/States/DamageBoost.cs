using System;
using DefaultNamespace;
using UnityEngine;

namespace Player.Grapple.States
{
    [Serializable]
    public class DamageBoost : GrappleState
    {
        [SerializeField] private float boostWindow;
        [SerializeField] private float slowDown;
        [SerializeField] private float shakeAmount;
        [SerializeField] private GameObject pointer;
        
        private float _elapsedTime;
        private IDisposable _timeOverride;
        
        public override void OnEnter()
        {
            base.OnEnter();
            _elapsedTime = 0;

            _timeOverride = TimeSlowdown.Instance.OverrideTimeScale(() => slowDown);
            StateMachine.VolumeWeight = 1;
            pointer.gameObject.SetActive(true);
        }

        public override void OnExit()
        {
            base.OnExit();
            
            _timeOverride.Dispose();
            StateMachine.VolumeWeight = 0;
            pointer.gameObject.SetActive(false);
        }

        public override void Update()
        {
            _elapsedTime += Time.deltaTime;
            
            if (_elapsedTime > boostWindow)
                StateMachine.TransitionTo(StateMachine.Idle);
            
            else if (Input.GetKey(KeyCode.Mouse1) == false)
            {
                CameraShake.Instance.Shake(shakeAmount);
                
                StateMachine.playerRigidbody.velocity = GetAimRay().direction * StateMachine.playerRigidbody.velocity.magnitude;
                StateMachine.TransitionTo(StateMachine.Idle);
            }
        }
    }
}