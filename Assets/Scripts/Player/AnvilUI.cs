using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

internal class AnvilUI : MonoBehaviour
{
    [Header("Config Data")]
    private Color selectedTabColor = "4B2614".HexToColor();
    private Color selectedTextColor = "FFC200".HexToColor();
    private Color unselectedTabColor = "6C402A".HexToColor();
    private Color unselectedTextColor = "FFFFFF".HexToColor();
    
    public static AnvilUI Instance;
    private static readonly (AnvilTabTypes tab, ITEM_TYPE[] items)[] tabs = 
    {
        (AnvilTabTypes.Other, new []
        {
            ITEM_TYPE.COIN,
        }),
        (AnvilTabTypes.Tools, new []
        {
            ITEM_TYPE.TOOL_PICKAXE_IRON,
            ITEM_TYPE.TOOL_AXE_IRON,
            ITEM_TYPE.TOOL_PICKAXE_MYTHRIL,
            ITEM_TYPE.TOOL_AXE_MYTHRIL,       
            ITEM_TYPE.TOOL_PICKAXE_ADAMANTITE,
            ITEM_TYPE.TOOL_AXE_ADAMANTITE,
        }),
        (AnvilTabTypes.Weapons, new []
        {
            ITEM_TYPE.WEAPON_SWORD_IRON,
            ITEM_TYPE.WEAPON_SWORD_MYTHRIL,
            ITEM_TYPE.WEAPON_SWORD_ADAMANTITE,
            ITEM_TYPE.WEAPON_BOW_IRON,
            ITEM_TYPE.WEAPON_BOW_MYTHRIL,
            ITEM_TYPE.WEAPON_BOW_ADAMANTITE,
        }),
        (AnvilTabTypes.Armor, new []
        {
            ITEM_TYPE.HELMET_IRON,
            ITEM_TYPE.CHESTPLATE_IRON,
            ITEM_TYPE.LEGGINGS_IRON,
            ITEM_TYPE.BOOTS_IRON,
            
            ITEM_TYPE.HELMET_MYTHRIL,
            ITEM_TYPE.CHESTPLATE_MYTHRIL,
            ITEM_TYPE.LEGGINGS_MYTHRIL,
            ITEM_TYPE.BOOTS_MYTHRIL,
            
            ITEM_TYPE.HELMET_ADAMANTITE,
            ITEM_TYPE.CHESTPLATE_ADAMANTITE,
            ITEM_TYPE.LEGGINGS_ADAMANTITE,
            ITEM_TYPE.BOOTS_ADAMANTITE,
        }),
        
    };

    private Transform slotHolder;
    private GameObject itemSlotPrefab;
    
    private Image[] tabImages;
    private TMP_Text[] tabTexts;

    private AnvilTabTypes _selectedTab;

    public ItemSlotUI[] cells;

    private Action<ITEM_TYPE> _craftItem;
    
    private int uiLayer;
    private RectTransform _recipePreview;

    private void Awake()
    {
        Instance = this;
        
        slotHolder = transform.Find("Panel").Find("RecipeSlots");
        _recipePreview = transform.Find("RecipePreview").GetComponent<RectTransform>();
        itemSlotPrefab = slotHolder.GetChild(0).gameObject;
        
        // CraftingUI/Panel/Tabs_Header/
        var tabParent = transform.Find("Panel").Find("Tabs_Header");
        var buttons = tabParent.GetComponentsInChildren<Button>();
        tabImages = tabParent.GetComponentsInChildren<Button>().Select(t => t.image).ToArray();
        tabTexts = buttons.Select(t => t.GetComponentInChildren<TMP_Text>()).ToArray();
        
        for (int i = 0; i < buttons.Length; i++)
        {
            var button = buttons[i];
            var i1 = i;
            button.onClick.AddListener(() => OpenTab(i1));
        }

        cells = Array.Empty<ItemSlotUI>();
        uiLayer = LayerMask.NameToLayer("UI");
    }

    private void OnEnable()
    {
        OpenTab((int)AnvilTabTypes.Other);
    }

    public void Init(Action<ITEM_TYPE> craftItem)
    {
        _craftItem = craftItem;
    }

