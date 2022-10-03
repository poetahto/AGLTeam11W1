using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class CollisionTarget : MonoBehaviour
    {
        [SerializeField] private Optional<ParticleSpawner> collisionParticles;

        public UnityEvent onHit;
        
        public void OnDamage(GameObject source)
        {
            onHit.Invoke();
            
            if (collisionParticles.ShouldBeUsed)
                PlayParticles(source.transform.position);
        }

        private void PlayParticles(Vector3 sourcePosition)
        {
            Vector3 targetPosition = transform.position;
            Vector3 direction = (sourcePosition - targetPosition).normalized;
            
            collisionParticles.Value.Spawn(sourcePosition, Quaternion.LookRotation(direction));
        }
    }
}