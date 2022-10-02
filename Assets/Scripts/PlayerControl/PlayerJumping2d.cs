using UnityEngine;

namespace PlayerControl
{
    public class PlayerJumping2d : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private new Rigidbody2D rigidbody;
        [SerializeField] private GroundChecker groundChecker;
        
        [Header("Jumping Settings")]
        [SerializeField] private float jumpSpeed = 5.0f;
        [SerializeField] private float normalGravity = 4;
        [SerializeField] private float fastFallGravity = 8;
        [SerializeField] private float jumpBufferTime = 0.5f;
        [SerializeField] private float coyoteTime = 0.25f;

        public bool GravityEnabled { get; set; } = true;
        private bool IsGrounded => groundChecker.IsGrounded;
        
        private AutoResettingBool _wantsToJump;
        private float _timeSpendFalling;
        private bool _coyoteAvailable;
        
        private void Awake()
        {
            _wantsToJump = new AutoResettingBool(jumpBufferTime, false);
        }

        private void OnValidate()
        {
            if (_wantsToJump != null)
                _wantsToJump = new AutoResettingBool(jumpBufferTime, false);
        }

        private void Update()
        {
            if (!IsGrounded)
                _timeSpendFalling += Time.deltaTime;

            else
            {
                _timeSpendFalling = 0;
                _coyoteAvailable = true;
            }

            if (Input.GetKeyDown(KeyCode.Space))
                _wantsToJump.Value = true;

            if (_wantsToJump.Value && (IsGrounded || (_timeSpendFalling < coyoteTime && _coyoteAvailable)))
            {
                Jump();
                _coyoteAvailable = false;
            }

            if (GravityEnabled)
            {
                float gravity = Input.GetKey(KeyCode.Space) && rigidbody.velocity.y > 0 ? normalGravity : fastFallGravity;
                gravity *= Time.deltaTime;
                rigidbody.velocity += Vector2.down * gravity;    
            }
        }

        private void Jump()
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpSpeed);
        }
    }
}