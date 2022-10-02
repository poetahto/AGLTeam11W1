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
        [SerializeField] private GrappleTarget defaultGrappleTarget;

        public override void Update()
        {
            base.Update();

            Vector3 playerPosition = StateMachine.playerTransform.position;
            Vector3 aimPosition = StateMachine.Camera.ScreenToWorldPoint(Input.mousePosition);
            aimPosition.z = playerPosition.z;
            
            var ray = new Ray2D(playerPosition, aimPosition - playerPosition);

            if (StateMachine.playerTransform.gameObject.Raycast2dIgnoreSelf(ray, out var hit))
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    StateMachine.grappleTransform.SetParent(hit.transform);
                    StateMachine.grappleTransform.position = hit.point;
                    
                    if (hit.transform.TryGetComponent(out GrappleTarget target))
                    {
                        target.OnGrappleHit(StateMachine.gameObject, hit);
                        StateMachine.TransitionTo(StateMachine.moveTowardsTargetState);
                    }
                    else
                    {
                        defaultGrappleTarget.OnGrappleHit(StateMachine.gameObject, hit);
                        StateMachine.TransitionTo(StateMachine.moveEnvironmentState);    
                    }
                    
                    TimeSlowdown.Instance.Hit(timeAmount, timeDuration);
                    CameraShake.Instance.Shake(shakeIntensity);
                    return;
                }

                Debug.DrawLine(playerPosition, hit.point, Color.red);
            }

            StateMachine.grappleTransform.position = Vector2.Lerp(StateMachine.grappleTransform.position,
                StateMachine.transform.position, recallSpeed * Time.deltaTime);
        }
    }
}