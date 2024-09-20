using System;
using UnityEngine;

[Flags]
public enum TOOL_TYPE
{
  NULL = 0,
  AXE,
  PICKAXE,
  SHOVEL,
  SCYTHE,
}
public enum ITEM_TYPE
{
  NULL = 0,
  WOOD,
  WOOD_BIRCH,
  WOOD_FIR,
  WOOD_OAK,
  WOOD_DARKOAK,
  ROCK,
  ORE_COAL,
  ORE_IRON,
  ORE_GOLD,
  ORE_ADAMANTITE,
  ORE_MYTHRIL,
  
  COIN,
  
  MEAT_RAW,
  
  HIDE_WOLF,
  
  SEED_WHEAT,
  SEED_FLAX,
  
  MUSHROOM_HEALSHROOM,
  MUSHROOM_MUNCHSHROOM,
  MUSHROOM_ZOOMSHROOM,
  
  DEPLOYABLE_FARM_PLANTER,
  DEPLOYABLE_CRAFTING_STATION,
  DEPLOYABLE_FURNACE,
  DEPLOYABLE_CAULDRON,
  DEPLOYABLE_CHEST,
  
  BUILDING_FOUNDATION,
  BUILDING_FOUNDATION_TRIANGLE,
  BUILDING_WALL,
  BUILDING_FLOOR,
  BUILDING_RAMP,
  
  TOOL_PICKAXE_WOODEN,
  TOOL_PICKAXE_IRON,
  TOOL_PICKAXE_MYTHRIL,
  TOOL_PICKAXE_ADAMANTITE,
  
  TOOL_AXE_WOODEN,
  TOOL_AXE_IRON,
  TOOL_AXE_MYTHRIL,
  TOOL_AXE_ADAMANTITE,
  
  BARK,
  BOWL,
  
  ARROW_STONE,
  ARROW_IRON,
  ARROW_MYTHRIL,
  ARROW_ADAMANTITE,
  
  INGOT_IRON,
  INGOT_MYTHRIL,
  INGOT_ADAMANTITE,
  
  WHEAT,
  FLAX,
  
  // ARMOR
  HELMET_IRON,
  CHESTPLATE_IRON,
  LEGGINGS_IRON,
  BOOTS_IRON,
  HELMET_MYTHRIL,
  CHESTPLATE_MYTHRIL,
  LEGGINGS_MYTHRIL,
  BOOTS_MYTHRIL,
  HELMET_ADAMANTITE,
  CHESTPLATE_ADAMANTITE,
  LEGGINGS_ADAMANTITE,
  BOOTS_ADAMANTITE
}

public class Item : MonoBehaviour, IInteractable, IHoverable
{
  // Start is called before the first frame update
  private Outline _outline;

  [SerializeField] private ITEM_TYPE itemType;
  private int amount; 
  private void Awake()
  {
    if(itemType == ITEM_TYPE.NULL) return;
    
    Debug.Log($"Item {itemType} awake");
    _outline = transform.GetChild(0).GetComponent<Outline>();
    _outline.enabled = false;
    
    // HitboxSystem.ColliderHashToDamagable.Add(transform.Find("Interaction").GetComponent<Collider>().GetInstanceID(), this);
  }

  public void Start()
  {
    if(itemType == ITEM_TYPE.NULL) return;
    
    var interaction = transform.GetChild(0).Find("Interaction");
    interaction.gameObject.layer = LayerMask.NameToLayer("Interactable");

    GetComponent<Rigidbody>().isKinematic = false;
  }

  public INTERACTABLE_TYPE GetInteractableType()
  {
    return INTERACTABLE_TYPE.ITEM;
  }

  public void Interact(IPlayer player)
  {
    Debug.Log("Interacting with item");
    var remainingAmount = player.AddItem(itemType, 1);
    if(remainingAmount > 0) return;
    
    gameObject.SetActive(false);
  }

  public void OnHoverEnter(IPlayer player)
  {
    _outline.enabled = true;
  }

  public void OnHoverExit(IPlayer player)
  {
    _outline.enabled = false;
  }

  public void SetItemType(ITEM_TYPE itemType, int amount)
  {
    this.itemType = itemType;
    this.amount = amount;
    GetComponent<Rigidbody>().isKinematic = false;
    
    Awake();
    Start();
  }
}