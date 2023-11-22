using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPool : MonoBehaviour
{
    public static List<IPlayer> Players = new List<IPlayer>();

    public static PlayerPool Instance;

    public void Start()
    {
        var players = SceneManager.GetActiveScene().GetRootGameObjects().Where(t => t.GetComponent<IPlayer>() != null).Select(t => t.GetComponent<IPlayer>()).ToList();
        foreach (var player in players)
        {
            AddPlayer(player);
        }
    }
    public static void AddPlayer(IPlayer player)
    {
        Players.Add(player);
    }
    
    public static void RemovePlayer(IPlayer player)
    {
        Players.Remove(player);
    }
    
    public static (IPlayer player, float distance) GetClosestPlayer(Vector3 position)
    {
        var closestDistance = float.MaxValue;
        IPlayer closestPlayer = null;
        
        foreach (var player in Players)
        {
            var distance = Vector3.Distance(player.Transform.position, position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer   = player;
            }
        }

        return (closestPlayer, closestDistance);
    }
    
}