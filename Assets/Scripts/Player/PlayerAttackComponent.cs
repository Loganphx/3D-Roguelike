using ECS.Movement.Services;
using UnityEngine;

internal struct PlayerAttackState : IState
{
  public float RemainingCooldown;
  public float AttackCooldown;
  public float ConsumeCooldown;

  public bool IsConsuming;

  public bool HasChanged;
}

internal class PlayerAttackComponent : IComponent<PlayerAttackState>
{
  private readonly IPlayer _player;
  private readonly Transform _cameraTransform;
  private PhysicsScene _physicsScene;
  private readonly RaycastHit[] _hits = new RaycastHit[5];
  private readonly Animator _animator;

  private PlayerAttackState _state = new PlayerAttackState()
  {
    AttackCooldown = 0.5f,
    ConsumeCooldown = 1.6f,
    RemainingCooldown = 0,
    HasChanged = true
  };

  private static readonly int AttackHash = Animator.StringToHash("Attack");
  private static readonly int ConsumeHash = Animator.StringToHash("Consuming");

  public PlayerAttackComponent(IPlayer player, Transform cameraTransform,
    Animator animator,
    PhysicsScene physicsScene)
  {
    _player = player;
    _cameraTransform = cameraTransform;
    _animator = animator;
    _physicsScene = physicsScene;
  }

  public bool ProcessInput(ref GameplayInput input, ref PlayerWeaponState weaponState)
  {
    ref var state = ref _state;
    state.HasChanged = false;

    if (state.IsConsuming && !input.SecondaryAttackHeld)
    {
      Debug.Log("Setting consume animation to false");
      _animator.SetBool(ConsumeHash, false);
      state.IsConsuming = false;
    }
    
    if (weaponState.EquippedWeapon != ITEM_TYPE.NULL && ItemPool.ItemDamages[weaponState.EquippedWeapon] > 0)
    {
      if (input.PrimaryAttackClicked && state.RemainingCooldown < 0)
      {
        _animator.SetTrigger(AttackHash);
        
        // Detect Damages
        var hits = _physicsScene.Raycast(_cameraTransform.position, _cameraTransform.forward, _hits, 5f,
          1 << LayerMask.NameToLayer("Damagable") | 1 << LayerMask.NameToLayer("Interactable"), QueryTriggerInteraction.Collide);

        if (hits != 0)
        {
          var hit = _hits[0];

          if (HitboxSystem.ColliderHashToDamagable.ContainsKey(hit.colliderInstanceID))
          {
            var damagable = HitboxSystem.ColliderHashToDamagable[hit.colliderInstanceID];
            damagable?.OnHit(_player, _cameraTransform.forward, hit.point, ItemPool.ItemTools[weaponState.EquippedWeapon],
              ItemPool.ItemDamages[weaponState.EquippedWeapon]);
          }
          else Debug.LogError($"Failed to find damagable for {_hits[0].transform.root}({hits})");
          
        }
        

        state.RemainingCooldown = state.AttackCooldown;
        state.HasChanged = true;
      }
    }

    // Debug.Log(input.SecondaryAttackHeld);
    if (input.SecondaryAttackHeld && ItemPool.ItemConsumables.Contains(weaponState.EquippedWeapon))
    {
      // Debug.Log("Attempting to consume");
      if (!state.IsConsuming && state.RemainingCooldown < 0)
      {
        // Debug.Log("Setting consume animation to true");
        // Start Consuming Animation and Countdown
        _animator.SetBool(ConsumeHash, true);
        state.RemainingCooldown = state.ConsumeCooldown;
        state.IsConsuming = true;
        state.HasChanged = true;
      }
      else if (state.IsConsuming && state.RemainingCooldown < 0)
      {
        Debug.Log($"Consumable: {input.SecondaryAttackHeld}");

        if (weaponState.EquippedWeapon == ITEM_TYPE.MUSHROOM_HEALSHROOM)
        {
          Debug.Log("Healing player");
          _player.AddHealth(10);
          _player.RemoveItem(weaponState.EquippedSlot, 1);
        }
        else if (weaponState.EquippedWeapon == ITEM_TYPE.MUSHROOM_MUNCHSHROOM)
        {
          Debug.Log("Hungering player");
          _player.AddHunger(10);
          _player.RemoveItem(weaponState.EquippedSlot, 1);
        }
        else if (weaponState.EquippedWeapon == ITEM_TYPE.MUSHROOM_ZOOMSHROOM)
        {
          Debug.Log("Zooming player");
          _player.AddStamina(10);
          _player.RemoveItem(weaponState.EquippedSlot, 1);
        }

        _animator.SetBool(ConsumeHash, false);

        state.IsConsuming = false;
        state.HasChanged = true;
      }
    }
    
    if (state.RemainingCooldown >= 0)
    {
      state.RemainingCooldown -= Time.fixedDeltaTime;
      state.HasChanged = true;
      // Debug.Log($"Cooldown: {state.RemainingCooldown}");
      return state.HasChanged;
    }

// Debug.Log($"Cooldown: {state.RemainingCooldown}");
// ReSharper disable once RedundantJumpStatement
    return state.HasChanged;
  }

  public PlayerAttackState State => _state;
}