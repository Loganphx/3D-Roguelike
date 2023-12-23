using System;
using System.Collections.Generic;
using UnityEngine;

public interface IDeployable
{
  public void Deploy(IPlayer player, ITEM_TYPE itemType);
}

public class FarmPlot : MonoBehaviour, IHoverable, IDeployable
{
  private GameObject _mockSeedTransform;
  private Crop _crop;

  public void OnHoverEnter(IPlayer player)
  {
    var currentItem = player.GetCurrentWeapon();
    if (currentItem == ITEM_TYPE.NULL) return;
    if (!ItemToCrops.ContainsKey(currentItem)) return;

    Debug.Log($"Hovering over seed: {currentItem}");
    _mockSeedTransform = PrefabPool.SpawnedPrefabs[ItemPool.ItemPrefabs[currentItem]];
    _mockSeedTransform.transform.SetParent(transform);
    _mockSeedTransform.transform.localPosition = Vector3.zero + Vector3.up * 0.235f;

    var meshRenderer = _mockSeedTransform.transform.GetChild(0).GetComponent<MeshRenderer>();
    meshRenderer.material = MaterialPool.Materials["Materials/BuildHover"];
  }

  public void OnHoverExit(IPlayer player)
  {
    if (_mockSeedTransform == null) return;

    _mockSeedTransform.transform.SetParent(null);
    _mockSeedTransform.transform.position = new Vector3(0, -10000, 0);
    _mockSeedTransform = null;
  }

  public void Deploy(IPlayer player, ITEM_TYPE seedType)
  {
    if (_crop != null) return;

    if (!ItemToCrops.ContainsKey(seedType)) return;

    Debug.Log($"Planting seed: {seedType}");
    player.RemoveItem(seedType, 1);

    var itemToCrop = ItemToCrops[seedType];
    var prefab = PrefabPool.Prefabs[itemToCrop];
    Debug.Log($"Planting seed: {seedType} with prefab: {prefab}");
    _crop = GameObject
      .Instantiate(prefab, Vector3.zero, Quaternion.identity, transform)
      .GetComponent<Crop>();
    _crop.transform.localPosition = Vector3.zero + Vector3.up * 0.235f;
    _crop.Plant(seedType);
    // transform.Find("Interaction").gameObject.layer = LayerMask.NameToLayer("Default");
  }

  private static readonly Dictionary<ITEM_TYPE, string> ItemToCrops = new Dictionary<ITEM_TYPE, string>()
  {
    { ITEM_TYPE.SEED_WHEAT, "Prefabs/Crops/crop_wheat" },
    { ITEM_TYPE.SEED_FLAX, "Prefabs/Crops/crop_flax" },
  };
}

internal class CropData
{

  public int GrowTime;
  public List<CropPhase> Phases;
}

public struct FarmPlotState
{
  public ITEM_TYPE SeedType;
  public int CurrentGrowTick;
  public int CurrentPhase;
  public bool IsGrown;
}

internal class CropPhase
{
  public float percentage;
  // public string prefabPath;
}
