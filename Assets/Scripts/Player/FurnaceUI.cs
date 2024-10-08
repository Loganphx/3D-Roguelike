using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

internal class FurnaceUI : MonoBehaviour
{
    internal static FurnaceUI Instance { get; private set; }

    private ItemSlotUI ingredientSlotUI;
    private ItemSlotUI fuelSlotUI;
    private ItemSlotUI productSlotUI;
    private Image fuelProgressImage;
    private Image smeltProgressImage;
    
    private void Awake()
    {
        Instance = this;

        var panel = transform.Find("Panel");
        fuelProgressImage = panel.Find("FuelTime_Background").Find("FuelTime_Fill").GetComponent<Image>();
        smeltProgressImage = panel.Find("SmeltTime_Background").Find("SmeltTime_Fill").GetComponent<Image>();
        
        GameObject itemSlot = panel.Find("ItemSlot_Ingredient").gameObject;
        var image = itemSlot.transform.GetChild(0).GetComponent<Image>();
            
        ingredientSlotUI = new ItemSlotUI()
        {
            GameObject = itemSlot,
            Image = image,
            AmountText = itemSlot.transform.GetChild(1).GetComponent<TMP_Text>(),
            type = ITEM_TYPE.NULL,
        };
        
        itemSlot = panel.Find("ItemSlot_Fuel").gameObject;
        image = itemSlot.transform.GetChild(0).GetComponent<Image>();
            
        Debug.Log(itemSlot + " " + image, itemSlot);
        fuelSlotUI = new ItemSlotUI()
        {
            GameObject = itemSlot,
            Image = image,
            AmountText = itemSlot.transform.GetChild(1).GetComponent<TMP_Text>(),
            type = ITEM_TYPE.NULL,
        };
        
        itemSlot = panel.Find("ItemSlot_Product").gameObject;
        image = itemSlot.transform.GetChild(0).GetComponent<Image>();
            
        productSlotUI = new ItemSlotUI()
        {
            GameObject = itemSlot,
            Image = image,
            AmountText = itemSlot.transform.GetChild(1).GetComponent<TMP_Text>(),
            type = ITEM_TYPE.NULL,
        };
        
        gameObject.SetActive(false);
    }

    public void Init(BehaviorSubject<(float startTime, float endTime)> onFuelEndTimeChanged, BehaviorSubject<(float startTime, float endTime)> onSmeltEndTimeChanged, 
        BehaviorSubject<InventoryItem> onFuelItemChanged, BehaviorSubject<InventoryItem> onIngredientItemChanged, BehaviorSubject<InventoryItem> onProductItemChanged)
    {
        onFuelEndTimeChanged.TakeUntilDisable(this).Subscribe(OnFuelEndTimeChanged);
        onSmeltEndTimeChanged.TakeUntilDisable(this).Subscribe(OnSmeltEndTimeChanged);
        onFuelItemChanged.TakeUntilDisable(this).Subscribe(OnFuelItemChanged);
        onIngredientItemChanged.TakeUntilDisable(this).Subscribe(OnIngredientItemChanged);
        onProductItemChanged.TakeUntilDisable(this).Subscribe(OnProductItemChanged);
    }
    
    private float _fuelStartTime;
    private float _fuelEndTime;
    

    private void OnFuelEndTimeChanged((float startTime, float endTime) time)
    {
        // Debug.Log("OnFuelEndTimeChanged");
        _fuelStartTime = time.startTime;
        _fuelEndTime = time.endTime;
    }

    private float _smeltStartTime;
    private float _smeltEndtime;
    private void OnSmeltEndTimeChanged((float startTime, float endTime) time)
    {
        // Debug.Log("OnSmeltEndTimeChanged");
        _smeltStartTime = time.startTime;
        _smeltEndtime = time.endTime;
    }

    private void OnFuelItemChanged(InventoryItem fuelItem)
    {
        // Debug.Log("OnFuelItemChanged");
        if (fuelItem.ItemId != ITEM_TYPE.NULL)
        {
            fuelSlotUI.Image.sprite = SpritePool.Sprites[ItemPool.ItemSprites[fuelItem.ItemId]];
            fuelSlotUI.Image.enabled = true;
            fuelSlotUI.AmountText.SetText(fuelItem.Amount.ToString());
        }
        else
        {
            fuelSlotUI.Image.sprite = null;
            fuelSlotUI.Image.enabled = false;
            fuelSlotUI.AmountText.SetText(string.Empty);
        }    
    }

    private void OnIngredientItemChanged(InventoryItem ingredientItem)
    {
        // Debug.Log("OnIngredientItemChanged");
        if (ingredientItem.ItemId != ITEM_TYPE.NULL)
        {
            ingredientSlotUI.Image.sprite = SpritePool.Sprites[ItemPool.ItemSprites[ingredientItem.ItemId]];
            ingredientSlotUI.Image.enabled = true;
            ingredientSlotUI.AmountText.SetText(ingredientItem.Amount.ToString());
        }
        else
        {
            ingredientSlotUI.Image.sprite = null;
            ingredientSlotUI.Image.enabled = false;
            ingredientSlotUI.AmountText.SetText(string.Empty);
        }
    }

    private void OnProductItemChanged(InventoryItem productItem)
    {
        // Debug.Log("OnProductItemChanged");
        if (productItem.ItemId != ITEM_TYPE.NULL)
        {
            productSlotUI.Image.sprite = SpritePool.Sprites[ItemPool.ItemSprites[productItem.ItemId]];
            productSlotUI.Image.enabled = true;
            productSlotUI.AmountText.SetText(productItem.Amount.ToString());
        }
        else
        {
            productSlotUI.Image.sprite = null;
            productSlotUI.Image.enabled = false;
            productSlotUI.AmountText.SetText(string.Empty);
        }
    }

    private void Update()
    {
        if (_fuelEndTime != 0)
        {
            fuelProgressImage.fillAmount = 1 - (Time.time - _fuelStartTime) / (_fuelEndTime - _fuelStartTime);
        }

        if (_smeltEndtime != 0)
        {
            smeltProgressImage.fillAmount = (Time.time - _smeltStartTime) / (_smeltEndtime - _smeltStartTime);

        }
    }
}