using UnityEngine;

namespace Player
{
    public class PlayerDamageApplier : MonoBehaviour
    {
        public void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.TryGetComponent(out PlayerDamageListener listener))
                listener.OnHit(gameObject, col.GetContact(0).point);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.TryGetComponent(out PlayerDamageListener listener))
                listener.OnHit(gameObject, col.transform.position);
        }
    }
}