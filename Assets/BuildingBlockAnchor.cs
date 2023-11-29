using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBlockAnchor : MonoBehaviour, IDeployable, IHoverable
{

  private GameObject _mockSeedTransform;
  public void Deploy(IPlayer player, ITEM_TYPE itemType)
  {
    if(!_cropData.ContainsKey(itemType)) return;
    
    Debug.Log($"Placing building block: {itemType}");
      
    // Collision Check before placing.
    var box = _mockSeedTransform.transform.GetChild(0);

    var startPos = box.position - new Vector3(0, box.localScale.y / 2f, 0);
    Debug.DrawLine(startPos, startPos + box.up, Color.blue, 5f);
    var invalid = Physics.BoxCast(startPos, box.localScale / 2, box.up, out var hit, box.rotation, 1);
    if (invalid) return;
    
    player.RemoveItem(itemType, 1);
      
    var buildingBlock = GameObject.Instantiate(PrefabPool.Prefabs[_cropData[itemType]]);
    
    var transform1 = transform;
    buildingBlock.transform.position = transform1.position;
    buildingBlock.transform.forward = -transform1.right;
    
    // Find Nearest Building Blocks
    // Turn off Anchors if they connect the pieces
    var buildingBlockComponent = buildingBlock.GetComponent<BuildingBlock>();
    buildingBlockComponent.Awake();
    buildingBlockComponent.Start();
    gameObject.SetActive(false);
    
  }

  public void OnHoverEnter(IPlayer player)
  {
    var currentItem = player.GetCurrentWeapon();
    if(currentItem == ITEM_TYPE.NULL) return;
    if(!_cropData.ContainsKey(currentItem)) return;
    
    Debug.Log($"Hovering over seed: {currentItem}");
    _mockSeedTransform = PrefabPool.SpawnedPrefabs[_cropData[currentItem]];
    _mockSeedTransform.transform.SetParent(transform);
    _mockSeedTransform.transform.localPosition = Vector3.zero;
    _mockSeedTransform.transform.forward = -transform.right;

    var box = _mockSeedTransform.transform.GetChild(0);
    var meshRenderer =box.GetComponent<MeshRenderer>();
    
    var startPos = box.position - new Vector3(0, box.localScale.y / 2f, 0);
    Debug.DrawLine(startPos, startPos + box.up, Color.blue, 5f);
    var invalid = Physics.BoxCast(startPos, box.localScale / 2, box.up, out var hit, box.rotation, 1);
    if (invalid)
    {
      Debug.Log($"Invalid placement: {hit.collider.gameObject}");
      meshRenderer.material = MaterialPool.Materials["Materials/InvalidBuildHover"];
    }
    else meshRenderer.material = MaterialPool.Materials["Materials/BuildHover"];
  }

  public void OnHoverExit(IPlayer player)
  {
    if(_mockSeedTransform == null) return;
    
    _mockSeedTransform.transform.SetParent(null);
    _mockSeedTransform.transform.position = new Vector3(0,-10000,0);
    _mockSeedTransform = null;
  }
    
  private static readonly Dictionary<ITEM_TYPE, string> _cropData = new Dictionary<ITEM_TYPE, string>()
  {
    {ITEM_TYPE.BUILDING_FOUNDATION, "Prefabs/Buildings/building_foundation"},
    {ITEM_TYPE.BUILDING_WALL, "Prefabs/Buildings/building_wall"},
  };
  
  // private static readonly Dictionary<ITEM_TYPE, string> _blueprintData = new Dictionary<ITEM_TYPE, string>()
  // {
  //   {ITEM_TYPE.BUILDING_FOUNDATION, "Prefabs/Buildings/blueprint_building_foundation"},
  //   {ITEM_TYPE.BUILDING_WALL, "Prefabs/Buildings/blueprint_building_wall"},
  // };
}
