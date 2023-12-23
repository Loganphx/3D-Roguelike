using ECS.Movement.Services;
using Unity.VisualScripting;
using UnityEngine;

internal class PlayerInteractComponent
{
  private readonly IPlayer _player;
  private readonly Transform _cameraTransform;
  private PhysicsScene _physicsScene;

  private IHoverable lastHover;

  private RaycastHit[] _hits = new RaycastHit[5];

  public PlayerInteractComponent(IPlayer player, Transform cameraTransform,
    PhysicsScene physicsScene)
  {
    _player = player;
    _cameraTransform = cameraTransform;
    _physicsScene = physicsScene;
  }

  public bool ProcessInput(ref GameplayInput input, ref PlayerWeaponState weaponState)
  {
    var hitCount = _physicsScene.Raycast(_cameraTransform.position, _cameraTransform.forward, _hits, 5f,
      1 << LayerMask.NameToLayer("Interactable"), QueryTriggerInteraction.Collide);

    if (hitCount > 0)
    {
      for (int i = 0; i < hitCount; i++)
      {
        var hit = _hits[i];
        var hoverable = hit.transform.GetComponentInParent<IHoverable>();
        var isHoverable = hoverable != null;
        
        if (isHoverable)
        {
          if (lastHover != hoverable)
          {
            if (lastHover != null) lastHover.OnHoverExit(_player);
            hoverable.OnHoverEnter(_player);
          }

          lastHover = hoverable;
        }

        if (input.PrimaryAttackClicked)
        {
          if (ItemPool.ItemDeployables.Contains(weaponState.EquippedWeapon))
          {
            var farmPlot = hit.transform.GetComponentInParent<IDeployable>();
            farmPlot?.Deploy(_player, weaponState.EquippedWeapon);
            return false;
          }
        }

        if (!input.Interact) return false;
        // Debug.Log($"Hit {hit.collider}");
        var interactable = hit.transform.GetComponentInParent<IInteractable>();
        if (interactable != null)
        {
          interactable.Interact(_player);
          return false;
        }
      }
    }
    else
    {
      if (lastHover != null) lastHover.OnHoverExit(_player);
    }

    return false;
  }
}

internal class PlayerDropComponent
{
  private IPlayer _player;
  private Transform _cameraTransform;
  
  internal PlayerDropComponent(IPlayer player, Transform cameraTransform)
  {
    _player = player;
    _cameraTransform = cameraTransform;
  }
  
  public bool ProcessInput(ref GameplayInput input, ref PlayerWeaponState weaponState, ref PlayerInventoryState inventoryState)
  {
    if (input.Drop && weaponState.EquippedWeapon != ITEM_TYPE.NULL)
    {
      var quantity = inventoryState.Items[weaponState.EquippedSlot].Amount;
      DropItem(weaponState.EquippedSlot, weaponState.EquippedWeapon, quantity);
      return true;
    }

    return false;
  }

  private void DropItem(int itemSlot, ITEM_TYPE itemType, int quantity)
  {

    var position = _cameraTransform.transform.position;
    var dropPosition = new Vector3(position.x, position.y + 0.15f, position.z);
    var itemTemplatePrefab = PrefabPool.Prefabs["Prefabs/Items/item_template"];
    var itemTemplate = GameObject.Instantiate(itemTemplatePrefab, dropPosition, Quaternion.identity);
    var itemPrefab = PrefabPool.Prefabs[ItemPool.ItemPrefabs[itemType]];

    var item = GameObject.Instantiate(itemPrefab, Vector3.zero,
      Quaternion.identity, itemTemplate.transform);

    item.transform.localPosition = Vector3.zero;
    
    itemTemplate.GetComponent<Item>().SetItemType(itemType);
      
    // var drop = GameObject.Instantiate(dropPrefab, dropPosition, Quaternion.identity);
    // hitDirection.y = 1;
    itemTemplate.GetComponent<Rigidbody>().AddForce(_cameraTransform.transform.forward * 3f, ForceMode.Impulse);
    
    _player.RemoveItem(itemSlot, quantity);
  }
}