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
        
        public override void OnEnter()
        {
            base.OnEnter();
            _elapsedTime = 0;

            TimeSlowdown.Instance.OverrideTimeScale();
            Time.timeScale = slowDown;
            StateMachine.VolumeWeight = 1;
            pointer.gameObject.SetActive(true);
        }

        public override void OnExit()
        {
            base.OnExit();
            Time.timeScale = 1;
            TimeSlowdown.Instance.ReleaseTimeScale();
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