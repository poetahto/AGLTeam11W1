using System;
using DefaultNamespace;
using UnityEngine;

namespace PlayerControl
{
    [Serializable]
    public class GrappleDamageBoostState : GrappleState
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
            TimeSlowdown.Instance.enabled = false;
            Time.timeScale = slowDown;
            StateMachine.VolumeWeight = 1;
            pointer.gameObject.SetActive(true);
        }

        public override void OnExit()
        {
            base.OnExit();
            Time.timeScale = 1;
            TimeSlowdown.Instance.enabled = true;
            StateMachine.VolumeWeight = 0;
            pointer.gameObject.SetActive(false);
        }

        public override void Update()
        {
            base.Update();
            _elapsedTime += Time.deltaTime;
            
            if (_elapsedTime > boostWindow)
                StateMachine.TransitionTo(StateMachine.idleState);
            
            else if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                var ray = GetAimRay();

                CameraShake.Instance.Shake(shakeAmount);
                
                StateMachine.playerRigidbody.velocity = ray.direction * StateMachine.playerRigidbody.velocity.magnitude;
                StateMachine.TransitionTo(StateMachine.idleState);
            }
        }
    }
}