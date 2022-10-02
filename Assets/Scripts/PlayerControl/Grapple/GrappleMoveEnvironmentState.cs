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
            base.Update();

            Vector3 playerPosition = StateMachine.playerTransform.position;
            
            Debug.DrawLine(playerPosition, StateMachine.grappleTransform.position, Color.green);
            
            Vector2 direction = (StateMachine.grappleTransform.position - playerPosition).normalized;
            StateMachine.playerRigidbody.AddForce(direction * grappleForce);
            
            if (Input.GetKeyUp(KeyCode.Mouse0))
                StateMachine.TransitionTo(StateMachine.idleState);
        }
    }
}