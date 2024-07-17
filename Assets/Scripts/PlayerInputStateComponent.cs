using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ECS.Movement.Services
{
    [Serializable]
    public class PlayerInputState : IState
    {
        public static readonly int HashId = typeof(PlayerInputState).GetHashCode();
        
        public float         mouseSensitivity       = 0.5f;
        public float         cameraVerticalRotation = 1;
        public GameplayInput Input;

        public bool HasChanged;
    }

    public class PlayerInputComponent : IComponent<PlayerInputState>
    {
      private readonly Keyboard _keyboard;
      private readonly Mouse _mouse;

      public PlayerInputState InputState;
      public PlayerInputState State => InputState;

      public PlayerInputComponent()
      {
        InputState = new PlayerInputState();
        _keyboard = Keyboard.current;
        _mouse = Mouse.current;
      }

      public void OnUpdate()
      {
        ref var inputState = ref InputState;
        ref var input      = ref inputState.Input;
        input.lookDelta = _mouse.delta.ReadValue();
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
          if (Cursor.visible)
          {
            Cursor.visible   = false;
            Cursor.lockState = CursorLockMode.Locked;
          }
          else 
          {
            Cursor.visible   = true;
            Cursor.lockState = CursorLockMode.Confined;
          }
        }
        
        input.ToggleInventory = input.ToggleInventory || _keyboard.iKey.wasPressedThisFrame;

        input.Drop = input.Drop || _keyboard.qKey.wasPressedThisFrame;
        input.Interact = input.Interact || _keyboard.eKey.wasPressedThisFrame;

        input.Action1 = input.Action1 || _keyboard.digit1Key.wasPressedThisFrame;
        input.Action2 = input.Action2 || _keyboard.digit2Key.wasPressedThisFrame;
        input.Action3 = input.Action3 || _keyboard.digit3Key.wasPressedThisFrame;
        input.Action4 = input.Action4 || _keyboard.digit4Key.wasPressedThisFrame;
        input.Action5 = input.Action5 || _keyboard.digit5Key.wasPressedThisFrame;
        input.Action6 = input.Action6 || _keyboard.digit6Key.wasPressedThisFrame;
        input.Action7 = input.Action7 || _keyboard.digit7Key.wasPressedThisFrame;

        input.PrimaryAttackClicked = input.PrimaryAttackClicked || _mouse.leftButton.wasPressedThisFrame;
        input.SecondaryAttackClicked = input.SecondaryAttackClicked || _mouse.rightButton.wasPressedThisFrame;
      }

      public void OnFixedUpdate()
      {
        ref var inputState    = ref InputState;
        ref var input         = ref inputState.Input;
        // var     moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector2 moveDirection = Vector2.zero;
        if(Input.GetKey(KeyCode.W)) moveDirection.y += 1;
        if(Input.GetKey(KeyCode.S)) moveDirection.y -= 1;
        if(Input.GetKey(KeyCode.A)) moveDirection.x -= 1;
        if(Input.GetKey(KeyCode.D)) moveDirection.x += 1;
        moveDirection.Normalize();
        // Debug.Log($"TICK: {TickManager.Instance.Tick} - Input {moveDirection}");

        input.moveDirection = moveDirection;

        input.Jump = _keyboard.spaceKey.isPressed;
        input.Sprint = _keyboard.leftShiftKey.isPressed;
        input.Crouch = _keyboard.leftCtrlKey.isPressed;
        
        input.PrimaryAttackHeld = _mouse.leftButton.isPressed;
        input.SecondaryAttackHeld = _mouse.rightButton.isPressed;
        if(input.Interact) Debug.Log("Interact!");
      }
    }
      /// <summary>
  /// Example definition of an INetworkStruct.
  /// </summary>
  [Serializable]
  public struct GameplayInput /* : ISerializable, IDeserializable<GameplayInput>*/
  {
    public Vector2 moveDirection;
    
    public Vector2 lookDelta;
    public float yEulerAngle;
    public float xEulerAngle;
    
    public NetworkButtons Actions;
    
    public byte weaponId;

    public bool Jump
    {
      get => Actions.IsSet(GameplayInputAction.Jump);
      set => Actions.Set(GameplayInputAction.Jump, value);
    }

    public bool Crouch
    {
      get => Actions.IsSet(GameplayInputAction.Crouch);
      set => Actions.Set(GameplayInputAction.Crouch, value);
    }
    
    public bool Sprint
    {
      get => Actions.IsSet(GameplayInputAction.Sprint);
      set => Actions.Set(GameplayInputAction.Sprint, value);
    }

    public bool Interact
    {
      get => Actions.IsSet(GameplayInputAction.Interact);
      set => Actions.Set(GameplayInputAction.Interact, value);
    }
    public bool Drop
    {
      get => Actions.IsSet(GameplayInputAction.Drop);
      set => Actions.Set(GameplayInputAction.Drop, value);
    }
    public bool Reload
    {
      get => Actions.IsSet(GameplayInputAction.Reload);
      set => Actions.Set(GameplayInputAction.Reload, value);
    }

    public bool PrimaryAttackClicked
    {
      get => Actions.IsSet(GameplayInputAction.Attack_Primary_Click);
      set => Actions.Set(GameplayInputAction.Attack_Primary_Click, value);
    }
    
    public bool PrimaryAttackHeld
    {
      get => Actions.IsSet(GameplayInputAction.Attack_Primary_Held);
      set => Actions.Set(GameplayInputAction.Attack_Primary_Held, value);
    }
    public bool SecondaryAttackClicked
    {
      get => Actions.IsSet(GameplayInputAction.Attack_Secondary_Click);
      set => Actions.Set(GameplayInputAction.Attack_Secondary_Click, value);
    }
    
    public bool SecondaryAttackHeld
    {
      get => Actions.IsSet(GameplayInputAction.Attack_Secondary_Held);
      set => Actions.Set(GameplayInputAction.Attack_Secondary_Held, value);
    }

    public bool Aim
    {
      get => Actions.IsSet(GameplayInputAction.Aim);
      set => Actions.Set(GameplayInputAction.Aim, value);
    }

    public bool Block
    {
      get => Actions.IsSet(GameplayInputAction.Block);
      set => Actions.Set(GameplayInputAction.Block, value);
    }

    public bool FlashLight
    {
      get => Actions.IsSet(GameplayInputAction.FlashLight);
      set => Actions.Set(GameplayInputAction.FlashLight, value);
    }
  
    public bool Kick
    {
      get => Actions.IsSet(GameplayInputAction.Kick);
      set => Actions.Set(GameplayInputAction.Kick, value);
    }

    public bool Heal
    {
      get => Actions.IsSet(GameplayInputAction.Heal);
      set => Actions.Set(GameplayInputAction.Heal, value);
    }

    public bool Action1
    {
      get => Actions.IsSet(GameplayInputAction.Action1);
      set => Actions.Set(GameplayInputAction.Action1, value);
    }

    public bool Action2
    {
      get => Actions.IsSet(GameplayInputAction.Action2);
      set => Actions.Set(GameplayInputAction.Action2, value);
    }

    public bool Action3
    {
      get => Actions.IsSet(GameplayInputAction.Action3);
      set => Actions.Set(GameplayInputAction.Action3, value);
    }

    public bool Action4
    {
      get => Actions.IsSet(GameplayInputAction.Action4);
      set => Actions.Set(GameplayInputAction.Action4, value);
    }

    public bool Action5
    {
      get => Actions.IsSet(GameplayInputAction.Action5);
      set => Actions.Set(GameplayInputAction.Action5, value);
    }
     public bool Action6
    {
      get => Actions.IsSet(GameplayInputAction.Action6);
      set => Actions.Set(GameplayInputAction.Action6, value);
    }
    
     public bool Action7
    {
      get => Actions.IsSet(GameplayInputAction.Action7);
      set => Actions.Set(GameplayInputAction.Action7, value);
    }
    
    public bool ToggleCursor
    {
      get => Actions.IsSet(GameplayInputAction.ToggleCursor);
      set => Actions.Set(GameplayInputAction.ToggleCursor, value);
    }
    
    public bool Escape
    {
      get => Actions.IsSet(GameplayInputAction.Escape);
      set => Actions.Set(GameplayInputAction.Escape, value);
    }
    public bool ToggleInventory
    {
      get => Actions.IsSet(GameplayInputAction.ToggleInventory);
      set => Actions.Set(GameplayInputAction.ToggleInventory, value);
    }

    // public void Serialize(Message message)
    // {
    //   message.AddVector3(moveDirection);
    //   message.AddFloat(yEulerAngle);
    //   message.AddFloat(xEulerAngle);
    //   message.AddInt(Actions.Bits);
    //   message.AddByte(weaponId);
    // }

    public int Size => 25;

    // public GameplayInput Deserialize(Message message)
    // {
    //   moveDirection = message.GetVector3();
    //   yEulerAngle   = message.GetFloat();
    //   xEulerAngle   = message.GetFloat();
    //   Actions       = (NetworkButtons) message.GetInt();
    //   weaponId      = message.GetByte();
    //
    //   return this;
    // }
  }
}