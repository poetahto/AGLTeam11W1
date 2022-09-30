using UnityEngine;

namespace PlayerControl
{
    public class PlayerGrapple : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D playerRigidbody;
        [SerializeField] private float grappleForce;
        
        private Camera _camera;
        private Vector3 _aimPosition;
        private bool _isGrappling;
        private Vector2 _grapplePoint;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            _aimPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            _aimPosition.z = transform.position.z;
            
            var ray = new Ray2D(transform.position, _aimPosition - transform.position);

            if (gameObject.Raycast2dIgnoreSelf(ray, out var hit))
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    _isGrappling = true;
                    _grapplePoint = hit.point;
                }

                if (Input.GetKeyUp(KeyCode.Mouse0))
                    _isGrappling = false;
                
                var color = _isGrappling ? Color.green : Color.red;
                var endPoint = _isGrappling ? _grapplePoint : hit.point;
                Debug.DrawLine(transform.position, endPoint, color);
            }

            if (_isGrappling)
            {
                Vector2 direction = (_grapplePoint - (Vector2) transform.position).normalized;
                playerRigidbody.AddForce(direction * grappleForce);
            }
        }
    }
}