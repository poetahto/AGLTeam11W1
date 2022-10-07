using System;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Events;

namespace Player.Grapple.States
{
    [Serializable]
    public class Idle : GrappleState
    {
        [SerializeField] private float timeAmount = 0.1f;
        [SerializeField] private float timeDuration = 0.25f;
        [SerializeField] private float shakeIntensity = 0.1f;
        [SerializeField] private float recallSpeed = 5;
        [SerializeField] private float maxDistance = 15;
        [SerializeField] private UnityEvent onHit;

        public override void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
                FireGrapple();

            else PullGrappleBack();
        }

        private RaycastHit2D[] _hits = new RaycastHit2D[100];
        
        private void FireGrapple()
        {
            var ray = GetAimRay();
            PlayerTransform.gameObject.RaycastAll2dIgnoreSelf(ray, _hits, out int hitCount, maxDistance);
            
            var hit = _hits
                .Take(hitCount)
                .OrderBy(hit2D => hit2D.distance)
                .FirstOrDefault(hit2D => !hit2D.collider.isTrigger || hit2D.collider.TryGetComponent(out GrappleTarget _));

            if (hit == default)
                hit.point = ray.origin + (ray.direction * maxDistance);

            // Move the grapple sprite to the hit location.
            GrappleTransform.SetParent(hit.transform);
            GrappleTransform.position = hit.point;

            if (hit.transform != null)
            {
                if (hit.transform.TryGetComponent(out GrappleTarget grappleTarget))
                    grappleTarget.OnGrappleHit(PlayerTransform.gameObject, hit);
                
                // Determine the correct state to transition to.
                StateMachine.TransitionTo(hit.transform.TryGetComponent(out CollisionTarget _)
                    ? StateMachine.MoveTowardsTarget
                    : StateMachine.MoveTowardsEnvironment);

                ApplyHitJuice();
                onHit.Invoke();
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