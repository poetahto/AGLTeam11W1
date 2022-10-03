using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class GrappleTarget : MonoBehaviour
    {
        [SerializeField] private Optional<ParticleSpawner> particleSpawner;

        public UnityEvent onHit;

        public void OnGrappleHit(GameObject source, RaycastHit2D hitInfo)
        {
            onHit.Invoke();

            if (particleSpawner.ShouldBeUsed)
                PlayParticles(source.transform.position, hitInfo.point);
        }

        private void PlayParticles(Vector3 sourcePosition, Vector3 targetPosition)
        {
            Vector3 direction = (sourcePosition - targetPosition).normalized;
            particleSpawner.Value.Spawn(targetPosition, Quaternion.LookRotation(direction));
        }
    }
}