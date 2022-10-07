using UnityEngine;

namespace DefaultNamespace.Level
{
    public class AutoUnparenter : MonoBehaviour
    {
        private void Awake()
        {
            transform.SetParent(null);
        }
    }
}