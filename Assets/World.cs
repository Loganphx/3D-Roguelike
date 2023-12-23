using System;
using System.Collections;
using System.Collections.Generic;
using ECS.Movement.Services;
using UnityEngine;

public class HitboxSystem
{
  public static Dictionary<int, IDamagable> ColliderHashToDamagable = new Dictionary<int, IDamagable>();
}
public struct GameState
{
  internal int Tick;
  
  internal PlayerInputState PlayerInputState;
  internal PlayerCameraState PlayerCameraState;
  internal PlayerMovementState PlayerMovementState;
  internal PlayerAttackState PlayerAttackState;
  internal PlayerHealthState PlayerHealthState;
  internal PlayerStaminaState PlayerStaminaState;
  internal PlayerHungerState PlayerHungerState;
  internal PlayerInventoryState PlayerInventoryState;
  internal PlayerPowerupState PlayerPowerupState;
  internal PlayerWeaponState PlayerWeaponState;

  public bool HasChanged;
}

public class World : MonoBehaviour
{
  public List<IComponent> Components = new List<IComponent>();

  public GameState[] StateBuffer = new GameState[50];
    
  public List<Player> Players = new List<Player>();

  private int Tick;
  private void Awake()
  {
    Tick = 1;
    PrefabPool.LoadPrefabs();
    SpritePool.LoadSprites();
    MaterialPool.LoadMaterials();
  }

  private void Start()
  {
    var player = new Player(new Vector3(3.98f, 0.78f, 1.91f));
    Players.Add(player);
    // Components.Add(player.playerInput);
    // Components.Add(player._playerCameraComponent);
    // Components.Add(player._playerMovementComponent);
    // Components.Add(player._playerInteractComponent);
    // Components.Add(player._playerAttackComponent);
    // Components.Add(player._playerHealthComponent);
    // Components.Add(player._playerStaminaComponent);
    // Components.Add(player._playerHungerComponent);
    // Components.Add(player._playerInventoryComponent);
    // Components.Add(player._playerPowerupComponent);
    // Components.Add(player._playerWeaponComponent);
    
  }

  private void Update()
  {
    foreach (var player in Players)
    {
      player.Update();
    }
  }

  private void FixedUpdate()
  {
    // Rolling Index to cycle through state buffer;
    int previousIndex = (Tick-1) % StateBuffer.Length;
    int currentIndex = Tick % StateBuffer.Length;
    
    // Debug.Log((Tick-1) + " " + previousIndex);
    // Debug.Log(Tick + " " + currentIndex);
    StateBuffer[currentIndex] = StateBuffer[previousIndex];
    ref var state = ref StateBuffer[0];
    state.Tick = Tick;
    foreach (var player in Players)
    {
      player.FixedUpdate();
      bool hasChanged = false;

      if (player.playerInput.State.HasChanged)
      {
        state.PlayerInputState = player.playerInput.State;
        state.HasChanged = true;
      }

      if (player._playerCameraComponent.State.HasChanged)
      {
        state.PlayerCameraState = player._playerCameraComponent.State;
        state.HasChanged = true;
      }

      if (player._playerMovementComponent.State.HasChanged)
      {
        state.PlayerMovementState = player._playerMovementComponent.State;
        state.HasChanged = true;
      }

      if (player._playerAttackComponent.State.HasChanged)
      {
        state.PlayerAttackState = player._playerAttackComponent.State;
        state.HasChanged = true;
      }

      if (player._playerHealthComponent.State.HasChanged)
      {
        state.PlayerHealthState = player._playerHealthComponent.State;
        state.HasChanged = true;
      }

      if (player._playerStaminaComponent.State.HasChanged)
      {
        state.PlayerStaminaState = player._playerStaminaComponent.State;
        state.HasChanged = true;
      }

      if (player._playerHungerComponent.State.HasChanged)
      {
        state.PlayerHungerState = player._playerHungerComponent.State;
        state.HasChanged = true;
      }

      if (player._playerInventoryComponent.State.HasChanged)
      {
        state.PlayerInventoryState = player._playerInventoryComponent.State;
        state.HasChanged = true;
      }

      if (player._playerPowerupComponent.State.HasChanged)
      {
        state.PlayerPowerupState = player._playerPowerupComponent.State;
        state.HasChanged = true;
      }

      if (player._playerWeaponComponent.State.HasChanged)
      {
        state.PlayerWeaponState = player._playerWeaponComponent.State;
        state.HasChanged = true;
      }
      
    }
    
    // if(state.HasChanged) Debug.Log("Tick: " + Tick + " HasChanged: " + state.HasChanged);
    
    Tick++;
  }
}