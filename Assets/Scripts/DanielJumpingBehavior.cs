using System;
using UnityEngine;

namespace Exercises
{
    public class DanielJumpingBehavior : MonoBehaviour
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

        [Header("Movement Settings")]
        [SerializeField] private MovementSettings groundedMovement;
        [SerializeField] private MovementSettings airborneMovement;
        
        private void Update()
        {
            bool isGrounded = IsGrounded();
            
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
                Jump();

            MovementSettings settings = isGrounded 
                ? groundedMovement 
                : airborneMovement;
            
            Move(settings);
        }

        private bool IsGrounded()
        {
            var ray = new Ray2D
            {
                origin = transform.position,
                direction = Vector2.down
            };
            
            return gameObject.Raycast2DIgnoreSelf(ray, groundedDistance, out _);
        }

        private static Vector2 GetInputDirection()
        {
            return new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            );
        }

        private void Move(MovementSettings settings)
        {
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
        public static bool Raycast2DIgnoreSelf(
            this GameObject gameObject, 
            Ray2D ray, 
            float maxDistance,
            out RaycastHit2D hit)
        {
            int oldLayer = gameObject.layer;
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            hit = Physics2D.Raycast(ray.origin, ray.direction, maxDistance);
            gameObject.layer = oldLayer;
            return hit != default(RaycastHit2D);
        }
    }
}