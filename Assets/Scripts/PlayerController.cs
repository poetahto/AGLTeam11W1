using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Serializable]
    public struct MovementSettings
    {
        public float speed;
        public float acceleration;
        public float deceleration;
        public float reverseMultiplier;
    }
    
    [Header("References")]
    [SerializeField] private new Rigidbody2D rigidbody;
    
    [Header("Jumping Settings")]
    [SerializeField] private float jumpSpeed = 5.0f;
    [SerializeField] private float groundedDistance = 0.6f;
    [SerializeField] private float normalGravity = 4;
    [SerializeField] private float fastFallGravity = 8;
    [SerializeField] private float jumpBufferTime = 0.5f;
    [SerializeField] private float coyoteTime = 0.25f;
    
    [Header("Movement Settings")]
    [SerializeField] private MovementSettings groundedMovement;
    [SerializeField] private MovementSettings airborneMovement;

    [Header("Raycast Settings")]
    [SerializeField] private Vector2 boxSize = Vector2.one;

    private AutoResettingBool _wantsToJump;
    private float _timeSpendFalling;
    private bool _coyoteAvailable;
    private bool _isGrounded;
    
    private void Awake()
    {
        _wantsToJump = new AutoResettingBool(jumpBufferTime, false);
    }
    
    private void Update()
    {
        _isGrounded = CheckIfGrounded();

        UpdateMovement();
        UpdateJumping();
        UpdateGravity();
    }

    private void UpdateJumping()
    {
        if (!_isGrounded)
            _timeSpendFalling += Time.deltaTime;

        else
        {
            _timeSpendFalling = 0;
            _coyoteAvailable = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
            _wantsToJump.Value = true;

        if (_wantsToJump.Value && (_isGrounded || (_timeSpendFalling < coyoteTime && _coyoteAvailable)))
        {
            Jump();
            _coyoteAvailable = false;
        }
    }

    private void UpdateGravity()
    {
        float gravity = Input.GetKey(KeyCode.Space) && rigidbody.velocity.y > 0 ? normalGravity : fastFallGravity;
        gravity *= Time.deltaTime;
        rigidbody.velocity += Vector2.down * gravity;
    }

    private bool CheckIfGrounded()
    {
        int oldLayer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        var hit = Physics2D.BoxCast(transform.position, boxSize, 0, Vector2.down, groundedDistance);
        gameObject.layer = oldLayer;
        return hit != default(RaycastHit2D);
    }

    private static Vector2 GetInputDirection()
    {
        return new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );
    }

    private void UpdateMovement()
    {
        MovementSettings settings = _isGrounded 
            ? groundedMovement 
            : airborneMovement;
        
        Vector2 inputDirection = GetInputDirection();
        Vector2 currentVelocity = rigidbody.velocity;

        var targetVelocity = new Vector2(inputDirection.x * settings.speed, currentVelocity.y);
        
        bool didReverse = targetVelocity.x != 0 && currentVelocity.x != 0 &&
                          // ReSharper disable once CompareOfFloatsByEqualityOperator
                          Mathf.Sign(targetVelocity.x) != Mathf.Sign(currentVelocity.x);
        
        float acceleration = inputDirection != Vector2.zero 
            ? settings.acceleration 
            : settings.deceleration;

        float reverseMultiplier = didReverse ? settings.reverseMultiplier : 1;
        float maxDelta = acceleration * reverseMultiplier * Time.deltaTime;
        
        rigidbody.velocity = Vector2.MoveTowards(currentVelocity, targetVelocity, maxDelta);
    }

    private void Jump()
    {
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpSpeed);
    }
}
    
public static class GameObjectExtensions
{
    public static bool Boxcast2dIgnoreSelf(
        this GameObject gameObject,
        Ray2D ray,
        Vector2 size,
        float angle,
        float maxDistance,
        out RaycastHit2D hit)
    {
        int oldLayer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        hit = Physics2D.BoxCast(ray.origin, size, angle, ray.direction, maxDistance);
        gameObject.layer = oldLayer;
        return hit != default(RaycastHit2D);
    }
}
