using ECS.Movement.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    private PlayerUIHUDComponent       _playerUIHUDComponent;
    private PlayerUIInventoryComponent _playerUIInventoryComponent;

    private void Awake()
    {
        _playerTransform            = transform;
        Application.targetFrameRate = 90;
        Gold                        = 100;
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

        _playerInteractComponent = new PlayerInteractComponent(this, _playerTransform, cameraTransform, physicsScene);
        _playerAttackComponent   = new PlayerAttackComponent(this, cameraTransform, animator, physicsScene);

        _playerHealthComponent    = new PlayerHealthComponent(this, 100);
        _playerStaminaComponent   = new PlayerStaminaComponent(this, 100);
        _playerHungerComponent    = new PlayerHungerComponent(this, 100);
        _playerInventoryComponent = new PlayerInventoryComponent(this);

        var canvas = transform.Find("Canvas");
        var panel  = canvas.GetChild(0).GetChild(0);
        _playerUIHUDComponent = new PlayerUIHUDComponent(
            panel.Find("Health").Find("Text").GetComponent<TMP_Text>(),
            panel.Find("Health").Find("Panel").GetComponent<Image>(),
            panel.Find("Stamina").Find("Panel").GetComponent<Image>(),
            panel.Find("Hunger").Find("Panel").GetComponent<Image>()
        );

        _playerUIInventoryComponent = new PlayerUIInventoryComponent(canvas.Find("Inventory").gameObject,
            canvas.Find("Inventory").Find("Slots"), canvas.Find("Hotbar"));
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
        _playerInteractComponent.ProcessInput(ref input);
        _playerAttackComponent.ProcessInput(ref input);

        _playerHungerComponent.OnFixedUpdate();

        _playerUIInventoryComponent.OnFixedUpdate(ref _playerInventoryComponent._state);
        input.Interact = false;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        var position = _playerCameraComponent.CameraTransform.position;
        Gizmos.DrawLine(position, position + (_playerCameraComponent.CameraTransform.forward * 5f));
    }

    public int       Gold      { get; set; }
    public Transform Transform => _playerTransform;

    public void AddItem(Item item)
    {
        Debug.Log($"Player picked up {item}");
        // Gold += item.GoldValue;
        _playerInventoryComponent.AddItem(item.itemType, ItemPool.ItemDamages[item.itemType], 1);
    }

    public void TakeDamage(IDamager damager, Vector3 hitDirection, int damage)
    {
        Debug.Log($"Player took {damage} damage from {damager}");
        _playerHealthComponent.TakeDamage(damage);
    }
}