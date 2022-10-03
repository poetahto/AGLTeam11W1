using System;
using UnityEngine;

namespace Player.Grapple
{
    [Serializable]
    public class MoveTowardsEnvironment : GrappleState
    {
        [SerializeField] private float grappleForce;
        
        public override void Update()
        {
            Vector3 direction = (GrappleTransform.position - PlayerTransform.position).normalized;
            Vector3 velocity = direction * grappleForce;
            StateMachine.playerRigidbody.AddForce(velocity);
            
            if (Input.GetKey(KeyCode.Mouse0) == false)
                StateMachine.TransitionTo(StateMachine.Idle);
        }
    }
}