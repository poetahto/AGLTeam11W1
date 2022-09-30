using poetools;
using UnityEngine;

public class CameraShake : PreparedSingleton<CameraShake>
{
    [SerializeField] private Transform sourceTransform;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float shakeSpeed;
    [SerializeField] private float shakeAmount;
    [SerializeField] private float recoverySpeed;
    [SerializeField] private float rotationAmount;
    [SerializeField] private float rotationRecovery;
    
    private float _intensity;
    
    public void Shake(float intensity)
    {
        _intensity += intensity;
        targetTransform.rotation *= Quaternion.Euler(0, 0, intensity * Random.Range(-rotationAmount, rotationAmount));
    }

    private void Update()
    {
        _intensity = Mathf.MoveTowards(_intensity, 0, Time.deltaTime * recoverySpeed);
        
        float xPos = (Mathf.PerlinNoise(Time.time * shakeSpeed, 0) - shakeAmount / 2) * (2 * shakeAmount);
        float yPos = (Mathf.PerlinNoise(0, Time.time * shakeSpeed) - shakeAmount / 2) * (2 * shakeAmount);
        Vector3 shakeOffset = new Vector3(xPos, yPos) * _intensity;

        targetTransform.position = sourceTransform.position + shakeOffset;
        targetTransform.rotation = Quaternion.Lerp(targetTransform.rotation, sourceTransform.rotation, rotationRecovery * Time.deltaTime);
    }
}