using UnityEngine;

public static class GameObjectExtensions
{
    public static bool Boxcast2dIgnoreSelf(
        this GameObject gameObject,
        Ray2D ray,
        Vector2 size,
        out RaycastHit2D hit,
        float maxDistance = float.PositiveInfinity,
        float angle = 0)
    {
        int oldLayer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        hit = Physics2D.BoxCast(ray.origin, size, angle, ray.direction, maxDistance);
        gameObject.layer = oldLayer;
        return hit != default(RaycastHit2D);
    }
    
    public static bool Raycast2dIgnoreSelf(
        this GameObject gameObject,
        Ray2D ray,
        out RaycastHit2D hit,
        float maxDistance = float.PositiveInfinity)
    {
        int oldLayer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        hit = Physics2D.Raycast(ray.origin, ray.direction, maxDistance);
        gameObject.layer = oldLayer;
        return hit != default(RaycastHit2D);
    }
}