using System;
using System.Linq;
using ECS.Movement.Services;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

// ReSharper disable LocalVariableHidesMember

// TODO: add a component that can be used to emit changes to the network.

/// <summary>
/// If we use components we can loop them and grab the network interface off of them, then use that to emit changes to clients.
/// </summary>
///
///
[Serializable]
public class Player : IPlayer, IDamagable
{
  private readonly Transform _playerTransform;

  internal PlayerInputComponent playerInput;
  internal PlayerCameraComponent _playerCameraComponent;
  [SerializeField]
  internal PlayerMovementComponent _playerMovementComponent;
  internal PlayerInteractComponent _playerInteractComponent;
  internal PlayerDropComponent _playerDropComponent;
  internal PlayerAttackComponent _playerAttackComponent;
  internal PlayerHealthComponent _playerHealthComponent;
  internal PlayerStaminaComponent _playerStaminaComponent;
  internal PlayerHungerComponent _playerHungerComponent;
  
  [SerializeField]
  internal PlayerInventoryComponent _playerInventoryComponent;
  internal PlayerEquipmentComponent _playerEquipmentComponent;
  internal PlayerPowerupComponent _playerPowerupComponent;
  internal PlayerWeaponComponent _playerWeaponComponent;

  private PlayerUIHUDComponent _playerUIHUDComponent;
  private PlayerUIInventoryComponent _playerUIInventoryComponent;
  private PlayerUIEquipmentComponent _playerUIEquipmentComponent;
  private PlayerUITrackerComponent _playerUITrackerComponent;
  private PlayerUIChatComponent _playerUIChatComponent;
  private PlayerUIStatsComponent _playerUIStatsComponent;
  private PlayerUIHotbarComponent _playerUIHotbarComponent;
  private PlayerUIPowerupComponent _playerUIPowerupComponent;
  private PlayerUIInteractComponent _playerUIInteractComponent;

  public bool HasChanged;

  public Player(Vector3 position)
  {
    _playerTransform = Object.Instantiate(PrefabPool.Prefabs["Prefabs/Player"], position, Quaternion.identity)
      .transform;
      
    Init();

    var colliders = _playerTransform.GetComponentsInChildren<Collider>();
    foreach (var collider in colliders)
    {
      HitboxSystem.ColliderHashToDamagable.Add(collider.GetInstanceID(), this);
    }
  }

