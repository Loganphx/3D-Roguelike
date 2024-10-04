using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

internal static class StringExtensions
{
    public static byte[] StringToByteArray(this string hex) {
        return Enumerable.Range(0, hex.Length)
            .Where(x => x % 2 == 0)
            .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
            .ToArray();
    }

    public static Color HexToColor(this string hex)
    {
        var rgb = hex.StringToByteArray();
        return new Color(rgb[0]/255f, rgb[1]/255f, rgb[2]/255f);
    }
}
internal class CraftingUI : MonoBehaviour
{
    [Header("Config Data")]
    private Color selectedTabColor = "4B2614".HexToColor();
    private Color selectedTextColor = "FFC200".HexToColor();
    private Color unselectedTabColor = "6C402A".HexToColor();
    private Color unselectedTextColor = "FFFFFF".HexToColor();
    
    public static CraftingUI Instance;
    private static readonly (TabTypes tab, ITEM_TYPE[] items)[] tabs = new (TabTypes tab, ITEM_TYPE[] items)[]
    { 
        (TabTypes.Basic, new []
        {
            ITEM_TYPE.BARK,
            ITEM_TYPE.BOWL
        }),
        (TabTypes.Tools, new []
        {
            ITEM_TYPE.TOOL_PICKAXE_WOODEN,
            ITEM_TYPE.TOOL_AXE_WOODEN,
            ITEM_TYPE.WEAPON_SWORD_WOODEN,
        }),
        (TabTypes.Stations, new []
        {
            ITEM_TYPE.DEPLOYABLE_FURNACE,
            ITEM_TYPE.DEPLOYABLE_CAULDRON,
            ITEM_TYPE.DEPLOYABLE_CHEST,
            ITEM_TYPE.DEPLOYABLE_FARM_PLANTER,
        }),
        (TabTypes.Build, new []
        {
            ITEM_TYPE.BUILDING_FOUNDATION,
            ITEM_TYPE.BUILDING_FOUNDATION_TRIANGLE,
            ITEM_TYPE.BUILDING_WALL,
        }),
        
    };

    private Transform slotHolder;
    private GameObject itemSlotPrefab;
    
    private Image[] tabImages;
    private TMP_Text[] tabTexts;

    private TabTypes _selectedTab;

    public ItemSlotUI[] cells;

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
        OpenTab((int)TabTypes.Basic);
    }

    [InvokedByButton("Button_Basic", "Button_Tools", "Button_Stations", "Button_Build")]
    private void OpenTab(int i)
    {
        _selectedTab = (TabTypes)i;

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

    enum TabTypes
    {
        Basic,
        Tools,
        Stations,
        Build,
    }
}