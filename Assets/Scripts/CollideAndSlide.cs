using UnityEngine;

namespace ECS.Movement.Services
{

  public static class Layers
  {
    public static int Default = 0;
    public static int Player = 3;
  }
  public struct StaminaComponent
  {
    public float CurrentStamina;
    public float MaxStamina;
    public bool HasChanged;
  }
  public struct PlayerMovementState : IState
  {
    public byte maxBounces;

    public float skinWidth;
    public float maxSlopeAngle;

    public Vector3 extents;
    public Vector3 gravity;
    public Vector3 currentGravityForce;

    public float movementSpeed;
    public float movementSpeedMultiplier;
    public float jumpHeight;

    public bool isGrounded;
    
    public Vector3 snapToSurface;

    public RaycastHit lastHit;
    
    public bool HasChanged;
  }
  
  // [Serializable]
  // public class CollideAndSlideComponent : MonoBehaviour
  // {
  //   private int _collisionLayerMask = ~(1 << Layers.Player);
  //   private RaycastHit[] _hits = new RaycastHit[3];
  //
  //   private      Transform                 playerTransform;
  //   private      PlayerInputComponent _playerInput;
  //   private      StaminaComponent          staminaComponent;
  //   private PhysicsScene _physicsScene;
  //   public const string                    ProfilerName = "CollideAndSlideSystem.OnFixedUpdate";
  //
  //   private void Awake()
  //   {
  //     playerTransform = transform;
  //     _physicsScene   = gameObject.scene.GetPhysicsScene();
  //     _playerInput    = GetComponent<PlayerInputComponent>();
  //     staminaComponent = new StaminaComponent()
  //     {
  //       CurrentStamina = 100,
  //       MaxStamina = 100,
  //       HasChanged = false
  //     };
  //     
  //   }
  //
  //   public void FixedUpdate()
  //   {
  //     Profiler.BeginSample(ProfilerName);
  //     HandleMovement();
  //     // foreach (var entity in _world.EntitiesByType[ConstEntityTypes.PlayerArchetypeId])
  //     // {
  //     //   var collideAndSlideComponent = entity.GetFirst<CollideAndSlideComponent>(CollideAndSlideComponent.HashId);
  //     // }
  //
  //     Profiler.EndSample();
  //   }
  //
  //   private void HandleMovement()
  //   {
  //     staminaComponent.HasChanged = false;
  //     
  //     var moveDirection = _playerInput.State.Input.moveDirection;
  //     
  //     if (_playerInput.State.Input.Aim)
  //     {
  //       moveDirection *= .5f;
  //     }
  //     
  //     if (_playerInput.State.Input.Sprint && staminaComponent.CurrentStamina > 0 &&
  //         moveDirection.y > 0)
  //     {
  //       //reduce stamina!
  //       staminaComponent.CurrentStamina--;
  //       staminaComponent.HasChanged = true;
  //       moveDirection.y *= 1.5f;
  //     }
  //     else if (staminaComponent.CurrentStamina < staminaComponent.MaxStamina)
  //     {
  //       staminaComponent.CurrentStamina++;
  //       staminaComponent.HasChanged = true;
  //     }
  //
  //     if (_playerInput.State.Input.Jump && _playerMovementData.isGrounded)
  //     {
  //       Jump(ref _playerMovementData, playerTransform);
  //       return;
  //     }
  //
  //     var velocity = playerTransform.forward * moveDirection.y +
  //                    playerTransform.rotation * Vector3.right * moveDirection.x;
  //     Move(ref _playerMovementData,
  //       playerTransform,
  //       velocity * (_playerMovementData.movementSpeed * _playerMovementData.movementSpeedMultiplier * Time.fixedDeltaTime));
  //   }
  //
  //   private void Jump(ref PlayerMovementData playerMovementData, Transform transformComponent)
  //   {
  //     playerMovementData.currentGravityForce =
  //       new Vector3(0, playerMovementData.jumpHeight * 175 * Time.fixedDeltaTime, 0);
  //     var transformPos = transformComponent.position;
  //     var moveAmount = HandleGravity(ref playerMovementData, transformPos, Vector3.zero);
  //     transformComponent.position = transformPos + moveAmount;
  //   }
  //
  //   public void Move(
  //     ref PlayerMovementData playerMovementData,
  //     Transform transform,
  //     Vector3 moveAmount
  //   )
  //   {
  //     var transformPos = transform.position;
  //     moveAmount = CollideAndSlide(ref playerMovementData, moveAmount, transformPos, 0,
  //       false, moveAmount);
  //
  //     if (!playerMovementData.isGrounded)
  //     {
  //       moveAmount+= HandleGravity(ref playerMovementData, transformPos, moveAmount);
  //     }
  //
  //     transform.position = transformPos + moveAmount;
  //   }
  //
  //   private Vector3 HandleGravity(ref PlayerMovementData playerMovementData,
  //     Vector3 position,
  //     Vector3 moveAmount)
  //   {
  //     moveAmount += CollideAndSlide(ref playerMovementData,
  //       playerMovementData.currentGravityForce * Time.fixedDeltaTime,
  //       position + moveAmount,
  //       0,
  //       true,
  //       playerMovementData.currentGravityForce);
  //
  //     if (!playerMovementData.isGrounded)
  //     {
  //       playerMovementData.currentGravityForce += playerMovementData.gravity;
  //     }
  //
  //     return moveAmount;
  //   }
  //
  //   private void CalculateBounds(Collider collider, ref PlayerMovementData playerMovementData)
  //   {
  //     var bounds = collider.bounds;
  //     bounds.Expand(-2 * playerMovementData.skinWidth);
  //     playerMovementData.extents = bounds.extents;
  //   }
  //
  //   /// <summary>
  //   /// 
  //   /// </summary>
  //   /// <param name="playerMovementData"></param>
  //   /// <param name="vel">Input Velocity</param>
  //   /// <param name="pos">Current Position</param>
  //   /// <param name="depth">Used for recursion can be ignored by default</param>
  //   /// <param name="gravityPass"></param>
  //   /// <param name="initialVelocity"></param>
  //   /// <returns></returns>
  //   private Vector3 CollideAndSlide(
  //     ref PlayerMovementData playerMovementData,
  //     Vector3 vel,
  //     Vector3 pos,
  //     int depth,
  //     bool gravityPass,
  //     Vector3 initialVelocity)
  //   {
  //     if (depth >= playerMovementData.maxBounces) return Vector3.zero;
  //
  //     float dist = vel.magnitude + playerMovementData.skinWidth;
  //     Debug.DrawLine(pos, pos+vel * dist, Color.blue );
  //     var count = _physicsScene.SphereCast(pos, playerMovementData.extents.x, vel.normalized, _hits, dist,
  //       _collisionLayerMask, QueryTriggerInteraction.Ignore);
  //     if (count == 0)
  //     {
  //       playerMovementData.isGrounded = false;
  //       return vel;
  //     }
  //
  //     playerMovementData.isGrounded = true;
  //     var hit = _hits.SortRaycastHits(count);
  //     playerMovementData.lastHit = hit;
  //     
  //     playerMovementData.snapToSurface = vel.normalized * (hit.distance - playerMovementData.skinWidth);
  //     Vector3 leftOver = vel - playerMovementData.snapToSurface;
  //     float angle = Vector3.Angle(Vector3.up, hit.normal);
  //
  //     if (playerMovementData.snapToSurface.sqrMagnitude <= playerMovementData.skinWidth)
  //       playerMovementData.snapToSurface = Vector3.zero;
  //
  //     if (angle <= playerMovementData.maxSlopeAngle)
  //     {
  //       if (gravityPass) return playerMovementData.snapToSurface;
  //
  //       leftOver = ProjectAndScale(leftOver, hit.normal);
  //     }
  //     // wall or steep slope.
  //     else
  //     {
  //       float scale = 1 - Vector3.Dot(new Vector3(hit.normal.x, 0, hit.normal.z).normalized,
  //         -new Vector3(initialVelocity.x, 0, initialVelocity.z).normalized);
  //
  //       if (playerMovementData.isGrounded && !gravityPass)
  //       {
  //         leftOver = ProjectAndScale(new Vector3(leftOver.x, 0, leftOver.z),
  //           new Vector3(hit.normal.x, 0, hit.normal.z));
  //       }
  //
  //       leftOver = ProjectAndScale(leftOver, hit.normal) * scale;
  //     }
  //
  //     return playerMovementData.snapToSurface +
  //            CollideAndSlide(ref playerMovementData, leftOver, pos + playerMovementData.snapToSurface,
  //              depth + 1, gravityPass,
  //              initialVelocity);
  //   }
  //
  //   private static Vector3 ProjectAndScale(Vector3 force, Vector3 normal)
  //   {
  //     float forceMagnitude = force.magnitude;
  //     force = Vector3.ProjectOnPlane(force, normal).normalized;
  //     force *= forceMagnitude;
  //
  //     return force;
  //   }
  // }
}