  private void Init()
  {
    var eventSystem = Object.Instantiate(PrefabPool.Prefabs["Prefabs/EventSystem"], Vector3.zero,  Quaternion.identity).GetComponent<EventSystem>();
    
    playerInput = new PlayerInputComponent();
    var cameraTransform = _playerTransform.GetComponentInChildren<Camera>().transform;
    _playerCameraComponent = new PlayerCameraComponent(_playerTransform, cameraTransform);

    var collider = _playerTransform.GetComponentInChildren<Collider>();
    var animator = _playerTransform.GetComponent<Animator>();
    var physicsScene = _playerTransform.gameObject.scene.GetPhysicsScene();
    LayerMask collisionLayerMask = ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Damagable"));
    _playerMovementComponent =
      new PlayerMovementComponent(_playerTransform, collider, physicsScene, collisionLayerMask);

    _playerInteractComponent = new PlayerInteractComponent(this, cameraTransform, physicsScene);
    _playerDropComponent = new PlayerDropComponent(this, cameraTransform);
    _playerAttackComponent = new PlayerAttackComponent(this, cameraTransform, animator, physicsScene);

    _playerHealthComponent = new PlayerHealthComponent(this, 100);
    _playerStaminaComponent = new PlayerStaminaComponent(this, 100);
    _playerHungerComponent = new PlayerHungerComponent(this, 100);
    _playerInventoryComponent = new PlayerInventoryComponent(this);
    _playerEquipmentComponent = new PlayerEquipmentComponent(this);
    _playerPowerupComponent = new PlayerPowerupComponent(this);
    _playerWeaponComponent =
      new PlayerWeaponComponent(this, _playerTransform.Find("Eyes").Find("Arm").Find("Arm").Find("Anchor"));
      
    var canvas = _playerTransform.Find("Canvas");
    var panel = canvas.GetChild(0).GetChild(0);
    _playerUIHUDComponent = new PlayerUIHUDComponent(
      panel.Find("Health").Find("Text").GetComponent<TMP_Text>(),
      panel.Find("Health").Find("Panel_Health").GetComponent<Image>(),
      panel.Find("Health").Find("Panel_Shield").GetComponent<Image>(),
      panel.Find("Stamina").Find("Panel").GetComponent<Image>(),
      panel.Find("Hunger").Find("Panel").GetComponent<Image>()
    );

    _playerUIInventoryComponent = new PlayerUIInventoryComponent(canvas.Find("Inventory").gameObject,
      canvas.Find("Inventory").Find("Panel").Find("Slots"));
    _playerUIEquipmentComponent =
      new PlayerUIEquipmentComponent(canvas.Find("Inventory").Find("Panel").Find("Character").gameObject);
    
    _playerUITrackerComponent = new PlayerUITrackerComponent(
      canvas.Find("Gold").Find("Panel_Gold").Find("Text").GetComponent<TMP_Text>(),
      canvas.Find("Gold").Find("Panel_Time").Find("Text").GetComponent<TMP_Text>(),
      canvas.Find("Gold").Find("Panel_Day").Find("Text").GetComponent<TMP_Text>());
    _playerUIChatComponent =
      new PlayerUIChatComponent(canvas.Find("Chat").Find("Scroll View").Find("Viewport").Find("Content").gameObject,
        canvas.Find("Chat").Find("Scroll View").Find("Scrollbar Vertical"));
    
    _playerUIStatsComponent = new PlayerUIStatsComponent(canvas.Find("Inventory").Find("Panel").Find("Stats").Find("Panel").gameObject);
    
    _playerUIHotbarComponent = new PlayerUIHotbarComponent(canvas.Find("Hotbar").gameObject, canvas.Find("Hotbar"));

    _playerUIPowerupComponent = new PlayerUIPowerupComponent(canvas.Find("Powerups"));
    _playerUIInteractComponent = new PlayerUIInteractComponent(this, eventSystem, canvas.Find("CraftingStation").gameObject,
      canvas.Find("Furnace").gameObject, canvas.Find("Cauldron").gameObject, canvas.Find("FletchingTable").gameObject);

    AddItem(ITEM_TYPE.COIN, 1000);
    AddItem(ITEM_TYPE.SEED_WHEAT, 5);
    AddItem(ITEM_TYPE.BUILDING_FOUNDATION, 5);
    AddItem(ITEM_TYPE.SEED_FLAX, 1);
    // AddItem(ITEM_TYPE.BUILDING_FOUNDATION, 2);
    AddItem(ITEM_TYPE.TOOL_AXE_WOODEN, 1);
    AddItem(ITEM_TYPE.TOOL_PICKAXE_WOODEN, 1);
    
    AddItem(ITEM_TYPE.WOOD, 25);
    AddItem(ITEM_TYPE.ROCK, 10);
    
    EquipItem(0, ITEM_TYPE.HELMET_IRON);
    EquipItem(1, ITEM_TYPE.CHESTPLATE_IRON);
    EquipItem(2, ITEM_TYPE.LEGGINGS_IRON);
    EquipItem(3, ITEM_TYPE.BOOTS_IRON);

    AddPowerup(POWERUP_TYPE.INCREASE_HEALTH);
    AddPowerup(POWERUP_TYPE.INCREASE_STAMINA);
    AddPowerup(POWERUP_TYPE.SLOW_HUNGER);
    AddPowerup(POWERUP_TYPE.SHIELD);
    AddPowerup(POWERUP_TYPE.EXTRA_JUMP);
    AddPowerup(POWERUP_TYPE.EXTRA_MELEE_DAMAGE);
    // _playerUIChatComponent.HandleMessage("Loganphx has joined the game.");
    // AddItem(ITEM_TYPE.DEPLOYABLE_FARM_PLANTER, 1);
    // AddItem(ITEM_TYPE.DEPLOYABLE_CRAFTING_STATION, 1);
  }

