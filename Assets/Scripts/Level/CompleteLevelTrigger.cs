using Player;
using UnityEngine;

namespace DefaultNamespace.Level
{
    public class CompleteLevelTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject victoryConfetti;
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out PlayerMovement2d _))
            {
                FindObjectOfType<LevelStateMachine>().CompleteLevel();
                Instantiate(victoryConfetti, transform.position, transform.rotation);
            }
        }
    }
}