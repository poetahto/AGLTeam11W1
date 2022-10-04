using poetools;
using UnityEngine;

public class CameraFovHit : PreparedSingleton<CameraFovHit>
{
    [SerializeField] private float recoverySpeed = 0.1f;

    private float _intensity;
    private float _originalFov;
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;

        if (_camera != null)
            _originalFov = _camera.orthographic ? _camera.orthographicSize : _camera.fieldOfView;
    }

    public void Hit(float intensity)
    {
        _intensity += intensity;
    }

    private void Update()
    {
        _intensity = Mathf.MoveTowards(_intensity, 0, Time.deltaTime * recoverySpeed);
        
        float newFov = _originalFov + _originalFov * _intensity;
        
        if (_camera.orthographic)
            _camera.orthographicSize = newFov;
        
        else _camera.fieldOfView = newFov;
    }
}