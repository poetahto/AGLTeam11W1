using System;
using UnityEngine;

namespace PlayerControl
{
    [Serializable]
    public class GrappleMoveTowardsTargetState : GrappleState
    {
        [SerializeField] private float speed;
        [SerializeField] private float acceleration;
        [SerializeField] private float velocityBraking = 0.1f;
        [SerializeField] private float detachDistance = 1f;
        
        public override void OnEnter()
        {
            base.OnEnter();
            StateMachine.playerRigidbody.velocity *= velocityBraking;
            StateMachine.playerTransform.GetComponent<PlayerJumping2d>().GravityEnabled = false;
        }
        
        public override void OnExit()
        {
            base.OnExit();
            StateMachine.playerTransform.GetComponent<PlayerJumping2d>().GravityEnabled = true;
        }

        public override void Update()
        {
            base.Update();

            Vector3 playerPosition = StateMachine.playerTransform.position;
            
            Debug.DrawLine(playerPosition, StateMachine.grappleTransform.position, Color.yellow);
            
            Vector2 vectorToGrapple = StateMachine.grappleTransform.position - playerPosition;
            
            StateMachine.playerRigidbody.velocity = Vector2.MoveTowards(StateMachine.playerRigidbody.velocity,
                vectorToGrapple.normalized * speed, acceleration * Time.deltaTime);
            
            if (Input.GetKeyUp(KeyCode.Mouse0) || vectorToGrapple.sqrMagnitude <= detachDistance * detachDistance)
                StateMachine.TransitionTo(StateMachine.idleState);
        }
    }
}