using UnityEngine;

namespace DefaultNamespace
{
    public class CollisionTarget : MonoBehaviour
    {
        [SerializeField] private ParticleSpawner deathParticles;

        public void OnDamage(GameObject source)
        {
            Vector3 sourcePos = source.transform.position;
            Vector3 targetPos = transform.position;
            
            deathParticles.Spawn(sourcePos, Quaternion.LookRotation((sourcePos - targetPos).normalized));
        }
    }
}