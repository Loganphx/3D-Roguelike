using System.Collections.Generic;
using UnityEngine;

namespace Interactables
{
    public static class ResourceManager
    {
        public static Dictionary<int, IDamagable> Builds = new Dictionary<int, IDamagable>();

        public static (IDamagable closestPlayer, Vector3 closestPlayerPosition, float closestDistance) GetClosestBuild(Vector3 position)
        {
            var closestDistance = float.PositiveInfinity;
            Vector3 closestPlayerPosition = Vector3.negativeInfinity;
            IDamagable closestPlayer = null;
        
            // Debug.Log(Players.Count);
            foreach (var build in Builds)
            {
                // if (!build..IsDead)
                {
                    var playerPosition = build.Value.Transform.position;
                    var distance = Vector3.Distance(build.Value.Transform.position, position);
                    // Debug.Log(distance);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPlayerPosition = playerPosition;
                        closestPlayer   = build.Value;
                    }
                }
                // else Debug.LogWarning($"Player is dead {player}");
        
            }

            return (closestPlayer, closestPlayerPosition, closestDistance);
        }
    }
}