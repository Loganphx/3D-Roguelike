using System;
using System.Collections;
using System.Collections.Generic;
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
      return;
    }

    for (var i = state.CurrentPhase; i < _cropData.Phases.Count; i++)
    {
      var phase = _cropData.Phases[i];
      if (percentage < phase.percentage)
      {
        if (state.CurrentPhase != i)
        {
          if (state.CurrentPhase > 0)
          {
            _cropModel.gameObject.SetActive(false);
            //GameObject.Destroy(_cropModel);
          }

          state.CurrentPhase = i;
          // var cropPhase = _cropData.Phases[state.CurrentPhase];
          var crop = transform.GetChild(i).gameObject;

          // var cropPrefab = PrefabPool.Prefabs[cropPhase.prefabPath];
          // var crop = GameObject.Instantiate(cropPrefab, Vector3.zero, Quaternion.identity, transform);
          crop.transform.localPosition = Vector3.zero + Vector3.up * 0.235f;

          Debug.Log(this,this);
          crop.gameObject.SetActive(true);
          _cropModel = crop;
        }

        break;
      }
    }
  }

  public void Interact(IPlayer player)
  {
    Debug.Log("Interacting with crop");
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
        }
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
        }
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