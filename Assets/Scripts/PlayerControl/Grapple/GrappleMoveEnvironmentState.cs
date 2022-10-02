using System;
using UnityEngine;

namespace PlayerControl
{
    [Serializable]
    public class GrappleMoveEnvironmentState : GrappleState
    {
        [SerializeField] private float grappleForce;
        
        public override void Update()
        {
            Vector3 direction = (StateMachine.grappleTransform.position - StateMachine.playerTransform.position).normalized;
            StateMachine.playerRigidbody.AddForce(direction * grappleForce);
            
            if (Input.GetKey(KeyCode.Mouse0) == false)
                StateMachine.TransitionTo(StateMachine.idleState);
        }
    }
}