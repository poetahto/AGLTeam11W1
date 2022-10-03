using UnityEngine;

namespace Player.Grapple
{
    public class GrappleLineBinder : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        
        private LineRenderer _lineRenderer;

        private void Awake()
        {
            transform.SetParent(null);
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            _lineRenderer.SetPositions(new [] {playerTransform.position, transform.position});
        }
    }
}