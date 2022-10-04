using System;
using UnityEngine;

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

    private void OnDrawGizmos()
    {
        Gizmos.color = _isGrounded ? Color.green : Color.red;
        Gizmos.DrawCube((Vector2) transform.position + (Vector2.down * groundedDistance), boxSize);
    }

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

    private RaycastHit2D[] _hits = new RaycastHit2D[25];
    
    private void UpdateIsGrounded()
    {
        var ray = new Ray2D
        {
            origin = transform.position,
            direction = Vector2.down
        };
        
        _isGrounded = gameObject.Boxcast2dIgnoreSelf(ray, boxSize, out var hit, groundedDistance) && !hit.collider.isTrigger;
    }

    private void FixedUpdate()
    {
        _dirty = true;
    }
}