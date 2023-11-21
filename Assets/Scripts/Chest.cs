using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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

public class ChestData
{
    public int GoldCost;
    public Color Color;
}
public enum RarityTypes
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}
public class Chest : MonoBehaviour, IInteractable, IHoverable
{
    public static Dictionary<RarityTypes, PowerupSpawnTable> RarityToSpawnTable = new Dictionary<RarityTypes, PowerupSpawnTable>()
    {
        {RarityTypes.Common, new PowerupSpawnTable()},
        {RarityTypes.Uncommon, new PowerupSpawnTable()},
        {RarityTypes.Rare, new PowerupSpawnTable()},
        {RarityTypes.Epic, new PowerupSpawnTable()},
        {RarityTypes.Legendary, new PowerupSpawnTable()},
    }; 
    
    public static Dictionary<RarityTypes, ChestData> RarityToChestData = new Dictionary<RarityTypes, ChestData>()
    {
        {RarityTypes.Common, new ChestData() { GoldCost = 10, Color = new Color(103/255f, 34/255f, 1/255f)}},
        {RarityTypes.Uncommon, new ChestData() { GoldCost = 25, Color = new Color(29/255f, 195/255f, 0/255f)}},
        {RarityTypes.Rare, new ChestData() { GoldCost = 100, Color = new Color(3/255f, 67/255f, 195/255f)}},
        {RarityTypes.Epic, new ChestData() { GoldCost = 250, Color = new Color(149/255f, 0/255f, 131/255f)}},
        {RarityTypes.Legendary, new ChestData() { GoldCost = 500, Color = new Color(233/255f, 71/255f, 0/255f)}},
    };
    
    [SerializeField] public RarityTypes       Rarity;
    
    [SerializeField] private Outline           _outline;
    
    public ChestData ChestData;
    public PowerupSpawnTable SpawnTable;
    

    public void Awake()
    {
        ChestData = RarityToChestData[Rarity];
        SpawnTable = RarityToSpawnTable[Rarity];
        
        var meshRenders = GetComponentsInChildren<MeshRenderer>();
        foreach (var meshRender in meshRenders)
        {
            meshRender.material.color         = ChestData.Color;
        }
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
    }
    public void Interact(IPlayer player)
    {
        if (player.Gold < ChestData.GoldCost)
        {
            Debug.LogError($"Failed to open chest, you need {ChestData.GoldCost - player.Gold} more gold");
            return;
        }

        var hinge = transform.Find("Hinge");
        // var localEulerAngles = hinge.localEulerAngles;
        // Debug.Log($"Opening chest: {localEulerAngles.x}");
        // Debug.Log($"Opening chest: {hinge.localEulerAngles.x}");
        hinge.localRotation = Quaternion.Euler(Math.Abs(hinge.localEulerAngles.x - 90) < 1f ?
            0 : 90, 0, 0);


        // GeneratePowerup(player);
    }

    private void GeneratePowerup(IPlayer player)
    {
        int random = Random.Range(0, 100);
        foreach (var entry in SpawnTable.Entries)
        {
            if (random < entry.Chance)
            {
                player.Gold -= ChestData.GoldCost;
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