  public void Update()
  {
    playerInput.OnUpdate();
    playerInput.OnFixedUpdate();

    _playerCameraComponent.ProcessInput(ref playerInput.InputState);

    _playerUIHUDComponent.OnUpdate(ref _playerHealthComponent._healthState,
      ref _playerStaminaComponent._staminaState,
      ref _playerHungerComponent._hungerState);

    _playerUIInventoryComponent.OnUpdate(ref playerInput.InputState.Input);
    _playerUIInteractComponent.OnUpdate();
  }

  public void FixedUpdate()
  {
    ref var inputState = ref playerInput.InputState;
    ref var input = ref inputState.Input;
    ref var inventoryState = ref _playerInventoryComponent._state;
    ref var equipmentState = ref _playerEquipmentComponent._state;
    ref var healthState = ref _playerHealthComponent._healthState;
    ref var staminaState = ref _playerStaminaComponent._staminaState;
    ref var hungerState = ref _playerHungerComponent._hungerState;
    ref var weaponState = ref _playerWeaponComponent._state;
    ref var attackState = ref _playerAttackComponent._state;
    ref var movementState = ref _playerMovementComponent.MovementState;


    var hasChanged = _playerMovementComponent.ProcessInput(ref input, ref staminaState);
    if (hasChanged) HasChanged = true;

    hasChanged = _playerStaminaComponent.ProcessInput(ref input, ref hungerState);
    if (hasChanged) HasChanged = true;

    hasChanged = _playerInteractComponent.ProcessInput(ref input, ref weaponState);
    if (hasChanged) HasChanged = true;

    hasChanged = _playerDropComponent.ProcessInput(ref input, ref weaponState, ref inventoryState);
    if (hasChanged) HasChanged = true;

    hasChanged = _playerAttackComponent.ProcessInput(ref input, ref weaponState);
    if (hasChanged) HasChanged = true;

    hasChanged = _playerWeaponComponent.OnFixedUpdate(ref input, ref inventoryState);
    if (hasChanged) HasChanged = true;

    hasChanged = _playerHungerComponent.OnFixedUpdate();
    if (hasChanged) HasChanged = true;
    
    hasChanged = _playerHealthComponent.OnFixedUpdate(ref hungerState);
    if (hasChanged) HasChanged = true;
    
    _playerUIInventoryComponent.OnFixedUpdate(ref inventoryState);
    _playerUIEquipmentComponent.OnFixedUpdate(ref equipmentState);

    _playerUITrackerComponent.OnFixedUpdate(ref inventoryState, ref DayManager.Instance.dayState);
    _playerUIStatsComponent.OnFixedUpdate(ref healthState, ref attackState, ref movementState);
    _playerUIHotbarComponent.OnFixedUpdate(ref inventoryState);
    _playerUIPowerupComponent.OnFixedUpdate(ref _playerPowerupComponent._state);
    _playerUIInteractComponent.OnFixedUpdate(ref _playerInteractComponent._state, ref inventoryState);

    input.Interact = false;
    input.Drop = false;
    input.JumpPressed = false;
    input.Action1 = false;
    input.Action2 = false;
    input.Action3 = false;
    input.Action4 = false;
    input.Action5 = false;
    input.Action6 = false;
    input.Action7 = false;
    input.PrimaryAttackClicked = false;
    input.SecondaryAttackClicked = false;

    input.ToggleInventory = false;

    // RESETTING HAS CHANGED FLAGS
    _playerInventoryComponent.OnFixedUpdate();

    hasChanged = _playerEquipmentComponent.OnFixedUpdate(ref healthState);
    if (hasChanged) HasChanged = true;
    
    _playerUIInteractComponent.OnLateFixedUpdate(ref _playerInteractComponent._state);
  }

