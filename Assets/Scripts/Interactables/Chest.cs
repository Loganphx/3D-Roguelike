using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
// ReSharper disable LocalVariableHidesMember

public class SpawnTableEntry
{
    public POWERUP_TYPE PowerupId;
    public int Chance;
}
public class PowerupSpawnTable
{
    public SpawnTableEntry[] Entries;
}

public class ChestData
{
    public int GoldCost;
    public Color Color;
    public PowerupSpawnTable SpawnTable;
    public string ChestName;
}
public enum RarityTypes
{
    NULL,
    COMMON,
    UNCOMMON,
    RARE,
    EPIC,
    LEGENDARY
}
public class Chest : MonoBehaviour, IInteractable, IHoverable
{
    public static Dictionary<RarityTypes, ChestData> RarityToChestData = new Dictionary<RarityTypes, ChestData>()
    {
        {RarityTypes.COMMON, new ChestData()
        {
            GoldCost = 10,
            Color = new Color(103/255f, 34/255f, 1/255f),
            SpawnTable = new PowerupSpawnTable()
            {
                Entries = new []
                {
                    new SpawnTableEntry()
                    {
                        PowerupId = POWERUP_TYPE.INCREASE_HEALTH,
                        Chance = 33,
                    },
                    new SpawnTableEntry()
                    {
                        PowerupId = POWERUP_TYPE.INCREASE_STAMINA,
                        Chance = 66,
                    },
                    new SpawnTableEntry()
                    {
                        PowerupId = POWERUP_TYPE.SLOW_HUNGER,
                        Chance = 1,
                    }
                }
            },
            ChestName = "Common\nChest"
        }},
        {RarityTypes.UNCOMMON, new ChestData()
        {
            GoldCost = 25, 
            Color = new Color(29/255f, 195/255f, 0/255f),
            SpawnTable = new PowerupSpawnTable()
            {
                Entries = new []
                {
                    new SpawnTableEntry()
                    {
                        PowerupId = POWERUP_TYPE.EXTRA_JUMP,
                        Chance = 100,
                    }
                }
            },
            ChestName = "Uncommon\nChest"
        }},
        {RarityTypes.RARE, new ChestData()
        {
            GoldCost = 100,
            Color = new Color(3/255f, 67/255f, 195/255f),
            SpawnTable = new PowerupSpawnTable()
            {
                Entries = new []
                {
                    new SpawnTableEntry()
                    {
                        PowerupId = POWERUP_TYPE.EXTRA_MELEE_DAMAGE,
                        Chance = 100,
                    }
                }
            },
            ChestName = "Rare\nChest"
        }},
        {RarityTypes.EPIC, new ChestData() { 
            GoldCost = 250, 
            Color = new Color(149/255f, 0/255f, 131/255f),
            SpawnTable = new PowerupSpawnTable()
            {
                Entries = new []
                {
                    new SpawnTableEntry()
                    {
                        PowerupId = POWERUP_TYPE.PIGGY_BANK,
                        Chance = 100,
                    }
                }
            },
            ChestName = "Epic\nChest"
        }},
        {RarityTypes.LEGENDARY, new ChestData()
        {
            GoldCost = 500, 
            Color = new Color(233/255f, 71/255f, 0/255f),
            SpawnTable = new PowerupSpawnTable()
            {
                Entries = new []
                {
                    new SpawnTableEntry()
                    {
                        PowerupId = POWERUP_TYPE.THORS_HAMMER,
                        Chance = 100,
                    }
                }
            },
            ChestName = "Legendary\nChest"
        }},
    };
    
    [SerializeField] public RarityTypes       Rarity;
    
    [SerializeField] private Outline           _outline;
    
    public ChestData ChestData;

    public void Awake()
    {
        ChestData = RarityToChestData[Rarity];
        
        var meshRenders = GetComponentsInChildren<MeshRenderer>();
        foreach (var meshRender in meshRenders)
        {
            meshRender.material.color         = ChestData.Color;
        }
        _outline = GetComponent<Outline>();
        _outline.enabled = false;

        transform.GetComponentInChildren<TMP_Text>().SetText(ChestData.ChestName);
    }

    public INTERACTABLE_TYPE GetInteractableType()
    {
        return INTERACTABLE_TYPE.CHEST;
    }

    public void Interact(IPlayer player)
    {
        if (player.GetGoldInInventory() < ChestData.GoldCost)
        {
            Debug.LogError($"Failed to open chest, you need {ChestData.GoldCost - player.GetGoldInInventory()} more gold");
            return;
        }
        
        player.RemoveItem(ITEM_TYPE.COIN, ChestData.GoldCost);

        var hinge = transform.GetChild(0).Find("Hinge");
        // var localEulerAngles = hinge.localEulerAngles;
        // Debug.Log($"Opening chest: {localEulerAngles.x}");
        // Debug.Log($"Opening chest: {hinge.localEulerAngles.x}");


        // TODO: Replace with actual animation. 
        hinge.localRotation = Quaternion.Euler(Math.Abs(hinge.localEulerAngles.x - 90) < 1f ?
            0 : 90, 0, 0);

        var powerupPrefab = PrefabPool.Prefabs[PowerupPool.PowerupPrefabs[((POWERUP_TYPE)(int)Rarity)]];
        var position = transform.position;
        var powerUpPos = new Vector3(position.x, position.y + 1f, position.z);
        var powerup = Instantiate(powerupPrefab, powerUpPos, Quaternion.identity);
        powerup.GetComponentInChildren<MeshRenderer>().material.color = ChestData.Color;

        enabled = false;
        var transforms = GetComponentsInChildren<Collider>();
        foreach (Collider transform in transforms)
        {
            Debug.Log(transform.gameObject.layer);
            Debug.Log(LayerMask.NameToLayer("Interactable"));
            if(transform.gameObject.layer == LayerMask.NameToLayer("Interactable"))
                transform.gameObject.layer = LayerMask.NameToLayer("Default");
        }
        // GeneratePowerup(player);
    }

    // ReSharper disable once UnusedMember.Local
    private void GeneratePowerup()
    {
        int random = Random.Range(0, 100);
        foreach (var entry in ChestData.SpawnTable.Entries)
        {
            if (random < entry.Chance)
            {
                SpawnPowerup(entry.PowerupId);
                return;
            }
        }
    }

    private void SpawnPowerup(POWERUP_TYPE powerupId)
    {
        Instantiate(PrefabPool.Prefabs[PowerupPool.PowerupPrefabs[powerupId]], transform.position, Quaternion.identity);
    }

    public void OnHoverEnter(IPlayer player)
    {
        _outline.enabled = true;
    }

    public void OnHoverExit(IPlayer player)
    {
        _outline.enabled = false;
    }
}