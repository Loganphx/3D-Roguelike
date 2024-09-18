using UnityEngine;

public class BuildingBlockAnchor : MonoBehaviour, IDeployable, IHoverable
{

  private Transform _mockSeedTransform;
  private static Collider[] _hits = new Collider[5];
  public void Deploy(IPlayer player, ITEM_TYPE itemType)
  {
    if(!IDeployable.ItemToDeployable[typeof(BuildingBlock)].ContainsKey(itemType)) return;
    
    Debug.Log($"Placing building block: {itemType}");
      
    // Collision Check before placing.
    var box = _mockSeedTransform.GetChild(0);

    var localScale = box.localScale;
    //Debug.DrawLine(startPos, startPos + box.up, Color.blue, 5f);
    var hits = Physics.OverlapBoxNonAlloc(_mockSeedTransform.position, localScale / 2, _hits, box.rotation, ~(1 << LayerMask.NameToLayer("Ground")), QueryTriggerInteraction.Ignore);
    if (hits > 0) return;
    
    player.RemoveItem(itemType, 1);
      
    var buildingBlock = GameObject.Instantiate(PrefabPool.Prefabs[IDeployable.ItemToDeployable[typeof(BuildingBlock)][itemType]]).transform;
    
    buildingBlock.position = _mockSeedTransform.position;
    buildingBlock.forward = _mockSeedTransform.forward;
    
    // Find Nearest Building Blocks
    // Turn off Anchors if they connect the pieces
    var buildingBlockComponent = buildingBlock.GetComponent<BuildingBlock>();
    // buildingBlockComponent.Awake();
    // buildingBlockComponent.Start();
    gameObject.SetActive(false);
    
  }

  public void OnHoverEnter(IPlayer player)
  {
    var currentItem = player.GetCurrentWeapon();
    if(currentItem == ITEM_TYPE.NULL) return;
    if(!IDeployable.ItemToDeployable[typeof(BuildingBlock)].ContainsKey(currentItem)) return;
    
    Debug.Log($"Hovering over seed: {currentItem}");
    _mockSeedTransform = PrefabPool.SpawnedPrefabs[IDeployable.ItemToDeployable[typeof(BuildingBlock)] [currentItem]].transform;
    _mockSeedTransform.SetParent(transform);
    var box = _mockSeedTransform.GetChild(0);
    var localScale = box.localScale;
    _mockSeedTransform.forward = transform.forward;
    var offset = _mockSeedTransform.forward * localScale.x/2;
    _mockSeedTransform.position = transform.position + offset;

    var meshRenderer =box.GetComponent<MeshRenderer>();

    var startPos = _mockSeedTransform.position;
    //Debug.DrawLine(startPos, startPos + box.up, Color.blue, 5f);
    var hits = Physics.OverlapBoxNonAlloc(startPos, localScale / 2.0f, _hits, box.rotation, ~(1 << LayerMask.NameToLayer("Ground")), QueryTriggerInteraction.Ignore);
    if (hits > 0)
    {
      Debug.Log($"Invalid placement ({hits}): {_hits[0].transform.root} - {_hits[0].transform}");
      Debug.DrawLine(startPos, _hits[0].transform.position, Color.red, 2);
      meshRenderer.material = MaterialPool.Materials["Materials/InvalidBuildHover"];
    }
    else meshRenderer.material = MaterialPool.Materials["Materials/BuildHover"];
  }

  public void OnHoverExit(IPlayer player)
  {
    if(_mockSeedTransform == null) return;
    
    _mockSeedTransform.SetParent(null);
    _mockSeedTransform.position = new Vector3(0,-10000,0);
    _mockSeedTransform = null;
  }
  
  // private static readonly Dictionary<ITEM_TYPE, string> _blueprintData = new Dictionary<ITEM_TYPE, string>()
  // {
  //   {ITEM_TYPE.BUILDING_FOUNDATION, "Prefabs/Buildings/blueprint_building_foundation"},
  //   {ITEM_TYPE.BUILDING_WALL, "Prefabs/Buildings/blueprint_building_wall"},
  // };
}
