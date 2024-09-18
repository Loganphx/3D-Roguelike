using UnityEngine;

namespace ECS.Movement.Services
{
    public static class RaycastExtensions
    {
        public static RaycastHit SortRaycastHits(this RaycastHit[] hits, int hitCount)
        {
            float      closestHitDistance = float.PositiveInfinity;
            RaycastHit closestHit         = default;
            for (int i = 0; i < hitCount; i++)
            {
                var hit = hits[i]; 
                if (hit.distance < closestHitDistance)
                {
                    closestHitDistance = hit.distance;
                    closestHit         = hit;
                }
            }
      
            return closestHit;
        }
    }
}