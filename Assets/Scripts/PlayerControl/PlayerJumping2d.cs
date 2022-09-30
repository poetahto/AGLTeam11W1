using UnityEngine;

namespace PlayerControl
{
    public class PlayerJumping2d : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private new Rigidbody2D rigidbody;
        
        [Header("Jumping Settings")]
        [SerializeField] private float jumpSpeed = 5.0f;
        [SerializeField] private float groundedDistance = 0.6f;
        [SerializeField] private float normalGravity = 4;
        [SerializeField] private float fastFallGravity = 8;
        [SerializeField] private Vector2 boxSize = Vector2.one;
        [SerializeField] private float jumpBufferTime = 0.5f;
        [SerializeField] private float coyoteTime = 0.25f;
        
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
            bool isGrounded = IsGrounded();

            if (!isGrounded)
                _timeSpendFalling += Time.deltaTime;

            else
            {
                _timeSpendFalling = 0;
                _coyoteAvailable = true;
            }

            if (Input.GetKeyDown(KeyCode.Space))
                _wantsToJump.Value = true;

            if (_wantsToJump.Value && (isGrounded || (_timeSpendFalling < coyoteTime && _coyoteAvailable)))
            {
                Jump();
                _coyoteAvailable = false;
            }

            float gravity = Input.GetKey(KeyCode.Space) && rigidbody.velocity.y > 0 ? normalGravity : fastFallGravity;
            gravity *= Time.deltaTime;
            rigidbody.velocity += Vector2.down * gravity;
        }

        private bool IsGrounded()
        {
            var ray = new Ray2D
            {
                origin = transform.position,
                direction = Vector2.down
            };

            return gameObject.Boxcast2dIgnoreSelf(ray, boxSize, out _, groundedDistance);
        }

        private void Jump()
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpSpeed);
        }
    }
}