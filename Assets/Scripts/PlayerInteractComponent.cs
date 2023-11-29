using ECS.Movement.Services;
using Unity.VisualScripting;
using UnityEngine;

internal class PlayerInteractComponent
{
  private readonly IPlayer _player;
  private readonly Transform _cameraTransform;
  private PhysicsScene _physicsScene;

  private IHoverable lastHover;

  public PlayerInteractComponent(IPlayer player, Transform cameraTransform,
    PhysicsScene physicsScene)
  {
    _player = player;
    _cameraTransform = cameraTransform;
    _physicsScene = physicsScene;
  }

  public void ProcessInput(ref GameplayInput input, ref PlayerWeaponState weaponState)
  {
    var hitSomething = _physicsScene.Raycast(_cameraTransform.position, _cameraTransform.forward, out var hit, 5f,
      1 << LayerMask.NameToLayer("Interactable"), QueryTriggerInteraction.Collide);
      
    if (hitSomething)
    {
      var hoverable = hit.transform.GetComponentInParent<IHoverable>();
      if (hoverable != null)
      {
        if (lastHover != hoverable)
        {
          if(lastHover != null) lastHover.OnHoverExit(_player);
          hoverable.OnHoverEnter(_player);
        }
        lastHover = hoverable;
      }

      
      if (ItemPool.ItemDeployables.Contains(weaponState.EquippedWeapon))
      {
        if (input.PrimaryAttackClicked)
        {
          var farmPlot = hit.transform.GetComponentInParent<IDeployable>();
          farmPlot?.Deploy(_player, weaponState.EquippedWeapon);
          return;
        }
      }
      
      if (!input.Interact) return;
      // Debug.Log($"Hit {hit.collider}");
      var interactable = hit.transform.GetComponentInParent<IInteractable>();
      interactable?.Interact(_player);
    }
    else
    {
      if (lastHover != null) lastHover.OnHoverExit(_player);
    }
  }
}