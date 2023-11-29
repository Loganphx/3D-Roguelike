using UnityEngine;

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
  
  SEED_WHEAT,
  SEED_FLAX,
  
  DEPLOYABLE_FARM_PLANTER,
  
  BUILDING_FOUNDATION,
  BUILDING_FOUNDATION_TRIANGLE,
  BUILDING_WALL,
  BUILDING_FLOOR,
  BUILDING_RAMP,
}

public class Item : MonoBehaviour, IInteractable, IHoverable
{
  // Start is called before the first frame update
  private Outline _outline;

  [SerializeField] private ITEM_TYPE itemType;

  private void Awake()
  {
    if(itemType == ITEM_TYPE.NULL) return;
    
    _outline = transform.GetChild(0).GetComponent<Outline>();
    _outline.enabled = false;
  }

  public void Start()
  {
    if(itemType == ITEM_TYPE.NULL) return;
    
    var interaction = transform.GetChild(0).Find("Interaction");
    interaction.gameObject.layer = LayerMask.NameToLayer("Interactable");

    GetComponent<Rigidbody>().isKinematic = false;
  }

  public void Interact(IPlayer player)
  {
    Debug.Log("Interacting with item");
    player.AddItem(itemType, 1);
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

  public void SetItemType(ITEM_TYPE itemType)
  {
    this.itemType = itemType;
    GetComponent<Rigidbody>().isKinematic = false;
    
    Awake();
    Start();
  }
}