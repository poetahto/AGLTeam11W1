using System;
using UnityEngine;

namespace PlayerControl
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement2d : MonoBehaviour
    {
        [Serializable]
        public struct MovementSettings
        {
            public float speed;
            public float acceleration;
            public float deceleration;
            public float reverseMultiplier;
        }

        [SerializeField] private float decelerationAmount = 5;
        [SerializeField] private MovementSettings groundedMovement;
        [SerializeField] private MovementSettings airborneMovement;

        private Rigidbody2D _rigidbody;
        private GroundChecker _groundChecker;
        private bool _hasGroundChecker;

        private bool IsGrounded => _hasGroundChecker && _groundChecker.IsGrounded;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _groundChecker = GetComponent<GroundChecker>();
            _hasGroundChecker = _groundChecker != null;
        }

        private void Update()
        {
            MovementSettings settings = IsGrounded 
                ? groundedMovement 
                : airborneMovement;
            
            Move(settings);
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
            Vector2 currentVelocity = _rigidbody.velocity;

            var targetVelocity = new Vector2(inputDirection.x * settings.speed, currentVelocity.y);
            
            bool didReverse = 
                targetVelocity.x != 0 && 
                currentVelocity.x != 0 &&
                (int) Mathf.Sign(targetVelocity.x) != (int) Mathf.Sign(currentVelocity.x);
            
            float acceleration = inputDirection != Vector2.zero 
                ? settings.acceleration 
                : settings.deceleration;

            if (inputDirection != Vector2.zero || !IsGrounded)
            {
                if ((int) Mathf.Sign(targetVelocity.x) == (int) Mathf.Sign(currentVelocity.x) && currentVelocity.magnitude > targetVelocity.magnitude)
                    return;
                
                // is accelerating
                float reverseMultiplier = didReverse ? settings.reverseMultiplier : 1;
                float maxDelta = acceleration * reverseMultiplier * Time.deltaTime;
                _rigidbody.velocity = Vector2.MoveTowards(currentVelocity, targetVelocity, maxDelta);
            }
            else
            {
                // is decelerating
                _rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, Vector2.zero, decelerationAmount * Time.deltaTime);
            }
        }
    }
}