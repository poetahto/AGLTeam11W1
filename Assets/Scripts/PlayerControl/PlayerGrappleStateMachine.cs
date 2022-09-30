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
        [SerializeField] private GameObject impactParticles;

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
                    if (hit.transform.TryGetComponent(out Damageable damageable))
                    {
                        StateMachine.moveTowardsDamageableState.GrapplePoint = hit.transform.position;
                        StateMachine.TransitionTo(StateMachine.moveTowardsDamageableState);
                    }
                    else
                    {
                        StateMachine.moveEnvironmentState.GrapplePoint = hit.point;
                        StateMachine.TransitionTo(StateMachine.moveEnvironmentState);    
                    }
                    
                    TimeSlowdown.Instance.Hit(timeAmount, timeDuration);
                    CameraShake.Instance.Shake(shakeIntensity);
                    var instance = UnityEngine.Object.Instantiate(impactParticles);
                    instance.transform.SetPositionAndRotation(hit.point, Quaternion.LookRotation((playerPosition - (Vector3) hit.point).normalized));
                }

                Debug.DrawLine(playerPosition, hit.point, Color.red);
            }
        }
    }

    [Serializable]
    public class GrappleMoveEnvironmentState : GrappleState
    {
        [SerializeField] private float grappleForce;
        
        public Vector2 GrapplePoint { get; set; }

        public override void Update()
        {
            base.Update();

            Vector3 playerPosition = StateMachine.playerTransform.position;
            
            Debug.DrawLine(playerPosition, GrapplePoint, Color.green);
            
            Vector2 direction = (GrapplePoint - (Vector2) playerPosition).normalized;
            StateMachine.playerRigidbody.AddForce(direction * grappleForce);
            
            if (Input.GetKeyUp(KeyCode.Mouse0))
                StateMachine.TransitionTo(StateMachine.idleState);
        }
    }

    [Serializable]
    public class GrappleMoveTowardsDamageableState : GrappleState
    {
        [SerializeField] private float speed;
        [SerializeField] private float acceleration;
        [SerializeField] private float velocityBraking = 0.1f;
        [SerializeField] private float detachDistance = 1f;
        
        public Vector2 GrapplePoint { get; set; }

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
            
            Debug.DrawLine(playerPosition, GrapplePoint, Color.yellow);
            
            Vector2 vectorToGrapple = GrapplePoint - (Vector2) playerPosition;
            
            StateMachine.playerRigidbody.velocity = Vector2.MoveTowards(StateMachine.playerRigidbody.velocity,
                vectorToGrapple.normalized * speed, acceleration * Time.deltaTime);
            
            if (Input.GetKeyUp(KeyCode.Mouse0))
                StateMachine.TransitionTo(StateMachine.idleState);

            if (vectorToGrapple.sqrMagnitude <= detachDistance)
            {
                StateMachine.TransitionTo(StateMachine.idleState);
            }
        }
    }

    [Serializable]
    public class GrappleDamageBoostState : GrappleState
    {
        
    }

    public abstract class GrappleState
    {
        public PlayerGrappleStateMachine StateMachine { get; set; }
        
        public virtual void OnEnter() {}
        public virtual void Update() {}
        public virtual void OnExit() {}
    }

    public class PlayerGrappleStateMachine : MonoBehaviour
    {
        [SerializeField] public Rigidbody2D playerRigidbody;
        [SerializeField] public Transform playerTransform;
        
        public GrappleMoveTowardsDamageableState moveTowardsDamageableState;
        public GrappleMoveEnvironmentState moveEnvironmentState;
        public GrappleDamageBoostState damageBoostState;
        public GrappleIdleState idleState;

        public Camera Camera { get; private set; }
        private Vector3 _aimPosition;
        private bool _isGrappling;
        private Vector2 _grapplePoint;

        private void Awake()
        {
            Camera = Camera.main;

            moveTowardsDamageableState.StateMachine = this;
            moveEnvironmentState.StateMachine = this;
            damageBoostState.StateMachine = this;
            idleState.StateMachine = this;

            TransitionTo(idleState);
        }

        private GrappleState _currentState;
        
        public void TransitionTo(GrappleState state)
        {
            _currentState?.OnExit();
            _currentState = state;
            _currentState?.OnEnter();
        }

        private void Update()
        {
            _currentState?.Update();
        }
    }
}
