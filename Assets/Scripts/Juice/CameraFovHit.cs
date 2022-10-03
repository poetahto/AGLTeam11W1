using poetools;
using UnityEngine;

public class CameraFovHit : PreparedSingleton<CameraFovHit>
{
    [SerializeField] private new Camera camera;
    [SerializeField] private float originalFov = 60;
    [SerializeField] private float recoverySpeed = 1;

    private float _intensity;

    public void Hit(float intensity)
    {
        _intensity += intensity;
    }

    private void Update()
    {
        _intensity = Mathf.MoveTowards(_intensity, 0, Time.deltaTime * recoverySpeed);
        camera.orthographicSize = originalFov + (originalFov * _intensity);
    }
}