    private void Update()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        var mousePosition = Input.mousePosition; 
        eventData.position = mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);

        foreach (var raycastResult in raysastResults)
        {
            if (raycastResult.gameObject.layer == uiLayer)
            {
                var cell = cells.First(t => t.GameObject == raycastResult.gameObject);
                DisplayRecipe(cell.type, mousePosition);
                return;
            }
        }
        
        HideRecipe();
    }

    private void DisplayRecipe(ITEM_TYPE product, Vector3 mousePosition)
    {
        Debug.Log($"Display Recipe for {product} @ {mousePosition}");
        var recipe = Anvil.Recipes.First(t => t.ProductItemId == product);
        _recipePreview.gameObject.SetActive(true);
        var sizeDelta = _recipePreview.sizeDelta;
        
        _recipePreview.position = new Vector3(mousePosition.x + sizeDelta.x/2, mousePosition.y + sizeDelta.y/2, 0);
        
        var productPanel = _recipePreview.GetChild(0).gameObject;
        var ingredientPanel1 = _recipePreview.GetChild(1).gameObject;
        var ingredientPanel2 = _recipePreview.GetChild(2).gameObject;
        var ingredientPanel3 = _recipePreview.GetChild(3).gameObject;

        productPanel.GetComponent<TextMeshProUGUI>().text = recipe.ProductItemId + " x " + recipe.ProductAmount;
        if (recipe.IngredientItemId1 == ITEM_TYPE.NULL)
        {
            ingredientPanel1.SetActive(false);
        }
        else
        {
            ingredientPanel1.SetActive(true);
            ingredientPanel1.GetComponent<TextMeshProUGUI>().text = recipe.IngredientItemId1 + " - " + recipe.IngredientAmount1;
        }
        
        if (recipe.IngredientItemId2 == ITEM_TYPE.NULL)
        {
            ingredientPanel2.SetActive(false);
        }
        else
        {
            ingredientPanel2.SetActive(true);
            ingredientPanel2.GetComponent<TextMeshProUGUI>().text = recipe.IngredientItemId2 + " - " + recipe.IngredientAmount2;
        }
        
        if (recipe.IngredientItemId3 == ITEM_TYPE.NULL)
        {
            ingredientPanel3.SetActive(false);
        }
        else
        {
            ingredientPanel3.SetActive(true);
            ingredientPanel3.GetComponent<TextMeshProUGUI>().text = recipe.IngredientItemId3 + " - " + recipe.IngredientAmount3;
        }
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(_recipePreview);
        
    }

    private void HideRecipe()
    {
        _recipePreview.gameObject.SetActive(false);
    }
    
    [InvokedByButton("Button_Basic", "Button_Tools", "Button_Stations", "Button_Build")]
    private void OpenTab(int i)
    {
        _selectedTab = (AnvilTabTypes)i;

        UpdateTabs();
        UpdateRecipes();
    }

    private void UpdateTabs()
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            var tabImage = tabImages[i];
            var tabText = tabTexts[i];

            if (i == (int)_selectedTab)
            {
                tabImage.color = this.selectedTabColor;
                tabText.color = this.selectedTextColor;
            }
            else
            {
                tabImage.color = this.unselectedTabColor;
                tabText.color = this.unselectedTextColor;
            }
        }
    }

    private void UpdateRecipes()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            Destroy(cells[i].GameObject);
        }

        cells = new ItemSlotUI[tabs[(int)_selectedTab].items.Length];
        
        for (int i = 0; i < tabs[(int)_selectedTab].items.Length; i++)
        {
            ITEM_TYPE itemType = tabs[(int)_selectedTab].items[i];
            
            GameObject itemSlot = Instantiate(itemSlotPrefab, slotHolder);
            itemSlot.SetActive(true);

            var image = itemSlot.gameObject.transform.GetChild(0).GetComponent<Image>();
            
            var itemSlotUI = new ItemSlotUI()
            {
                GameObject = itemSlot.gameObject,
                Image = image,
                AmountText = image.GetComponentInChildren<TMP_Text>(),
                type = itemType,
            };

            var button = itemSlot.transform.GetComponent<Button>();
            button.onClick.AddListener(() => _craftItem(itemType));
            
            itemSlotUI.Image.enabled = true;
            itemSlotUI.Image.sprite = SpritePool.Sprites[ItemPool.ItemSprites[itemType]];

            cells[i] = itemSlotUI;
            // Spawn Requirements
            
        }
    }

    enum AnvilTabTypes
    {
        Other,
        Tools,
        Weapons,
        Armor,
    }
}