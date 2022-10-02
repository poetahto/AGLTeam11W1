using UnityEngine;
using UnityEngine.SceneManagement;

namespace poetools
{
    public class PooledPrefabFactory : MonoBehaviour, IGameObjectFactory
    {
        [SerializeField] private GameObject prefab;

        public GameObject Prefab => prefab;
        private ObjectPool<GameObject> _objectPool;
        private static Scene _poolScene;
        
        public void Awake()
        {
            if (_poolScene.IsValid() == false)
                _poolScene = SceneManager.CreateScene(PoetoolsNaming.PoolSceneName);
            
            _objectPool = new ObjectPool<GameObject>(
                CreatePooledInstance, 
                actionOnGet:     pooledGameObject => pooledGameObject.SetActive(true), 
                actionOnRelease: pooledGameObject => pooledGameObject.SetActive(false), 
                actionOnDestroy: Destroy
            );
        }

        private void OnDestroy()
        {
            _objectPool.Dispose();
        }

        private GameObject CreatePooledInstance()
        {
            GameObject instance = Instantiate(prefab);
            instance.SetActive(false);
            SceneManager.MoveGameObjectToScene(instance, _poolScene);
            return instance;
        }

        public GameObject Create()
        {
            return _objectPool.Get();
        }

        public void Release(GameObject instance)
        {
            _objectPool.Release(instance);
        }
    }
}