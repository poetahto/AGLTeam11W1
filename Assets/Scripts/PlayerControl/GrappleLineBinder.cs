using UnityEngine;

namespace PlayerControl
{
    public class GrappleLineBinder : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        
        private LineRenderer _lineRenderer;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            _lineRenderer.SetPositions(new [] {playerTransform.position, transform.position});
        }
    }
}