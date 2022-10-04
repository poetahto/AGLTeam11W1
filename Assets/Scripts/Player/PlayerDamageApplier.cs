using DefaultNamespace.Level;
using UnityEngine;

namespace Player
{
    public class PlayerDamageApplier : MonoBehaviour
    {
        private LevelStateMachine _stateMachine;

        private void Awake()
        {
            _stateMachine = FindObjectOfType<LevelStateMachine>();
        }

        public void OnCollisionEnter2D(Collision2D col)
        {
            if (col.collider == GetComponent<Collider2D>() && col.gameObject.TryGetComponent(out PlayerDamageListener listener))
                ApplyDamage(listener);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col == GetComponent<Collider2D>() && col.gameObject.TryGetComponent(out PlayerDamageListener listener))
                ApplyDamage(listener);
        }

        private void ApplyDamage(PlayerDamageListener listener)
        {
            _stateMachine.RestartLevel();
            
            // todo: death animation
            Debug.LogError("Death animation is not implemented yet! Check todo.");
        }
    }
}