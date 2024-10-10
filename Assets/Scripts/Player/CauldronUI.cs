using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

internal class CauldronUI : MonoBehaviour
{
    internal static CauldronUI Instance { get; private set; }

    private ItemSlotUI[] ingredientSlotUIs;
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

        ingredientSlotUIs = new ItemSlotUI[4];
        GameObject itemSlot;
        Image image;
        for (int i = 0; i < ingredientSlotUIs.Length; i++)
        {
            itemSlot = panel.Find($"ItemSlot_Ingredient_{i}").gameObject;
            image = itemSlot.transform.GetChild(0).GetComponent<Image>();

            ingredientSlotUIs[i] = new ItemSlotUI()
            {
                GameObject = itemSlot,
                Image = image,
                AmountText = itemSlot.transform.GetChild(1).GetComponent<TMP_Text>(),
                type = ITEM_TYPE.NULL,
            };
        }
       
        
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
        BehaviorSubject<InventoryItem> onFuelItemChanged, BehaviorSubject<(byte slotIndex, InventoryItem item)> onIngredientItemChanged, BehaviorSubject<InventoryItem> onProductItemChanged)
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
        fuelSlotUI.AmountText.SetText(fuelItem.Amount.ToString());
        if (fuelItem.ItemId != ITEM_TYPE.NULL)
        {
            fuelSlotUI.Image.sprite = SpritePool.Sprites[ItemPool.ItemSprites[fuelItem.ItemId]];
            fuelSlotUI.Image.enabled = true;
        }
        else
        {
            fuelSlotUI.Image.sprite = null;
            fuelSlotUI.Image.enabled = false;
            fuelSlotUI.AmountText.SetText(string.Empty);
        }    
    }

    private void OnIngredientItemChanged((byte slotIndex, InventoryItem item) ingredient)
    {
        // Debug.Log("OnIngredientItemChanged");
        
        ingredientSlotUIs[ingredient.slotIndex].AmountText.SetText(ingredient.item.Amount.ToString());
        if (ingredient.item.ItemId != ITEM_TYPE.NULL)
        {
            ingredientSlotUIs[ingredient.slotIndex].Image.sprite = SpritePool.Sprites[ItemPool.ItemSprites[ingredient.item.ItemId]];
            ingredientSlotUIs[ingredient.slotIndex].Image.enabled = true;
        }
        else
        {
            ingredientSlotUIs[ingredient.slotIndex].Image.sprite = null;
            ingredientSlotUIs[ingredient.slotIndex].Image.enabled = false;
            ingredientSlotUIs[ingredient.slotIndex].AmountText.SetText(string.Empty);
        }
    }

    private void OnProductItemChanged(InventoryItem productItem)
    {
        // Debug.Log("OnProductItemChanged");
        productSlotUI.AmountText.SetText(productItem.Amount.ToString());
        if (productItem.ItemId != ITEM_TYPE.NULL)
        {
            productSlotUI.Image.sprite = SpritePool.Sprites[ItemPool.ItemSprites[productItem.ItemId]];
            productSlotUI.Image.enabled = true;
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