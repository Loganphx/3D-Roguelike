using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ITEM_TYPE
{
    NULL = 0,
    WOOD,
    WOOD_BIRCH,
    WOOD_FIR,
    WOOD_OAK,
    WOOD_DARKOAK,
    ORE_STONE,
    ORE_COAL,
    ORE_IRON,
    ORE_GOLD,
    ORE_ADAMANTITE,
    ORE_MYTHRIL,
    COIN
}
public class Item : MonoBehaviour, IInteractable, IHoverable
{
    // Start is called before the first frame update
    private Outline _outline;

    [SerializeField] public ITEM_TYPE itemType;
    void Awake()
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
    }

    public void Interact(IPlayer player)
    {
        Debug.Log("Interacting with item");
        player.AddItem(this);
        gameObject.SetActive(false);
    }
    
    public void OnHoverEnter()
    {
        _outline.enabled = true;
    }

    public void OnHoverExit()
    {
        _outline.enabled = false;
    }
}
