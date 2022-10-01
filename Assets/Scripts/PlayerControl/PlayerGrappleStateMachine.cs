using System;
using System.Collections;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Rendering;

namespace PlayerControl
{
    [Serializable]
    public class GrappleIdleState : GrappleState
    {
        [SerializeField] private float timeAmount = 0.1f;
        [SerializeField] private float timeDuration = 0.25f;
        [SerializeField] private float shakeIntensity = 0.1f;
        [SerializeField] private GameObject impactParticles;
        [SerializeField] private float recallSpeed = 5;

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
                    
                    if (hit.transform.TryGetComponent(out Damageable damageable))
                    {
                        StateMachine.TransitionTo(StateMachine.moveTowardsDamageableState);
                    }
                    else
                    {
                        StateMachine.TransitionTo(StateMachine.moveEnvironmentState);    
                    }
                    
                    TimeSlowdown.Instance.Hit(timeAmount, timeDuration);
                    CameraShake.Instance.Shake(shakeIntensity);
                    var instance = UnityEngine.Object.Instantiate(impactParticles);
                    instance.transform.SetPositionAndRotation(hit.point, Quaternion.LookRotation((playerPosition - (Vector3) hit.point).normalized));
                    return;
                }

                Debug.DrawLine(playerPosition, hit.point, Color.red);
            }

            StateMachine.grappleTransform.position = Vector2.Lerp(StateMachine.grappleTransform.position,
            StateMachine.transform.position, recallSpeed * Time.deltaTime);
        }
    }

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

    [Serializable]
    public class GrappleMoveTowardsDamageableState : GrappleState
    {
        [SerializeField] private float speed;
        [SerializeField] private float acceleration;
        [SerializeField] private float velocityBraking = 0.1f;
        [SerializeField] private float detachDistance = 1f;
        [SerializeField] private float boostDistance = 5f;
        
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

    [Serializable]
    public class GrappleDamageBoostState : GrappleState
    {
        [SerializeField] private float boostWindow;
        [SerializeField] private float slowDown;
        [SerializeField] private float shakeAmount;
        [SerializeField] private GameObject pointer;
        
        private float _elapsedTime;
        
        public override void OnEnter()
        {
            base.OnEnter();
            _elapsedTime = 0;
            TimeSlowdown.Instance.enabled = false;
            Time.timeScale = slowDown;
            StateMachine.VolumeWeight = 1;
            pointer.gameObject.SetActive(true);
        }

        public override void OnExit()
        {
            base.OnExit();
            Time.timeScale = 1;
            TimeSlowdown.Instance.enabled = true;
            StateMachine.VolumeWeight = 0;
            pointer.gameObject.SetActive(false);
        }

        public override void Update()
        {
            base.Update();
            _elapsedTime += Time.deltaTime;
            
            if (_elapsedTime > boostWindow)
                StateMachine.TransitionTo(StateMachine.idleState);
            
            else if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                Vector3 playerPosition = StateMachine.playerTransform.position;
                Vector3 aimPosition = StateMachine.Camera.ScreenToWorldPoint(Input.mousePosition);
                aimPosition.z = playerPosition.z;
                var ray = new Ray2D(playerPosition, aimPosition - playerPosition);

                CameraShake.Instance.Shake(shakeAmount);
                
                StateMachine.playerRigidbody.velocity = ray.direction * StateMachine.playerRigidbody.velocity.magnitude;
                StateMachine.TransitionTo(StateMachine.idleState);
            }
        }
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
        [SerializeField] public Transform grappleTransform;
        [SerializeField] private float boostWindow = 0.5f; 
        
        public GrappleMoveTowardsDamageableState moveTowardsDamageableState;
        public GrappleMoveEnvironmentState moveEnvironmentState;
        public GrappleDamageBoostState damageBoostState;
        public GrappleIdleState idleState;

        [SerializeField] private Volume hitVolume;

        public Camera Camera { get; private set; }
        private Vector3 _aimPosition;
        private bool _isGrappling;
        private Vector2 _grapplePoint;
        private GrappleState _currentState;
        public float VolumeWeight { get; set; }

        private void Awake()
        {
            Camera = Camera.main;

            moveTowardsDamageableState.StateMachine = this;
            moveEnvironmentState.StateMachine = this;
            damageBoostState.StateMachine = this;
            idleState.StateMachine = this;
            
            FindObjectOfType<PlayerSpeedDamageSource>().OnHit += OnOnHit;

            TransitionTo(idleState);
        }

        private void OnOnHit()
        {
            _hitTimestamp = Time.unscaledTime;
            if (ShouldBoost)
                TransitionTo(damageBoostState);
        }

        public void TransitionTo(GrappleState state)
        {
            _currentState?.OnExit();
            _currentState = state;
            _currentState?.OnEnter();
        }

        private bool _wantsToBoost;
        private float _boostTimestamp = float.NaN;
        private float _hitTimestamp = float.NaN;
        private bool ShouldBoost => _wantsToBoost && Time.unscaledTime - _boostTimestamp <= boostWindow;

        [SerializeField] private float volumeSpeed;
        
        private void Update()
        {
            hitVolume.weight = Mathf.Lerp(hitVolume.weight, VolumeWeight, volumeSpeed*Time.unscaledDeltaTime);
            
            _wantsToBoost = Input.GetKey(KeyCode.Mouse1);
            
            if (Input.GetKeyDown(KeyCode.Mouse1))
                _boostTimestamp = Time.unscaledTime;

            if (Time.unscaledTime - _hitTimestamp <= boostWindow && Time.unscaledTime - _boostTimestamp <= boostWindow && _currentState != damageBoostState)
                TransitionTo(damageBoostState);
            
            _currentState?.Update();
        }
    }
}
