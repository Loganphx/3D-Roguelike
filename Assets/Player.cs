using ECS.Movement.Services;
using UnityEngine;

// TODO: add a component that can be used to emit changes to the network.

/// <summary>
/// If we use components we can loop them and grab the network interface off of them, then use that to emit changes to clients.
/// </summary>
///

public interface IPlayer
{
    public int Gold { get; set; }
}
public class Player : MonoBehaviour, IPlayer
{
    private Transform    _playerTransform;

    private PlayerInputComponent    playerInput;
    private PlayerCameraComponent   _playerCameraComponent;
    private PlayerMovementComponent _playerMovementComponent;
    private PlayerInteractComponent _playerInteractComponent;
    private PlayerAttackComponent _playerAttackComponent;

    private void Awake()
    {
        _playerTransform            = transform;
        Application.targetFrameRate = 90;
        Gold = 100;
    }

    private void Start()
    {
        playerInput = new PlayerInputComponent();
        var cameraTransform = transform.GetComponentInChildren<Camera>().transform;
        _playerCameraComponent   = new PlayerCameraComponent(_playerTransform, cameraTransform);
        
        var collider     = GetComponentInChildren<Collider>();
        var physicsScene = gameObject.scene.GetPhysicsScene();
        LayerMask collisionLayerMask      = ~(1 << LayerMask.NameToLayer("Player"));
        _playerMovementComponent = new PlayerMovementComponent(_playerTransform, collider, physicsScene, collisionLayerMask);
        
        _playerInteractComponent = new PlayerInteractComponent(this, _playerTransform, cameraTransform, physicsScene);
        _playerAttackComponent = new PlayerAttackComponent(this, cameraTransform, physicsScene);
    }

    public void Update()
    {
        playerInput.OnUpdate();
        playerInput.OnFixedUpdate();

        _playerCameraComponent.ProcessInput(ref playerInput.InputState);
    }

    public void FixedUpdate()
    {
        ref var inputState = ref playerInput.InputState;
        ref var input      = ref inputState.Input;
        _playerMovementComponent.ProcessInput(ref input);
        _playerInteractComponent.ProcessInput(ref input);
        _playerAttackComponent.ProcessInput(ref input);

        input.Interact = false;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        var position = _playerCameraComponent.CameraTransform.position;
        Gizmos.DrawLine(position, position + (_playerCameraComponent.CameraTransform.forward * 5f));
    }

    public int Gold { get; set; }
}