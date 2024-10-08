using System;
using System.Collections.Generic;
using Interactables;
using UnityEngine;

[RequireComponent(typeof(Damagable), typeof(HealthBarUI))]
public class Anvil : MonoBehaviour, IInteractable, IHoverable, IDamagable
{
    public static Recipe[] Recipes =
    {
        // Other
        new Recipe(ITEM_TYPE.COIN, 5, ITEM_TYPE.INGOT_GOLD, 1),
        // Tools
        new Recipe(
            ITEM_TYPE.TOOL_PICKAXE_IRON, 1,
            ITEM_TYPE.WOOD_BIRCH, 5, 
            ITEM_TYPE.INGOT_IRON, 5),
        new Recipe(
            ITEM_TYPE.TOOL_AXE_IRON, 1,
            ITEM_TYPE.WOOD_BIRCH, 5, 
            ITEM_TYPE.INGOT_IRON, 5),
        new Recipe(
            ITEM_TYPE.TOOL_PICKAXE_MYTHRIL, 1,
            ITEM_TYPE.WOOD_OAK, 5, 
            ITEM_TYPE.INGOT_MYTHRIL, 5),
        new Recipe(
            ITEM_TYPE.TOOL_AXE_MYTHRIL, 1,
            ITEM_TYPE.WOOD_OAK, 5, 
            ITEM_TYPE.INGOT_MYTHRIL, 5),
        new Recipe(
            ITEM_TYPE.TOOL_PICKAXE_ADAMANTITE, 1,
            ITEM_TYPE.WOOD_DARKOAK, 5, 
            ITEM_TYPE.INGOT_ADAMANTITE, 5),
        new Recipe(
            ITEM_TYPE.TOOL_AXE_ADAMANTITE, 1,
            ITEM_TYPE.WOOD_DARKOAK, 5, 
            ITEM_TYPE.INGOT_ADAMANTITE, 5),
        // Weapons
        new Recipe(
            ITEM_TYPE.WEAPON_SWORD_IRON, 1,
            ITEM_TYPE.INGOT_IRON, 5,
            ITEM_TYPE.WOOD_BIRCH, 5),
        new Recipe(
            ITEM_TYPE.WEAPON_SWORD_MYTHRIL, 1,
            ITEM_TYPE.INGOT_MYTHRIL, 5,
            ITEM_TYPE.WOOD_OAK, 5),
        new Recipe(
            ITEM_TYPE.WEAPON_SWORD_ADAMANTITE, 1,
            ITEM_TYPE.INGOT_ADAMANTITE, 5,
            ITEM_TYPE.WOOD_DARKOAK, 5),
        new Recipe(
            ITEM_TYPE.WEAPON_BOW_IRON, 1,
            ITEM_TYPE.ROPE, 1, 
            ITEM_TYPE.WOOD_BIRCH, 5,
            ITEM_TYPE.INGOT_IRON, 5),
        new Recipe(
            ITEM_TYPE.WEAPON_BOW_MYTHRIL, 1,
            ITEM_TYPE.ROPE, 1, 
            ITEM_TYPE.WOOD_FIR, 5,
            ITEM_TYPE.INGOT_IRON, 5),
        new Recipe(
            ITEM_TYPE.WEAPON_BOW_ADAMANTITE, 1,
            ITEM_TYPE.ROPE, 1, 
            ITEM_TYPE.WOOD_DARKOAK, 5,
            ITEM_TYPE.INGOT_ADAMANTITE, 5),
        // Armor
        new Recipe(
            ITEM_TYPE.HELMET_IRON, 1,
            ITEM_TYPE.INGOT_IRON, 5),
        new Recipe(
            ITEM_TYPE.CHESTPLATE_IRON, 1,
            ITEM_TYPE.INGOT_IRON, 8),
        new Recipe(
            ITEM_TYPE.LEGGINGS_IRON, 1,
            ITEM_TYPE.INGOT_IRON, 7),
        new Recipe(
            ITEM_TYPE.BOOTS_IRON, 1,
            ITEM_TYPE.INGOT_IRON, 4),
        new Recipe(
            ITEM_TYPE.HELMET_MYTHRIL, 1,
            ITEM_TYPE.INGOT_MYTHRIL, 5),
        new Recipe(
            ITEM_TYPE.CHESTPLATE_MYTHRIL, 1,
            ITEM_TYPE.INGOT_MYTHRIL, 8),
        new Recipe(
            ITEM_TYPE.LEGGINGS_MYTHRIL, 1,
            ITEM_TYPE.INGOT_MYTHRIL, 7),
        new Recipe(
            ITEM_TYPE.BOOTS_MYTHRIL, 1,
            ITEM_TYPE.INGOT_MYTHRIL, 4),
        new Recipe(
            ITEM_TYPE.HELMET_ADAMANTITE, 1,
            ITEM_TYPE.INGOT_ADAMANTITE, 5),
        new Recipe(
            ITEM_TYPE.CHESTPLATE_ADAMANTITE, 1,
            ITEM_TYPE.INGOT_ADAMANTITE, 8),
        new Recipe(
            ITEM_TYPE.LEGGINGS_ADAMANTITE, 1,
            ITEM_TYPE.INGOT_ADAMANTITE, 7),
        new Recipe(
            ITEM_TYPE.BOOTS_ADAMANTITE, 1,
            ITEM_TYPE.INGOT_ADAMANTITE, 4),

    };

    private Outline _outline;
    private void Awake()
    {
        ResourceManager.Builds.Add(GetHashCode(), this);

        GetComponent<Damagable>().Initialize(new LootTable(new List<LootTableItem>()
        {
            new()
            {
                ItemType = ITEM_TYPE.DEPLOYABLE_ANVIL,
                maxAmount = 1,
                minAmount = 1,
                dropChance = 100,
                useStats = false
            }
        }), 100);
        
        _outline = transform.GetChild(0).GetComponent<Outline>();
        _outline.enabled = false;
    }

    public void OnEnable()
    {
        ResourceManager.Builds.TryAdd(GetHashCode(), this);
    }

    private void OnDestroy()
    {
        ResourceManager.Builds.Remove(GetHashCode());
    }

    private void OnDisable()
    {
        ResourceManager.Builds.Remove(GetHashCode());
    }
    
    public INTERACTABLE_TYPE GetInteractableType()
    {
        return INTERACTABLE_TYPE.ANVIL;
    }

    public void Interact(IPlayer player)
    {
    }

    public Transform Transform => transform;
    public void OnHit(IDamager damager, Vector3 hitDirection, Vector3 hitPosition, TOOL_TYPE toolType, int damage)
    {
        GetComponent<Damagable>().OnHit(damager, hitDirection, hitPosition, toolType, damage);
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
