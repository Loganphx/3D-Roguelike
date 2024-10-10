using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class LootTableItem
{
    public ITEM_TYPE ItemType;
    public int minAmount;
    public int maxAmount;
    public float dropChance;
    public bool useStats = true;
}
public class LootTable
{
    private List<LootTableItem> items;

    public LootTable(List<LootTableItem> items)
    {
        this.items = items;
    }
    public List<(ITEM_TYPE item, int amount)> GetLoot(IDamager player)
    {
        List<(ITEM_TYPE item, int amount)> generatedLoot = new List<(ITEM_TYPE item, int amount)>();
        foreach (var lootTableItem in items)
        {
            var rand = UnityEngine.Random.Range(0, 100f);
            if (rand < lootTableItem.dropChance)
            {
                var amount = UnityEngine.Random.Range(lootTableItem.minAmount, lootTableItem.maxAmount + 1); 
                generatedLoot.Add((lootTableItem.ItemType, amount));
            }
        }

        return generatedLoot;
    }
}

[DisallowMultipleComponent]
public class Damagable : MonoBehaviour, IDamagable
{
    private int currentHealth;
    private int maxHealth;
    private LootTable lootTable;
    private bool usePlayerStats;
    public Action<Vector3> OnDeath;

    public void Initialize(LootTable lootTable, int maxHealth)
    {
        this.lootTable = lootTable;
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;

        var colliders = transform.GetComponentsInChildren<Collider>();
        foreach (var collider in colliders)
        {
            if (!collider.gameObject.CompareTag("Exclude")) 
                HitboxSystem.ColliderHashToDamagable.Add(collider.GetInstanceID(), this);
        }
    }

    public Transform Transform => transform;

    public virtual void OnHit(IDamager damager, Vector3 hitDirection, Vector3 hitPosition, TOOL_TYPE toolType, int damage)
    {
        if(currentHealth <= 0) return;
    
        currentHealth -= damage;
        // Debug.Log($"Hit {gameObject} for {damage} => {currentHealth} / {maxHealth}");
        if (currentHealth <= 0)
        {
            OnKill(damager, hitDirection);
        }
    }
    public void OnKill(IDamager damager, Vector3 hitDirection)
    {
        var generatedLoot = lootTable.GetLoot(damager);
        this.Death(hitDirection, generatedLoot);
        OnDeath?.Invoke(hitDirection);
    }

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

}