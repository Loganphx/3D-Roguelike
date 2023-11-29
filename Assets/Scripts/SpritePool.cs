using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SpritePool
{
  public static Dictionary<string, Sprite> Sprites;

  public static void LoadSprites()
  {
    Sprites = new Dictionary<string, Sprite>()
    {
      { "Sprites/Untitled", Resources.Load<Sprite>("Sprites/Untitled") },
      { "Sprites/Items/rock", Resources.Load<Sprite>("Sprites/Items/rock") },
      { "Sprites/Items/coin", Resources.Load<Sprite>("Sprites/Items/coin") },

      { "Sprites/Items/ore_coal", Resources.Load<Sprite>("Sprites/Items/ore_coal") },
      { "Sprites/Items/ore_iron", Resources.Load<Sprite>("Sprites/Items/ore_iron") },
      { "Sprites/Items/ore_gold", Resources.Load<Sprite>("Sprites/Items/ore_gold") },
      { "Sprites/Items/ore_mythril", Resources.Load<Sprite>("Sprites/Items/ore_mythril") },
      { "Sprites/Items/ore_adamantite", Resources.Load<Sprite>("Sprites/Items/ore_adamantite") },

      { "Sprites/Items/wood_tree", Resources.Load<Sprite>("Sprites/Items/wood_tree") },
      { "Sprites/Items/wood_birch", Resources.Load<Sprite>("Sprites/Items/wood_birch") },
      { "Sprites/Items/wood_fir", Resources.Load<Sprite>("Sprites/Items/wood_fir") },
      { "Sprites/Items/wood_oak", Resources.Load<Sprite>("Sprites/Items/wood_oak") },
      { "Sprites/Items/wood_darkoak", Resources.Load<Sprite>("Sprites/Items/wood_darkoak") },

      { "Sprites/Powerups/powerup_common", Resources.Load<Sprite>("Sprites/Powerups/powerup_common") },
      { "Sprites/Powerups/powerup_uncommon", Resources.Load<Sprite>("Sprites/Powerups/powerup_uncommon") },
      { "Sprites/Powerups/powerup_rare", Resources.Load<Sprite>("Sprites/Powerups/powerup_rare") },
      { "Sprites/Powerups/powerup_epic", Resources.Load<Sprite>("Sprites/Powerups/powerup_epic") },
      { "Sprites/Powerups/powerup_legendary", Resources.Load<Sprite>("Sprites/Powerups/powerup_legendary") },
      
      { "Sprites/Items/seed_wheat", Resources.Load<Sprite>("Sprites/Items/seed_wheat") },
      { "Sprites/Items/seed_flax", Resources.Load<Sprite>("Sprites/Items/seed_flax") },
      
      { "Sprites/Items/building_foundation", Resources.Load<Sprite>("Sprites/Items/building_foundation") },
      { "Sprites/Items/building_wall", Resources.Load<Sprite>("Sprites/Items/building_wall") },
    };
  }

  private static Sprite LoadSprite(string path)
  {
    var sprite = Resources.Load<Sprite>(path);
    if (sprite == null) throw new FileNotFoundException($"Failed to load sprite @ {path}");
    return sprite;
  }
}