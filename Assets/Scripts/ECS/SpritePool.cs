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
      
      { "Sprites/Items/mushroom_healshroom", Resources.Load<Sprite>("Sprites/Items/mushroom_healshroom") },
      { "Sprites/Items/mushroom_zoomshroom", Resources.Load<Sprite>("Sprites/Items/mushroom_zoomshroom") },
      { "Sprites/Items/mushroom_munchshroom", Resources.Load<Sprite>("Sprites/Items/mushroom_munchshroom") },
            
      { "Sprites/Powerups/powerup_common", Resources.Load<Sprite>("Sprites/Powerups/powerup_common") },
      { "Sprites/Powerups/powerup_uncommon", Resources.Load<Sprite>("Sprites/Powerups/powerup_uncommon") },
      { "Sprites/Powerups/powerup_rare", Resources.Load<Sprite>("Sprites/Powerups/powerup_rare") },
      { "Sprites/Powerups/powerup_epic", Resources.Load<Sprite>("Sprites/Powerups/powerup_epic") },
      { "Sprites/Powerups/powerup_legendary", Resources.Load<Sprite>("Sprites/Powerups/powerup_legendary") },
      
      { "Sprites/Items/seed_wheat", Resources.Load<Sprite>("Sprites/Items/seed_wheat") },
      { "Sprites/Items/seed_flax", Resources.Load<Sprite>("Sprites/Items/seed_flax") },
      
      { "Sprites/Items/wheat", Resources.Load<Sprite>("Sprites/Items/wheat") },
      { "Sprites/Items/flax", Resources.Load<Sprite>("Sprites/Items/flax") },
            
      { "Sprites/Items/pickaxe_wooden", Resources.Load<Sprite>("Sprites/Items/pickaxe_wooden") },
      { "Sprites/Items/pickaxe_iron", Resources.Load<Sprite>("Sprites/Items/pickaxe_iron") },
      { "Sprites/Items/pickaxe_gold", Resources.Load<Sprite>("Sprites/Items/pickaxe_gold") },
      { "Sprites/Items/pickaxe_mythril", Resources.Load<Sprite>("Sprites/Items/pickaxe_mythril") },
      { "Sprites/Items/pickaxe_adamantite", Resources.Load<Sprite>("Sprites/Items/pickaxe_adamantite") },
      
      { "Sprites/Items/axe_wooden", Resources.Load<Sprite>("Sprites/Items/axe_wooden") },
      { "Sprites/Items/axe_iron", Resources.Load<Sprite>("Sprites/Items/axe_iron") },
      { "Sprites/Items/axe_mythril", Resources.Load<Sprite>("Sprites/Items/axe_mythril") },
      { "Sprites/Items/axe_adamantite", Resources.Load<Sprite>("Sprites/Items/axe_adamantite") },
   
      { "Sprites/Items/sword_wooden", Resources.Load<Sprite>("Sprites/Items/sword_wooden") },
      { "Sprites/Items/sword_iron", Resources.Load<Sprite>("Sprites/Items/sword_iron") },
      { "Sprites/Items/sword_mythril", Resources.Load<Sprite>("Sprites/Items/sword_mythril") },
      { "Sprites/Items/sword_adamantite", Resources.Load<Sprite>("Sprites/Items/sword_adamantite") },
      
      { "Sprites/Items/deployable_farm_planter", Resources.Load<Sprite>("Sprites/Items/deployable_farm_planter") },
      { "Sprites/Items/deployable_crafting_station", Resources.Load<Sprite>("Sprites/Items/deployable_crafting_station") },
      { "Sprites/Items/deployable_furnace", Resources.Load<Sprite>("Sprites/Items/deployable_furnace") },
      { "Sprites/Items/deployable_cauldron", Resources.Load<Sprite>("Sprites/Items/deployable_cauldron") },
      { "Sprites/Items/deployable_chest", Resources.Load<Sprite>("Sprites/Items/deployable_chest") },
      
      { "Sprites/Items/building_foundation", Resources.Load<Sprite>("Sprites/Items/building_foundation") },
      { "Sprites/Items/building_foundation_triangle", Resources.Load<Sprite>("Sprites/Items/building_foundation_triangle") },
      { "Sprites/Items/building_wall", Resources.Load<Sprite>("Sprites/Items/building_wall") },
      { "Sprites/Items/building_ramp", Resources.Load<Sprite>("Sprites/Items/building_ramp") },
      
      { "Sprites/Items/bowl", Resources.Load<Sprite>("Sprites/Items/bowl") },
      { "Sprites/Items/bark", Resources.Load<Sprite>("Sprites/Items/bark") },
      
      { "Sprites/Items/arrow_stone", Resources.Load<Sprite>("Sprites/Items/arrow_stone") },
      { "Sprites/Items/arrow_iron", Resources.Load<Sprite>("Sprites/Items/arrow_iron") },
      { "Sprites/Items/arrow_mythril", Resources.Load<Sprite>("Sprites/Items/arrow_mythril") },
      { "Sprites/Items/arrow_adamantite", Resources.Load<Sprite>("Sprites/Items/arrow_adamantite") },
      
      { "Sprites/Items/helmet_iron", Resources.Load<Sprite>("Sprites/Items/helmet_iron") },
      { "Sprites/Items/chestplate_iron", Resources.Load<Sprite>("Sprites/Items/chestplate_iron") },
      { "Sprites/Items/leggings_iron", Resources.Load<Sprite>("Sprites/Items/leggings_iron") },
      { "Sprites/Items/boots_iron", Resources.Load<Sprite>("Sprites/Items/boots_iron") },
            
      { "Sprites/Items/helmet_mythril", Resources.Load<Sprite>("Sprites/Items/helmet_mythril") },
      { "Sprites/Items/chestplate_mythril", Resources.Load<Sprite>("Sprites/Items/chestplate_mythril") },
      { "Sprites/Items/leggings_mythril", Resources.Load<Sprite>("Sprites/Items/leggings_mythril") },
      { "Sprites/Items/boots_mythril", Resources.Load<Sprite>("Sprites/Items/boots_mythril") },

      { "Sprites/Items/helmet_adamantite", Resources.Load<Sprite>("Sprites/Items/helmet_adamantite") },
      { "Sprites/Items/chestplate_adamantite", Resources.Load<Sprite>("Sprites/Items/chestplate_adamantite") },
      { "Sprites/Items/leggings_adamantite", Resources.Load<Sprite>("Sprites/Items/leggings_adamantite") },
      { "Sprites/Items/boots_adamantite", Resources.Load<Sprite>("Sprites/Items/boots_adamantite") },
      
      { "Sprites/Items/rope", Resources.Load<Sprite>("Sprites/Items/rope") },
      
      { "Sprites/Items/bow_wooden", Resources.Load<Sprite>("Sprites/Items/bow_wooden") },
      { "Sprites/Items/bow_iron", Resources.Load<Sprite>("Sprites/Items/bow_iron") },
      { "Sprites/Items/bow_mythril", Resources.Load<Sprite>("Sprites/Items/bow_mythril") },
      { "Sprites/Items/bow_adamantite", Resources.Load<Sprite>("Sprites/Items/bow_adamantite") },
      
      
    }; 
  }

  private static Sprite LoadSprite(string path)
  {
    var sprite = Resources.Load<Sprite>(path);
    if (sprite == null) throw new FileNotFoundException($"Failed to load sprite @ {path}");
    return sprite;
  }
}