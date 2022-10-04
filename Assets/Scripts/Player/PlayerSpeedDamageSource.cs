using System;
using DefaultNamespace;
using UnityEngine;

namespace Player
{
    public class PlayerSpeedDamageSource : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D playerRigidbody;
        [SerializeField] private float minSpeed;
        [SerializeField] private float timeHitDuration;
        [SerializeField] private float timeHitAmount;
        [SerializeField] private float fovHitAmount;

        public event Action OnHit;
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (playerRigidbody.velocity.sqrMagnitude < minSpeed * minSpeed)
                return;

            if (col.TryGetComponent(out CollisionTarget target))
            {
                CameraShake.Instance.Shake(1);
                TimeSlowdown.Instance.Hit(timeHitAmount, timeHitDuration);
                CameraFovHit.Instance.Hit(fovHitAmount);
                
                target.OnDamage(gameObject);
                OnHit?.Invoke();
            }
        }
    }
}