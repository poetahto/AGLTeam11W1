using System;
using DefaultNamespace;
using UnityEngine;

namespace PlayerControl
{
    [Serializable]
    public class GrappleIdleState : GrappleState
    {
        [SerializeField] private float timeAmount = 0.1f;
        [SerializeField] private float timeDuration = 0.25f;
        [SerializeField] private float shakeIntensity = 0.1f;
        [SerializeField] private float recallSpeed = 5;

        public override void Update()
        {
            if (PlayerTransform.gameObject.Raycast2dIgnoreSelf(GetAimRay(), out var hit))
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    GrappleTransform.SetParent(hit.transform);
                    GrappleTransform.position = hit.point;
                    
                    if (hit.transform.TryGetComponent(out GrappleTarget target))
                        target.OnGrappleHit(PlayerTransform.gameObject, hit);

                    GrappleState nextState;

                    if (hit.transform.TryGetComponent(out CollisionTarget _))
                        nextState = StateMachine.moveTowardsTargetState;
                    
                    else nextState = StateMachine.moveEnvironmentState;
                    
                    StateMachine.TransitionTo(nextState);

                    TimeSlowdown.Instance.Hit(timeAmount, timeDuration);
                    CameraShake.Instance.Shake(shakeIntensity);
                    return;
                }
            }

            GrappleTransform.position = Vector2.Lerp(GrappleTransform.position,
                StateMachine.transform.position, recallSpeed * Time.deltaTime);
        }
    }
}