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
    
    public static bool RaycastAll2dIgnoreSelf(
        this GameObject gameObject,
        Ray2D ray,
        in RaycastHit2D[] hitArray,
        out int hitCount,
        float maxDistance = float.PositiveInfinity)
    {
        int oldLayer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        // hit = Physics2D.Raycast(ray.origin, ray.direction, maxDistance);
        hitCount = Physics2D.RaycastNonAlloc(ray.origin, ray.direction, hitArray, maxDistance);
        gameObject.layer = oldLayer;
        return hitCount > 0;
    }
    
    public static bool BoxcastAll2dIgnoreSelf(
        this GameObject gameObject,
        Ray2D ray,
        Vector2 size,
        in RaycastHit2D[] hitArray,
        out int hitCount,
        float maxDistance = float.PositiveInfinity,
        float angle = 0)
    {
        int oldLayer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        hitCount = Physics2D.BoxCastNonAlloc(ray.origin, size, angle, ray.direction, hitArray, maxDistance);
        gameObject.layer = oldLayer;
        return hitCount > 0;
    }
}