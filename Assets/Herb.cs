using System;
using System.Collections.Generic;
using UnityEngine;

public enum HERB_TYPE
{
  ZOOMSHROOM,
  MUNCHSHROOM,
  HEALSHROOM,
}
public class Herb : MonoBehaviour, IInteractable, IHoverable
{
  private Outline _outline;
  
  [SerializeField] private HERB_TYPE _herbType;

  private void Awake()
  {
    _outline = transform.GetChild(0).GetComponent<Outline>();
    _outline.enabled = false;
  }

  public void Interact(IPlayer player)
  {
    Debug.Log("Interacting with herb");
    var remainingAmount = player.AddItem(Herbs[_herbType], 1);
    
    if(remainingAmount > 0) return;
    
    // Destroy(gameObject);
    gameObject.SetActive(false);
  }

  public void OnHoverEnter(IPlayer player)
  {
    _outline.enabled = true;
  }

  public void OnHoverExit(IPlayer player)
  {
    _outline.enabled = false;
  }

  private static Dictionary<HERB_TYPE, ITEM_TYPE> Herbs = new Dictionary<HERB_TYPE, ITEM_TYPE>()
  {
    { HERB_TYPE.HEALSHROOM, ITEM_TYPE.MUSHROOM_HEALSHROOM },
    { HERB_TYPE.ZOOMSHROOM, ITEM_TYPE.MUSHROOM_ZOOMSHROOM },
    { HERB_TYPE.MUNCHSHROOM, ITEM_TYPE.MUSHROOM_MUNCHSHROOM },
  };
}
