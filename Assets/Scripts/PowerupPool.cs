using System.Collections.Generic;

public static class PowerupPool
{
  public static readonly Dictionary<POWERUP_TYPE, string> PowerupSprites = new Dictionary<POWERUP_TYPE, string>()
  {
    { POWERUP_TYPE.NULL, null },
    { POWERUP_TYPE.COMMON, "Sprites/Powerups/powerup_common" },
    { POWERUP_TYPE.UNCOMMON, "Sprites/Powerups/powerup_uncommon" },
    { POWERUP_TYPE.RARE, "Sprites/Powerups/powerup_rare" },
    { POWERUP_TYPE.EPIC, "Sprites/Powerups/powerup_epic" },
    { POWERUP_TYPE.LEGENDARY, "Sprites/Powerups/powerup_legendary" },
  };

  public static readonly Dictionary<POWERUP_TYPE, string> PowerupPrefabs = new Dictionary<POWERUP_TYPE, string>()
  {
    { POWERUP_TYPE.NULL, null },
    { POWERUP_TYPE.COMMON, "Prefabs/Powerups/powerup_common" },
    { POWERUP_TYPE.UNCOMMON, "Prefabs/Powerups/powerup_uncommon" },
    { POWERUP_TYPE.RARE, "Prefabs/Powerups/powerup_rare" },
    { POWERUP_TYPE.EPIC, "Prefabs/Powerups/powerup_epic" },
    { POWERUP_TYPE.LEGENDARY, "Prefabs/Powerups/powerup_legendary" },
  };
}