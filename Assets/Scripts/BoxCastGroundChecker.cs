﻿using UnityEngine;

public abstract class GroundChecker : MonoBehaviour
{
    public abstract bool IsGrounded { get; }
}

/// <summary>
/// Wraps functionality for determining if an object is touching the ground.
/// Uses a box-cast internally, and caches results for the current frame for performance.
/// </summary>
public class BoxCastGroundChecker : GroundChecker
{
    [SerializeField] private Vector2 boxSize = Vector2.one;
    [SerializeField] private float groundedDistance = -0.1f;
    
    private bool _isGrounded;
    private bool _dirty;
    
    public override bool IsGrounded
    {
        get
        {
            if (_dirty)
            {
                UpdateIsGrounded();
                _dirty = false;
            }

            return _isGrounded;
        }
    }

    private void UpdateIsGrounded()
    {
        var ray = new Ray2D
        {
            origin = transform.position,
            direction = Vector2.down
        };
        
        _isGrounded = gameObject.Boxcast2dIgnoreSelf(ray, boxSize, 0, groundedDistance, out _);
    }

    private void FixedUpdate()
    {
        _dirty = true;
    }
}