  public void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    var position = _playerCameraComponent.CameraTransform.position;
    Gizmos.DrawLine(position, position + (_playerCameraComponent.CameraTransform.forward * 5f));
  }

  public Transform Transform => _playerTransform;
  public bool IsDead
  {
    get {     
      ref var state = ref _playerHealthComponent._healthState;
      return state.IsDead; 
    }
  }

  public int GetGoldInInventory()
  {
    ref var state = ref _playerInventoryComponent._state;
    return state.GoldCount;
  }

  public ITEM_TYPE GetCurrentWeapon()
  {
    ref var state = ref _playerWeaponComponent._state;

    return state.EquippedWeapon;
  }

  public int AddItem(ITEM_TYPE itemId, int amount)
  {
    Debug.Log($"Player picked up {itemId}");
    // Gold += item.GoldValue;
    return _playerInventoryComponent.AddItem(itemId,
      ItemPool.ItemDamages[itemId],
      ItemPool.ItemDeployables.Contains(itemId),
      ItemPool.ItemConsumables.Contains(itemId),
      amount);
  }

  public void EquipItem(int slotIndex, ITEM_TYPE itemType)
  {
    _playerEquipmentComponent.EquipItem(slotIndex, itemType);
  }

  public void RemoveItem(ITEM_TYPE itemId, int amount)
  {
    Debug.Log($"Removing {itemId} x {amount} from Player.");
    _playerInventoryComponent.RemoveItem(itemId, amount);
  }

  public bool ContainsItem(ITEM_TYPE itemId, int amount)
  {
    if(itemId == ITEM_TYPE.NULL) return true;
    if(amount == 0) return true;

    return _playerInventoryComponent.ContainsItem(itemId, amount);
  }

  public void RemoveItem(int slotIndex, int amount)
  {
    _playerInventoryComponent.RemoveItem(slotIndex, amount);
  }

  public void AddPowerup(POWERUP_TYPE powerupId, int amount = 1)
  {
    _playerUIChatComponent.HandleMessage(amount > 1
      ? $"Player picked up {powerupId} x {amount}"
      : $"Player picked up {powerupId}");

    _playerPowerupComponent.AddPowerup(powerupId, amount);

    switch (powerupId)
    {
      case POWERUP_TYPE.EXTRA_JUMP:
      {
        ref var state = ref _playerMovementComponent.MovementState;
        state.maxJumps++;
        break;
      }
      case POWERUP_TYPE.INCREASE_HEALTH:
      {
        ref var state = ref _playerHealthComponent._healthState;
        state.Health += 10;
        state.MaxHealth += 10;
        break;
      }
      case POWERUP_TYPE.INCREASE_STAMINA:
      {
        ref var state = ref _playerStaminaComponent._staminaState;
        state.Stamina += 10;
        state.MaxStamina += 10;
        break;
      }
      case POWERUP_TYPE.SLOW_HUNGER:
      {
        ref var state = ref _playerHungerComponent._hungerState;
        state.HungerDrainRate *= .9f;
        break;
      }
      case POWERUP_TYPE.SHIELD:
      {
        ref var state = ref _playerHealthComponent._healthState;
        state.Shield += 10;
        state.MaxShield += 10;
        break;
      }
      case POWERUP_TYPE.EXTRA_MELEE_DAMAGE:
      {
        ref var state = ref _playerAttackComponent._state;
        state.Strength += 10;
        break;
      }
      default:
        Debug.LogException(new NotSupportedException($"{powerupId} is not yet implemented"));
        break;
    }
  }

  public void OnHit(IDamager damager, Vector3 hitDirection, Vector3 hitPosition, TOOL_TYPE toolType, int damage)
  {
    Debug.Log($"Player took {damage} damage from {damager}");
    _playerHealthComponent.TakeDamage(damage);
  }

  public void AddHealth(int amount)
  {
    _playerHealthComponent.AddHealth(amount);
  }

  public void AddShield(int amount)
  {
    _playerHealthComponent.AddShield(amount);
  }

  public void AddStamina(int amount)
  {
    _playerStaminaComponent.AddStamina(amount);
  }

  public void AddHunger(int amount)
  {
    _playerHungerComponent.AddHunger(amount);
  }
}