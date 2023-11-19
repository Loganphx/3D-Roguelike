using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPowerup
{
    
}

public class SpawnTableEntry
{
    public IPowerup Powerup;
    public int Chance;
}
public class PowerupSpawnTable
{
    public SpawnTableEntry[] Entries = new SpawnTableEntry[]
    {
     new SpawnTableEntry()
     {
         Chance = 10,
         Powerup = default
     },
     new SpawnTableEntry()
     {
         Chance = 20,
         Powerup = default
     }   
    };
}

public class Chest : MonoBehaviour, IInteractable, IHoverable
{
    [SerializeField] public PowerupSpawnTable SpawnTable;
    [SerializeField] public int               GoldCost;
    [SerializeField] public Color             Color;
    [SerializeField] private Outline           _outline;
    
    public void Awake()
    {
        GetComponent<MeshRenderer>().material.color         = Color;
        _outline = GetComponent<Outline>();

        _outline.enabled = false;
    }
    public void Interact(IPlayer player)
    {
        if (player.Gold < GoldCost)
        {
            Debug.LogError($"Failed to open chest, you need {GoldCost - player.Gold} more gold");
            return;
        }
        
        GeneratePowerup(player);
    }

    private void GeneratePowerup(IPlayer player)
    {
        int random = Random.Range(0, 100);
        foreach (var entry in SpawnTable.Entries)
        {
            if (random < entry.Chance)
            {
                player.Gold -= GoldCost;
                SpawnPowerup(entry.Powerup);
                return;
            }
        }
    }

    private void SpawnPowerup(IPowerup powerup)
    {
        GameObject.Instantiate(powerup as MonoBehaviour, transform.position, Quaternion.identity);
    }

    public void OnHoverEnter()
    {
        _outline.enabled = true;
    }

    public void OnHoverExit()
    {
        _outline.enabled = false;
    }
}