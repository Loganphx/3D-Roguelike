using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vector3 = UnityEngine.Vector3;

public static class PlayerPool
{
    public static List<IPlayer> Players = new List<IPlayer>();

    public static void Initialize()
    {
        Players = new List<IPlayer>();
    }
    public static void AddPlayer(IPlayer player)
    {
        Players.Add(player);
    }
    
    public static void RemovePlayer(IPlayer player)
    {
        Players.Remove(player);
    }
    
    public static (IPlayer player, Vector3 position, float distance) GetClosestPlayer(Vector3 position)
    {
        var closestDistance = float.PositiveInfinity;
        Vector3 closestPlayerPosition = Vector3.negativeInfinity;
        IPlayer closestPlayer = null;
        
        // Debug.Log(Players.Count);
        foreach (var player in Players)
        {
            if (!player.IsDead)
            {
                var playerPosition = player.Transform.position;
                var distance = Vector3.Distance(player.Transform.position, position);
                // Debug.Log(distance);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPlayerPosition = playerPosition;
                    closestPlayer   = player;
                }
            }
            // else Debug.LogWarning($"Player is dead {player}");
        
        }

        return (closestPlayer, closestPlayerPosition, closestDistance);
    }
    
}