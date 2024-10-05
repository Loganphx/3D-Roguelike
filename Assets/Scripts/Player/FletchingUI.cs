using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FletchingUI : MonoBehaviour
{
     [Header("Config Data")]
    private Color selectedTabColor = "4B2614".HexToColor();
    private Color selectedTextColor = "FFC200".HexToColor();
    private Color unselectedTabColor = "6C402A".HexToColor();
    private Color unselectedTextColor = "FFFFFF".HexToColor();
    
    public static FletchingUI Instance;
    private static readonly (FletchingTabTypes tab, ITEM_TYPE[] items)[] tabs = new (FletchingTabTypes tab, ITEM_TYPE[] items)[]
    { 
        (FletchingTabTypes.Basic, new []
        {
            ITEM_TYPE.ROPE,
        }),
        (FletchingTabTypes.Ammo, new []
        {
            ITEM_TYPE.ARROW_STONE,
            ITEM_TYPE.ARROW_IRON,
            ITEM_TYPE.ARROW_MYTHRIL,
            ITEM_TYPE.ARROW_ADAMANTITE,
        }),
        (FletchingTabTypes.Weapons, new []
        {
            ITEM_TYPE.WEAPON_BOW_WOODEN,
        }),
    };

    private Transform slotHolder;
    private GameObject itemSlotPrefab;
    
    private Image[] tabImages;
    private TMP_Text[] tabTexts;

    private FletchingTabTypes _selectedTab;

    internal ItemSlotUI[] cells;

    private Action<ITEM_TYPE> _craftItem;
    public void Init(Action<ITEM_TYPE> craftItem)
    {
        _craftItem = craftItem;
    }
    
    private void Awake()
    {
        Instance = this;
        
        slotHolder = transform.Find("Panel").Find("RecipeSlots");
        
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
    }

    private void OnEnable()
    {
        OpenTab((int)FletchingTabTypes.Basic);
    }

    [InvokedByButton("Button_Basic", "Button_Tools", "Button_Stations", "Button_Build")]
    private void OpenTab(int i)
    {
        _selectedTab = (FletchingTabTypes)i;

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
                AmountText = image.GetComponentInChildren<TMP_Text>()
            };

            var button = itemSlot.transform.GetComponent<Button>();
            button.onClick.AddListener(() => _craftItem(itemType));
            
            itemSlotUI.Image.enabled = true;
            itemSlotUI.Image.sprite = SpritePool.Sprites[ItemPool.ItemSprites[itemType]];

            cells[i] = itemSlotUI;
            // Spawn Requirements
            
        }
    }

    enum FletchingTabTypes
    {
        Basic,
        Ammo,
        Weapons,
    }
}