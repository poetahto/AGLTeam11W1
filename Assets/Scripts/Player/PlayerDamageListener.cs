using DefaultNamespace;
using DefaultNamespace.Level;
using UnityEngine;

namespace Player
{
    public class PlayerDamageListener : MonoBehaviour
    {
        [SerializeField] private ParticleSystem deathParticles;
        [SerializeField] private GameObject grapple;
        [SerializeField] private float shakeAmount = 1;
        [SerializeField] private float timeHitAmount = 1;
        [SerializeField] private float timeHitDuration = 1;

        private LevelStateMachine _levelStateMachine;

        private void Awake()
        {
            _levelStateMachine = FindObjectOfType<LevelStateMachine>();
        }

        public void OnHit(GameObject source, Vector3 point)
        {
            deathParticles.Play();
            deathParticles.transform.SetParent(null);
            gameObject.SetActive(false);
            grapple.SetActive(false);
            
            Vector3 direction = (source.transform.position - point).normalized;
            deathParticles.transform.SetPositionAndRotation(point, Quaternion.LookRotation(direction));
            CameraShake.Instance.Shake(shakeAmount);
            TimeSlowdown.Instance.Hit(timeHitAmount, timeHitDuration);
            
            _levelStateMachine.RestartLevel();
        }
    }
}