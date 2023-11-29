using ECS.Movement.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// ReSharper disable LocalVariableHidesMember

// TODO: add a component that can be used to emit changes to the network.

/// <summary>
/// If we use components we can loop them and grab the network interface off of them, then use that to emit changes to clients.
/// </summary>
///
///
public class Player : MonoBehaviour, IPlayer, IDamagable
{
    private Transform _playerTransform;

    private PlayerInputComponent     playerInput;
    private PlayerCameraComponent    _playerCameraComponent;
    private PlayerMovementComponent  _playerMovementComponent;
    private PlayerInteractComponent  _playerInteractComponent;
    private PlayerAttackComponent    _playerAttackComponent;
    private PlayerHealthComponent    _playerHealthComponent;
    private PlayerStaminaComponent   _playerStaminaComponent;
    private PlayerHungerComponent    _playerHungerComponent;
    private PlayerInventoryComponent _playerInventoryComponent;
    private PlayerPowerupComponent _playerPowerupComponent;
    private PlayerWeaponComponent _playerWeaponComponent;

    private PlayerUIHUDComponent       _playerUIHUDComponent;
    private PlayerUIInventoryComponent _playerUIInventoryComponent;
    private PlayerUIPowerupComponent _playerUIPowerupComponent;

    private void Awake()
    {
        _playerTransform            = transform;
        Application.targetFrameRate = 90;
    }

    private void Start()
    {
        playerInput = new PlayerInputComponent();
        var cameraTransform = transform.GetComponentInChildren<Camera>().transform;
        _playerCameraComponent = new PlayerCameraComponent(_playerTransform, cameraTransform);

        var       collider           = GetComponentInChildren<Collider>();
        var       animator           = GetComponent<Animator>();
        var       physicsScene       = gameObject.scene.GetPhysicsScene();
        LayerMask collisionLayerMask = ~(1 << LayerMask.NameToLayer("Player"));
        _playerMovementComponent =
            new PlayerMovementComponent(_playerTransform, collider, physicsScene, collisionLayerMask);

        _playerInteractComponent = new PlayerInteractComponent(this, cameraTransform, physicsScene);
        _playerAttackComponent   = new PlayerAttackComponent(this, cameraTransform, animator, physicsScene);

        _playerHealthComponent    = new PlayerHealthComponent(this, 100);
        _playerStaminaComponent   = new PlayerStaminaComponent(this, 100);
        _playerHungerComponent    = new PlayerHungerComponent(this, 100);
        _playerInventoryComponent = new PlayerInventoryComponent(this);
        _playerPowerupComponent = new PlayerPowerupComponent(this);
        _playerWeaponComponent = new PlayerWeaponComponent(this, transform.Find("Eyes").Find("Arm").Find("Anchor"));

        var canvas = transform.Find("Canvas");
        var panel  = canvas.GetChild(0).GetChild(0);
        _playerUIHUDComponent = new PlayerUIHUDComponent(
            panel.Find("Health").Find("Text").GetComponent<TMP_Text>(),
            panel.Find("Health").Find("Panel").GetComponent<Image>(),
            panel.Find("Stamina").Find("Panel").GetComponent<Image>(),
            panel.Find("Hunger").Find("Panel").GetComponent<Image>()
        );

        _playerUIInventoryComponent = new PlayerUIInventoryComponent(canvas.Find("Inventory").gameObject,
            canvas.Find("Inventory").Find("Slots"), canvas.Find("Hotbar"), canvas.Find("Gold").GetChild(0).Find("Text").GetComponent<TMP_Text>());
        
        _playerUIPowerupComponent = new PlayerUIPowerupComponent(canvas.Find("Powerups"));
        
        AddItem(ITEM_TYPE.COIN, 1000);
        AddItem(ITEM_TYPE.SEED_WHEAT, 3);
        AddItem(ITEM_TYPE.SEED_FLAX, 1);
        AddItem(ITEM_TYPE.BUILDING_FOUNDATION, 10);
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
    }

    public void FixedUpdate()
    {
        ref var inputState   = ref playerInput.InputState;
        ref var input        = ref inputState.Input;
        ref var staminaState = ref _playerStaminaComponent._staminaState;
        ref var hungerState  = ref _playerHungerComponent._hungerState;
        _playerMovementComponent.ProcessInput(ref input, ref staminaState);
        _playerStaminaComponent.ProcessInput(ref input, ref hungerState);
        _playerInteractComponent.ProcessInput(ref input, ref _playerWeaponComponent._state);
        _playerAttackComponent.ProcessInput(ref input);
        _playerWeaponComponent.OnFixedUpdate(ref input, ref _playerInventoryComponent._state);

        _playerHungerComponent.OnFixedUpdate();

        _playerUIInventoryComponent.OnFixedUpdate(ref _playerInventoryComponent._state);
        _playerUIPowerupComponent.OnFixedUpdate(ref _playerPowerupComponent._state);
        
        input.Interact = false;
        input.Action1 = false;
        input.Action2 = false;
        input.Action3 = false;
        input.Action4 = false;
        input.Action5 = false;
        input.Action6 = false;
        input.PrimaryAttackClicked = false;
        
        input.ToggleInventory = false;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        var position = _playerCameraComponent.CameraTransform.position;
        Gizmos.DrawLine(position, position + (_playerCameraComponent.CameraTransform.forward * 5f));
    }

    public Transform Transform => _playerTransform;

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

    public void AddItem(ITEM_TYPE itemId, int amount)
    {
        Debug.Log($"Player picked up {itemId}");
        // Gold += item.GoldValue;
        _playerInventoryComponent.AddItem(itemId, ItemPool.ItemDamages[itemId], ItemPool.ItemDeployables.Contains(itemId), amount);
    }

    public void RemoveItem(ITEM_TYPE itemId, int amount)
    {
        Debug.Log($"Removing {itemId} x {amount} from Player.");
        _playerInventoryComponent.RemoveItem(itemId, amount);
    }

    public void AddPowerup(POWERUP_TYPE powerupId, int amount)
    {
        Debug.Log($"Player picked up {powerupId}");
        _playerPowerupComponent.AddPowerup(powerupId, amount);
    }

    public void TakeDamage(IDamager damager, Vector3 hitDirection, int damage)
    {
        Debug.Log($"Player took {damage} damage from {damager}");
        _playerHealthComponent.TakeDamage(damage);
    }
}