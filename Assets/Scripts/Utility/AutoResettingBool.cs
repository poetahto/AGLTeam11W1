using UnityEngine;

/// <summary>
/// A boolean that automatically resets after a certain amount of time.
/// </summary>
public class AutoResettingBool
{
    private readonly bool _defaultValue;
    private readonly float _resetTime;
    private float _lastSetTime;
    
    public bool Value
    {
        get
        {
            bool result = Time.unscaledTime - _lastSetTime > _resetTime;
            return _defaultValue ? result : !result;
        }
        set
        {
            if (value != _defaultValue)
                _lastSetTime = Time.unscaledTime;
        }
    }

    public AutoResettingBool(float resetTime, bool defaultValue)
    {
        _resetTime = resetTime;
        _defaultValue = defaultValue;
    }
}