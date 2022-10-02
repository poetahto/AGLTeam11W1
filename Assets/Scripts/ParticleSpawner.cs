using System.Collections;
using poetools;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class ParticleSpawner : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particlePrefab;

        private WaitForSeconds _waitForParticleDuration;
        private ObjectPool<ParticleSystem> _objectPool;
        private static Scene _poolScene;

        private void Awake()
        {
            _waitForParticleDuration = new WaitForSeconds(particlePrefab.main.duration);
            
            if (_poolScene.IsValid() == false)
                _poolScene = SceneManager.CreateScene(PoetoolsNaming.PoolSceneName);

            _objectPool = new ObjectPool<ParticleSystem>(
                CreatePooledInstance,
                actionOnGet: SetupParticle,
                actionOnRelease: CleanupParticle, 
                actionOnDestroy: Destroy
            );
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
            _objectPool.Dispose();
        }

        public void Spawn(Vector3 position, Quaternion rotation)
        {
            IEnumerator coroutine = SpawnParticle(position, rotation);
            StartCoroutine(coroutine);
        }
        
        private IEnumerator SpawnParticle(Vector3 position, Quaternion rotation)
        {
            var particle = _objectPool.Get();
            particle.transform.SetPositionAndRotation(position, rotation);
            yield return _waitForParticleDuration;
            _objectPool.Release(particle);
        }
        
        private ParticleSystem CreatePooledInstance()
        {
            ParticleSystem instance = Instantiate(particlePrefab);
            SceneManager.MoveGameObjectToScene(instance.gameObject, _poolScene);
            CleanupParticle(instance);
            return instance;
        }

        private static void SetupParticle(ParticleSystem system)
        {
            system.gameObject.SetActive(true);
            system.Play(true);
        }
        
        private static void CleanupParticle(ParticleSystem system)
        {
            system.gameObject.SetActive(false);
            system.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
}