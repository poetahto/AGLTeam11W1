using UnityEngine;
using UnityEngine.Rendering;

namespace PlayerControl
{
    public class GrappleStateMachine : MonoBehaviour
    {
        [SerializeField] public Rigidbody2D playerRigidbody;
        [SerializeField] public Transform playerTransform;
        [SerializeField] public Transform grappleTransform;
        [SerializeField] private float boostWindow = 0.5f; 
        [SerializeField] private Volume hitVolume;
        [SerializeField] private float volumeSpeed;
        
        public GrappleMoveTowardsTargetState moveTowardsTargetState;
        public GrappleMoveEnvironmentState moveEnvironmentState;
        public GrappleDamageBoostState damageBoostState;
        public GrappleIdleState idleState;

        public float VolumeWeight { get; set; }
        public Camera Camera { get; private set; }
        private bool ShouldBoost => _wantsToBoost && Time.unscaledTime - _boostTimestamp <= boostWindow;
        
        private Vector3 _aimPosition;
        private bool _isGrappling;
        private Vector2 _grapplePoint;
        private GrappleState _currentState;
        private bool _wantsToBoost;
        private float _boostTimestamp = float.NaN;
        private float _hitTimestamp = float.NaN;
        
        private void Awake()
        {
            Camera = Camera.main;

            moveTowardsTargetState.StateMachine = this;
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
