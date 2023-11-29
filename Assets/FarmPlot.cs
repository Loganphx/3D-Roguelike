using System.Collections.Generic;
using UnityEngine;

public interface IDeployable
{
  public void Deploy(IPlayer player, ITEM_TYPE itemType);
}

public class FarmPlot : MonoBehaviour, IHoverable, IDeployable
{
  private GameObject _mockSeedTransform;

  public void OnHoverEnter(IPlayer player)
  {
    var currentItem = player.GetCurrentWeapon();
    if(currentItem == ITEM_TYPE.NULL) return;
    if(!_cropData.ContainsKey(currentItem)) return;
    
    Debug.Log($"Hovering over seed: {currentItem}");
    _mockSeedTransform = PrefabPool.SpawnedPrefabs[ItemPool.ItemPrefabs[currentItem]];
    _mockSeedTransform.transform.SetParent(transform);
    _mockSeedTransform.transform.localPosition = Vector3.zero + Vector3.up * 0.235f;
    
    var meshRenderer =_mockSeedTransform.transform.GetChild(0).GetComponent<MeshRenderer>();
    meshRenderer.material = MaterialPool.Materials["Materials/BuildHover"];
  }

  public void OnHoverExit(IPlayer player)
  {
    if(_mockSeedTransform == null) return;
    
    _mockSeedTransform.transform.SetParent(null);
    _mockSeedTransform.transform.position = new Vector3(0,-10000,0);
    _mockSeedTransform = null;
  }

  public void Deploy(IPlayer player, ITEM_TYPE seedType)
  {
    if(!_cropData.ContainsKey(seedType)) return;
    
    Debug.Log($"Planting seed: {seedType}");
    player.RemoveItem(seedType, 1);

    var cropModel = GameObject.Instantiate(PrefabPool.Prefabs[ItemPool.ItemPrefabs[seedType]], Vector3.zero, Quaternion.identity,
      transform);
    cropModel.transform.localPosition = Vector3.zero + Vector3.up * 0.235f;

    transform.Find("Interaction").gameObject.layer = LayerMask.NameToLayer("Default");
  }


  private readonly Dictionary<ITEM_TYPE, CropData> _cropData = new Dictionary<ITEM_TYPE, CropData>()
  {
    {ITEM_TYPE.SEED_WHEAT, null},
    {ITEM_TYPE.SEED_FLAX, null},
  };
}
internal class CropData {

}
