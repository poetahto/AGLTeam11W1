using UnityEngine;

namespace DefaultNamespace
{
    public class GrappleTarget : MonoBehaviour
    {
        [SerializeField] private Optional<ParticleSpawner> particleSpawner;
        
        public void OnGrappleHit(GameObject source, RaycastHit2D hitInfo)
        {
            Vector3 position = hitInfo.point;
            Quaternion rotation = Quaternion.LookRotation((source.transform.position - position).normalized);
            
            if (particleSpawner.ShouldBeUsed)
                particleSpawner.Value.Spawn(position, rotation);
        }
    }
}