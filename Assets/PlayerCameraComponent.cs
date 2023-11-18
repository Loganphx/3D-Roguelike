using ECS.Movement.Services;
using UnityEngine;

internal struct PlayerCameraState : IState
{
    public float mouseSensitivity;
    public float cameraVerticalRotation;
}

internal class PlayerCameraComponent : IComponent<PlayerCameraState>
{
    private readonly Transform         _playerTransform;
    private          Transform         _cameraTransform;
    private          PlayerCameraState _state;
    public           PlayerCameraState State => _state;

    public void OnFixedUpdate()
    {
        throw new System.NotImplementedException();
    }

    public PlayerCameraComponent(Transform playerTransform, Transform cameraTransform)
    {
        _playerTransform = playerTransform;
        _cameraTransform = cameraTransform;
        _state = new PlayerCameraState()
        {
            mouseSensitivity       = 0.5f,
            cameraVerticalRotation = 0,
        };
    }
    public void ProcessInput(ref PlayerInputState playerInputState)
    {
        ref var input = ref playerInputState.Input;
        ref var state = ref _state;
        // Move to cursor visible check
        if (!Cursor.visible)
        {
            var lookDelta = input.lookDelta;
            state.cameraVerticalRotation = State.cameraVerticalRotation - (lookDelta.y * state.mouseSensitivity);
            state.cameraVerticalRotation =
                Mathf.Clamp(State.cameraVerticalRotation, -90, 90);

            var cameraEulerAngles =
                Vector3.right * State.cameraVerticalRotation;

            _cameraTransform.localRotation = Quaternion.Euler(cameraEulerAngles);
            //_cameraTransformComponent.SetLocalRotation(Quaternion.Euler(cameraEulerAngles));
            // var eulerAngles = new Vector3(_playerTransform.Rotation.x,
            //   _playerTransform.Rotation.y + ((lookDelta.x * playerInputStateComponent.mouseSensitivity)),
            //   _playerTransform.Rotation.z);
            //var rotation = Quaternion.Euler(eulerAngles);

            // var transform = new Transform().Rotate(ECSPhysics.UpVector3, )
            var rotation = _playerTransform.rotation * Quaternion.Euler(
                Vector3.up * (lookDelta.x * state.mouseSensitivity
                ));

            input.yEulerAngle = rotation.eulerAngles.y;
            input.xEulerAngle = cameraEulerAngles.x;
            // Debug.Log($"Player Look Delta {lookDelta.x} {lookDelta.y}");
            // Debug.Log($"setting player rotation to {rotation.x}, {rotation.y}, {rotation.z}");
            _playerTransform.rotation = rotation;
        }
    }
}