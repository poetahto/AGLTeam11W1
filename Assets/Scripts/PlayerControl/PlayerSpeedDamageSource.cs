using DefaultNamespace;
using UnityEngine;

namespace PlayerControl
{
    public class PlayerSpeedDamageSource : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D playerRigidbody;
        [SerializeField] private float minSpeed;
        [SerializeField] private float timeHitDuration;
        [SerializeField] private float timeHitAmount;
        [SerializeField] private GameObject hitParticles;

        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (playerRigidbody.velocity.sqrMagnitude < minSpeed * minSpeed)
                return;
            
            CameraShake.Instance.Shake(1);
            TimeSlowdown.Instance.Hit(timeHitAmount, timeHitDuration);
            
            var instance = Instantiate(hitParticles);
            instance.transform.SetPositionAndRotation(col.transform.position, Quaternion.LookRotation((transform.position - col.transform.position).normalized));
        }
    }
}