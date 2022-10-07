using DefaultNamespace;
using DefaultNamespace.Level;
using UnityEngine;

namespace Player
{
    public class PlayerDamageListener : MonoBehaviour
    {
        [SerializeField] private ParticleSystem deathParticles;
        [SerializeField] private AudioSource deathSound;
        [SerializeField] private GameObject grapple;
        [SerializeField] private float shakeAmount = 1;
        [SerializeField] private float timeHitAmount = 1;
        [SerializeField] private float timeHitDuration = 1;

        private LevelStateMachine _levelStateMachine;

        private void Awake()
        {
            _levelStateMachine = FindObjectOfType<LevelStateMachine>();
            var wakeup = AudioPlayer.Instance;
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        public void OnHit(GameObject source, Vector3 point)
        {
            deathSound.Play();
            
            deathParticles.Play();
            deathParticles.transform.SetParent(null);

            grapple.SetActive(false);
            gameObject.SetActive(false);

            deathParticles.transform.SetParent(null);
            deathParticles.transform.position = point;
            CameraShake.Instance.Shake(shakeAmount);
            TimeSlowdown.Instance.Hit(timeHitAmount, timeHitDuration);

            
            _levelStateMachine.HandlePlayerDeath();
        }
    }
}