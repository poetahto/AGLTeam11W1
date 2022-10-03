using Player.Grapple.States;
using UnityEngine;
using UnityEngine.Rendering;

namespace Player.Grapple
{
    public class GrappleStateMachine : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] public Rigidbody2D playerRigidbody;
        [SerializeField] public PlayerJumping2d playerJumping;
        [SerializeField] private PlayerSpeedDamageSource damageSource;
        [SerializeField] public Transform playerTransform;
        [SerializeField] public Transform grappleTransform;
        [SerializeField] private Volume hitVolume;

        [Header("Boosting Settings")]
        [SerializeField] private float boostWindow = 0.5f; 
        [SerializeField] private float volumeSpeed;
        
        [Header("State Settings")]
        [SerializeField] private Idle idle;
        [SerializeField] private MoveTowardsTarget moveTowardsTarget;
        [SerializeField] private MoveTowardsEnvironment moveTowardsEnvironment;
        [SerializeField] private DamageBoost damageBoost;

        public GrappleState Idle => idle;
        public GrappleState MoveTowardsTarget => moveTowardsTarget;
        public GrappleState MoveTowardsEnvironment => moveTowardsEnvironment;
        public GrappleState DamageBoost => damageBoost;

        public float VolumeWeight { get; set; }
        public Camera Camera { get; private set; }
        
        private Vector3 _aimPosition;
        private bool _isGrappling;
        private Vector2 _grapplePoint;
        private GrappleState _currentState;
        private float _boostTimestamp = float.NaN;
        private float _hitTimestamp = float.NaN;
        
        private void Awake()
        {
            Camera = Camera.main;

            idle.StateMachine = this;
            moveTowardsTarget.StateMachine = this;
            moveTowardsEnvironment.StateMachine = this;
            damageBoost.StateMachine = this;
            
            TransitionTo(idle);
        }

        private void OnEnable()
        {
            damageSource.OnHit += OnHit;
        }

        private void OnDisable()
        {
            damageSource.OnHit -= OnHit;
        }

        private void OnHit()
        {
            _hitTimestamp = Time.time;
        }

        public void TransitionTo(GrappleState state)
        {
            _currentState?.OnExit();
            _currentState = state;
            _currentState?.OnEnter();
        }
        
        private void Update()
        {
            // Animate the boost effect post processing.
            hitVolume.weight = Mathf.Lerp(hitVolume.weight, VolumeWeight, volumeSpeed*Time.unscaledDeltaTime);
            
            CheckForBoost();
            
            _currentState?.Update();
        }

        private void CheckForBoost()
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
                _boostTimestamp = Time.time;

            float timeSinceHit = Time.time - _hitTimestamp;
            float timeSinceInput = Time.time - _boostTimestamp;

            if (timeSinceInput <= boostWindow && timeSinceHit <= boostWindow / 2 && _currentState != damageBoost)
            {
                TransitionTo(DamageBoost);
                _boostTimestamp = float.NaN;
                _hitTimestamp = float.NaN;
            }
        }
    }
}
