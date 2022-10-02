using UnityEngine;

namespace poetools
{
    public interface IGameObjectFactory
    {
        GameObject Create();
        void Release(GameObject instance);
    }
    
    public abstract class GameObjectFactory : MonoBehaviour, IGameObjectFactory
    {
        public abstract GameObject Create();
        public abstract void Release(GameObject instance);
    }
}