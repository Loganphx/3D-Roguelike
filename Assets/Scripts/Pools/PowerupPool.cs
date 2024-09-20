using System.Collections.Generic;

public static class PowerupPool
{
    public static readonly Dictionary<POWERUP_TYPE, string> PowerupSprites = new Dictionary<POWERUP_TYPE, string>()
    {
        { POWERUP_TYPE.NULL, null },
        { POWERUP_TYPE.INCREASE_HEALTH, "Sprites/Powerups/powerup_common" },
        { POWERUP_TYPE.INCREASE_STAMINA, "Sprites/Powerups/powerup_common" },
        { POWERUP_TYPE.SLOW_HUNGER, "Sprites/Powerups/powerup_common" },
        { POWERUP_TYPE.SHIELD, "Sprites/Powerups/powerup_common" },

        { POWERUP_TYPE.EXTRA_JUMP, "Sprites/Powerups/powerup_uncommon" },

        { POWERUP_TYPE.EXTRA_MELEE_DAMAGE, "Sprites/Powerups/powerup_rare" },

        { POWERUP_TYPE.PIGGY_BANK, "Sprites/Powerups/powerup_epic" },

        { POWERUP_TYPE.THORS_HAMMER, "Sprites/Powerups/powerup_legendary" },
    };

    public static readonly Dictionary<POWERUP_TYPE, string> PowerupPrefabs = new Dictionary<POWERUP_TYPE, string>()
    {
        { POWERUP_TYPE.NULL, null },
        { POWERUP_TYPE.INCREASE_HEALTH, "Prefabs/Powerups/powerup_common" },
        { POWERUP_TYPE.INCREASE_STAMINA, "Prefabs/Powerups/powerup_common" },
        { POWERUP_TYPE.SLOW_HUNGER, "Prefabs/Powerups/powerup_common" },
        { POWERUP_TYPE.SHIELD, "Prefabs/Powerups/powerup_common" },

        { POWERUP_TYPE.EXTRA_JUMP, "Prefabs/Powerups/powerup_uncommon" },

        { POWERUP_TYPE.EXTRA_MELEE_DAMAGE, "Prefabs/Powerups/powerup_rare" },

        { POWERUP_TYPE.PIGGY_BANK, "Prefabs/Powerups/powerup_epic" },

        { POWERUP_TYPE.THORS_HAMMER, "Prefabs/Powerups/powerup_legendary" },
    };
}