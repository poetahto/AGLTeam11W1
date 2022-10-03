using System;
using UnityEngine;

namespace Player.Grapple.States
{
    [Serializable]
    public class MoveTowardsTarget : GrappleState
    {
        [SerializeField] private float speed;
        [SerializeField] private float acceleration;
        [SerializeField] private float velocityBraking = 0.1f;
        [SerializeField] private float detachDistance = 1f;

        public override void OnEnter()
        {
            base.OnEnter();
            StateMachine.playerRigidbody.velocity *= velocityBraking;
            StateMachine.playerJumping.GravityEnabled = false;
        }
        
        public override void OnExit()
        {
            base.OnExit();
            StateMachine.playerJumping.GravityEnabled = true;
        }

        public override void Update()
        {
            Vector3 vectorToGrapple = GrappleTransform.position - PlayerTransform.position;
            
            UpdatePlayerVelocity(vectorToGrapple.normalized);
            CheckForDetach(vectorToGrapple);
        }

        private void CheckForDetach(Vector2 vectorToGrapple)
        {
            bool wantsToDetach = Input.GetKey(KeyCode.Mouse0) == false;
            bool isTooClose = vectorToGrapple.sqrMagnitude <= detachDistance * detachDistance;
            
            if (wantsToDetach || isTooClose)
                StateMachine.TransitionTo(StateMachine.Idle);
        }

        private void UpdatePlayerVelocity(Vector2 direction)
        {
            Vector2 currentVelocity = StateMachine.playerRigidbody.velocity;
            Vector2 targetVelocity = direction * speed;
            float maxDelta = acceleration * Time.deltaTime;
            
            StateMachine.playerRigidbody.velocity = Vector2.MoveTowards(currentVelocity, targetVelocity, maxDelta);
        }
    }
}