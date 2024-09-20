using System.Collections.Generic;
using ECS.Movement.Services;
using TMPro;
using UnityEngine;

internal class PlayerUIStatsComponent
{
    private Dictionary<STAT_TYPE, TMP_Text> stats;

    public PlayerUIStatsComponent(GameObject statsPanel)
    {
        stats = new Dictionary<STAT_TYPE, TMP_Text>();

        for (int i = 0; i < statsPanel.transform.childCount; i++)
        {
            stats.Add((STAT_TYPE)i, statsPanel.transform.GetChild(i).GetChild(0).GetComponent<TMP_Text>());
        }
    }

    public void OnFixedUpdate(ref PlayerHealthState healthState, ref PlayerAttackState attackState, ref PlayerMovementState movementState)
    {
        if (healthState.HasChanged)
        {
            stats[STAT_TYPE.HEALTH].SetText(healthState.Health.ToString());
            stats[STAT_TYPE.SHIELD].SetText(healthState.Shield.ToString());
            stats[STAT_TYPE.ARMOR].SetText(healthState.Armor.ToString());
        }

        if (attackState.HasChanged)
        {
            stats[STAT_TYPE.STRENGTH].SetText(attackState.Strength.ToString());
            stats[STAT_TYPE.CRIT_CHANCE].SetText(attackState.CritChance.ToString("F"));
            stats[STAT_TYPE.ATTACK_SPEED].SetText(attackState.AttackSpeed.ToString("F"));
            stats[STAT_TYPE.MAX_HIT].SetText(attackState.MaxHit.ToString());
        }

        if (movementState.HasChanged)
        {
            // TODO REFORMAT TO DISPLAY IN M/S
            stats[STAT_TYPE.SPEED].SetText(movementState.movementSpeed.ToString("F"));
            stats[STAT_TYPE.MAX_JUMPS].SetText(movementState.maxJumps.ToString());
        }
    }
  
  
}