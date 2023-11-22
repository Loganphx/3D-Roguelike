using ECS.Movement.Services;
using UnityEngine;

internal class PlayerInteractComponent
{
  private readonly IPlayer _player;
  private Transform _playerTransform;
  private Transform _cameraTransform;
  private PhysicsScene _physicsScene;

  private IHoverable lastHover;
  public PlayerInteractComponent(IPlayer player, Transform playerTransform, Transform cameraTransform,
    PhysicsScene physicsScene)
  {
    _player = player;
    _playerTransform = playerTransform;
    _cameraTransform = cameraTransform;
    _physicsScene = physicsScene;
  }

  public void ProcessInput(ref GameplayInput input)
  {
    var hitSomething = _physicsScene.Raycast(_cameraTransform.position, _cameraTransform.forward, out var hit, 5f,
      1 << LayerMask.NameToLayer("Interactable"), QueryTriggerInteraction.Collide);

    if(input.Interact) Debug.Log($"Interact: {hitSomething}");

    if (hitSomething)
    {
      var hoverable = hit.collider.transform.root.GetComponent<IHoverable>();
      if (hoverable != null)
      {
        hoverable.OnHoverEnter();
        if (lastHover != null && lastHover != hoverable) lastHover.OnHoverExit();
        lastHover = hoverable;
                
      }

      if (!input.Interact) return;
      // Debug.Log($"Hit {hit.collider}");
      var interactable = hit.collider.transform.root.GetComponent<IInteractable>();
      interactable.Interact(_player);
    }
    else
    {
      if(lastHover != null) lastHover.OnHoverExit();
    }
  }
}