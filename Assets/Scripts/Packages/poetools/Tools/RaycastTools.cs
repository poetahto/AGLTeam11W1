using UnityEngine;

namespace poetools.Tools
{
    public static class RaycastTools
    {
        public static bool Raycast3D(Ray ray, out RaycastHit hit, float maxDistance, int layerMask, 
            QueryTriggerInteraction interaction, float time = 1.0f)
        {
            bool result = Physics.Raycast(ray, out hit, maxDistance, layerMask, interaction);
            #if UNITY_EDITOR
                DrawDebugHandle(ray, result, hit, time, maxDistance);
            #endif
            return result;
        }
        
        public static bool Raycast3D(Ray ray, out RaycastHit hit, float maxDistance, float time = 1.0f)
        {
            bool result = Physics.Raycast(ray, out hit, maxDistance);
            #if UNITY_EDITOR
                DrawDebugHandle(ray, result, hit, time, maxDistance);
            #endif
            return result;
        }
        
        public static bool Raycast3D(Ray ray, out RaycastHit hit, float time = 1.0f)
        {
            bool result = Physics.Raycast(ray, out hit);
            #if UNITY_EDITOR
                DrawDebugHandle(ray, result, hit, time);
            #endif
            return result;
        }

        private static void DrawDebugHandle(Ray ray, bool success, RaycastHit hit, float time, float maxDistance = 1.0f)
        {
            Vector3 end = success ? hit.point : ray.origin + ray.direction * maxDistance;
            Color color = success ? Color.blue : Color.red;
            
            DebugDrawTools.Arrow(ray.origin, end, color, time);
        }
    }
}