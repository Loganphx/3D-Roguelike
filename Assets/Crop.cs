using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Crop : MonoBehaviour, IInteractable, IHoverable
{
  private Outline _outline;

  private GameObject _cropModel;
  private CropData _cropData;

  private FarmPlotState _state;
  private Image _progressBar;
  private Canvas _canvas;

  private void Awake()
  {
    _outline = transform.GetComponent<Outline>();
    _outline.enabled = false;

    _canvas = transform.Find("Canvas").GetComponent<Canvas>();
    _progressBar = _canvas.transform.Find("ProgressBar").Find("Fill").GetComponent<Image>();
    _progressBar.enabled = false;
  }

  public void Plant(ITEM_TYPE seedType)
  {
    ref var state = ref _state;
    _cropData = _crops[seedType];

    state.SeedType = seedType;
    state.CurrentPhase = 0;
    state.CurrentGrowTick = 0;

    _cropModel = transform.GetChild(0).gameObject;
    
    transform.Find("Interaction").gameObject.layer = LayerMask.NameToLayer("Default");
  }

  private void FixedUpdate()
  {
    ref var state = ref _state;

    if (state.SeedType == ITEM_TYPE.NULL) return;
    if (state.IsGrown) return;

    state.CurrentGrowTick++;
    var percentage = state.CurrentGrowTick / (float)_cropData.GrowTime;

    if (percentage >= 1.0f)
    {
      state.IsGrown = true;
      state.CurrentPhase = _cropData.Phases.Count - 1;
      transform.Find("Interaction").gameObject.layer = LayerMask.NameToLayer("Interactable");
      return;
    }

    for (var i = state.CurrentPhase; i < _cropData.Phases.Count; i++)
    {
      var phase = _cropData.Phases[i];
      if (percentage < phase.percentage)
      {
        if (state.CurrentPhase != i)
        {
          // if (state.CurrentPhase > 0) 
          {
            _cropModel.SetActive(false);
            //GameObject.Destroy(_cropModel);
          }

          state.CurrentPhase = i;
          // var cropPhase = _cropData.Phases[state.CurrentPhase];
          Debug.Log($"{i}/{_cropData.Phases.Count}");
          Debug.Log($"{transform.GetChild(i)}");
          var crop = transform.GetChild(i).gameObject;

          // var cropPrefab = PrefabPool.Prefabs[cropPhase.prefabPath];
          // var crop = GameObject.Instantiate(cropPrefab, Vector3.zero, Quaternion.identity, transform);
          crop.transform.localPosition = Vector3.zero + Vector3.up * 0.235f;

          Debug.Log(this,this);
          crop.SetActive(true);
          _cropModel = crop;
        }

        break;
      }
    }
  }

  public INTERACTABLE_TYPE GetInteractableType()
  {
    return INTERACTABLE_TYPE.CROP;
  }

  public void Interact(IPlayer player)
  {
    Debug.Log("Interacting with crop");
    ref var state = ref _state;

    if (state.IsGrown)
    {
      // Pickup 
      // var t = player.AddItem(_cropData.Product, _cropData.ProductQuantity);
      Debug.Log($"{state.SeedType} Crop died");
      // var dropPrefab = PrefabPool.Prefabs[_nodeData.itemType];
      var position = transform.position;
      var dropPosition = new Vector3(position.x, position.y + 0.15f, position.z);
      var itemTemplatePrefab = PrefabPool.Prefabs["Prefabs/Items/item_template"];
      var itemTemplate = Instantiate(itemTemplatePrefab, dropPosition, Quaternion.identity);
      var itemPrefab = PrefabPool.Prefabs[ItemPool.ItemPrefabs[_cropData.Product]];

      var item = Instantiate(itemPrefab, Vector3.zero,
        Quaternion.identity, itemTemplate.transform);

      item.transform.localPosition = Vector3.zero;
    
      itemTemplate.GetComponent<Item>().SetItemType(_cropData.Product);

      // var drop = GameObject.Instantiate(dropPrefab, dropPosition, Quaternion.identity);
      var hitDirection = player.Transform.forward;
      hitDirection.y = 1;
      itemTemplate.GetComponent<Rigidbody>().AddForce(hitDirection * 3f, ForceMode.Impulse);
      // drop.GetComponent<Rigidbody>().AddForce(hitDirection * 10f, ForceMode.Impulse);

      //drop.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
      gameObject.SetActive(false);
    }
  }

  private readonly Dictionary<ITEM_TYPE, CropData> _crops = new Dictionary<ITEM_TYPE, CropData>()
  {
    {
      ITEM_TYPE.SEED_WHEAT, new CropData()
      {
        GrowTime = 6000,
        Phases = new List<CropPhase>()
        {
          new CropPhase()
          {
            percentage = 0.0f,
            // prefabPath = "Prefabs/Crops/crop_wheat_0"
          },
          new CropPhase()
          {
            percentage = 0.3f,
            // prefabPath = "Prefabs/Crops/crop_wheat_1"
          },
          new CropPhase()
          {
            percentage = 0.5f,
            // prefabPath = "Prefabs/Crops/crop_wheat_2"
          },
          new CropPhase()
          {
            percentage = 0.75f,
            // prefabPath = "Prefabs/Crops/crop_wheat_3"
          },
          new CropPhase()
          {
            percentage = 1.0f,
            // prefabPath = "Prefabs/Crops/crop_wheat_4"
          },
        },
        Product = ITEM_TYPE.WHEAT,
        ProductQuantity = 5
      }
    },
    {
      ITEM_TYPE.SEED_FLAX, new CropData()
      {
        GrowTime = 6000,
        Phases = new List<CropPhase>()
        {
          new CropPhase()
          {
            percentage = 0.0f,
            // prefabPath = "Prefabs/Crops/crop_wheat_0"
          },
          new CropPhase()
          {
            percentage = 0.3f,
            // prefabPath = "Prefabs/Crops/crop_wheat_1"
          },
          new CropPhase()
          {
            percentage = 0.5f,
            // prefabPath = "Prefabs/Crops/crop_wheat_2"
          },
          new CropPhase()
          {
            percentage = 0.75f,
            // prefabPath = "Prefabs/Crops/crop_wheat_3"
          },
          new CropPhase()
          {
            percentage = 1.0f,
            // prefabPath = "Prefabs/Crops/crop_wheat_4"
          },
        },
        Product = ITEM_TYPE.FLAX,
        ProductQuantity = 3
      }
    }
  };

  public void OnHoverEnter(IPlayer player)
  {
    ref var state = ref _state;

    _outline.enabled = true;
    _canvas.enabled = true;
    _progressBar.fillAmount = state.CurrentGrowTick / (float)_cropData.GrowTime;
  }

  public void OnHoverExit(IPlayer player)
  {
    _outline.enabled = false;
    _canvas.enabled = false;
  }
}