using System;
using DefaultNamespace;
using UnityEngine;

namespace Player.Grapple.States
{
    [Serializable]
    public class Idle : GrappleState
    {
        [SerializeField] private float timeAmount = 0.1f;
        [SerializeField] private float timeDuration = 0.25f;
        [SerializeField] private float shakeIntensity = 0.1f;
        [SerializeField] private float recallSpeed = 5;

        public override void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
                FireGrapple();

            else PullGrappleBack();
        }

        private void FireGrapple()
        {
            if (PlayerTransform.gameObject.Raycast2dIgnoreSelf(GetAimRay(), out var hit))
            {
                // Move the grapple sprite to the hit location.
                GrappleTransform.SetParent(hit.transform);
                GrappleTransform.position = hit.point;
                    
                // Inform the object that we hit it.
                if (hit.transform.TryGetComponent(out GrappleTarget target))
                    target.OnGrappleHit(PlayerTransform.gameObject, hit);

                // Determine the correct state to transition to.
                StateMachine.TransitionTo(hit.transform.TryGetComponent(out CollisionTarget _)
                    ? StateMachine.MoveTowardsTarget
                    : StateMachine.MoveTowardsEnvironment);

                ApplyHitJuice();
            }
        }
        
        private void ApplyHitJuice()
        {
            TimeSlowdown.Instance.Hit(timeAmount, timeDuration);
            CameraShake.Instance.Shake(shakeIntensity);
        }

        private void PullGrappleBack()
        {
            Vector3 grapplePos = GrappleTransform.position;
            Vector3 playerPos = PlayerTransform.position;
            float delta = recallSpeed * Time.deltaTime;
            
            GrappleTransform.position = Vector3.Lerp(grapplePos, playerPos, delta);
        }
    }
}