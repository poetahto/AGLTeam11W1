using UnityEngine;

namespace PlayerControl
{
    public class CirclePointer : MonoBehaviour
    {
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            Vector3 playerPosition = transform.position;
            Vector3 aimPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            aimPosition.z = playerPosition.z;
            var ray = new Ray2D(playerPosition, aimPosition - playerPosition);
            transform.rotation = Quaternion.LookRotation(Vector3.forward,  Quaternion.Euler(0, 0, 0) * ray.direction);
        }
    }
}