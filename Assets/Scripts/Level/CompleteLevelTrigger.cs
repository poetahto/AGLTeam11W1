using System;
using Player;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace.Level
{
    public class CompleteLevelTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject victoryConfetti;
        [SerializeField] private UnityEvent onComplete;
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out PlayerMovement2d _))
            {
                FindObjectOfType<LevelStateMachine>().CompleteLevel();
                onComplete.Invoke();
                Instantiate(victoryConfetti, transform.position, transform.rotation);
            }
        }
    }
}