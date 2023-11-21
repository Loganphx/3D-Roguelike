using ECS.Movement.Services;
using UnityEngine;

internal class PlayerMovementComponent : IComponent<PlayerMovementState>
{
  private Transform _playerTransform;
  private PhysicsScene _physicsScene;

  public PlayerMovementState MovementState;
  public PlayerMovementState State => MovementState;

  private RaycastHit[] _hits = new RaycastHit[5];
  private LayerMask _collisionLayerMask;


  public PlayerMovementComponent(Transform playerTransform, Collider collider, PhysicsScene physicsScene,
    LayerMask collisionLayerMask)
  {
    _playerTransform = playerTransform;
    _physicsScene = physicsScene;

    _collisionLayerMask = collisionLayerMask;

    MovementState = new PlayerMovementState()
    {
      maxBounces = 5,
      skinWidth = 0.2f,
      maxSlopeAngle = 45,
      gravity = new Vector3(0, -15 / 50f, 0),
      currentGravityForce = new Vector3(0, 0, 0),
      movementSpeed = 3.2f,
      movementSpeedMultiplier = 1f,
      jumpHeight = 2,
    };
    CalculateBounds(collider, ref MovementState);
  }

  public void ProcessInput(ref GameplayInput input)
  {
    Move(ref input);
  }

  private void Move(ref GameplayInput playerInput)
  {
    var moveDirection = playerInput.moveDirection;
    // Debug.Log($"TICK: {TickManager.Instance.Tick} - Moving w/ {moveDirection}");
    if (playerInput.Jump && MovementState.isGrounded)
    {
      Jump(ref MovementState, _playerTransform);
      return;
    }

    var velocity = _playerTransform.forward * moveDirection.y +
                   _playerTransform.rotation * Vector3.right * moveDirection.x;
    Move(ref MovementState,
      _playerTransform,
      velocity * (MovementState.movementSpeed * MovementState.movementSpeedMultiplier *
                  Time.fixedDeltaTime));
  }

  private void Jump(ref PlayerMovementState playerMovementData, Transform transformComponent)
  {
    playerMovementData.currentGravityForce =
      new Vector3(0, playerMovementData.jumpHeight * 175 * Time.fixedDeltaTime, 0);
    var transformPos = transformComponent.position;
    var moveAmount = HandleGravity(ref playerMovementData, transformPos, Vector3.zero);
    transformComponent.position = transformPos + moveAmount;
  }

  public void Move(
    ref PlayerMovementState playerMovementData,
    Transform transform,
    Vector3 moveAmount
  )
  {
    var transformPos = transform.position;
    moveAmount = CollideAndSlide(ref playerMovementData, moveAmount, transformPos, 0,
      false, moveAmount);

    if (!playerMovementData.isGrounded)
    {
      moveAmount += HandleGravity(ref playerMovementData, transformPos, moveAmount);
    }

    transform.position = transformPos + moveAmount;
  }

  private Vector3 HandleGravity(ref PlayerMovementState playerMovementData,
    Vector3 position,
    Vector3 moveAmount)
  {
    moveAmount += CollideAndSlide(ref playerMovementData,
      playerMovementData.currentGravityForce * Time.fixedDeltaTime,
      position + moveAmount,
      0,
      true,
      playerMovementData.currentGravityForce);

    if (!playerMovementData.isGrounded)
    {
      playerMovementData.currentGravityForce += playerMovementData.gravity;
    }

    return moveAmount;
  }

  private void CalculateBounds(Collider collider, ref PlayerMovementState playerMovementData)
  {
    var bounds = collider.bounds;
    bounds.Expand(-2 * playerMovementData.skinWidth);
    playerMovementData.extents = bounds.extents;
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="playerMovementData"></param>
  /// <param name="vel">Input Velocity</param>
  /// <param name="pos">Current Position</param>
  /// <param name="depth">Used for recursion can be ignored by default</param>
  /// <param name="gravityPass"></param>
  /// <param name="initialVelocity"></param>
  /// <returns></returns>
  private Vector3 CollideAndSlide(
    ref PlayerMovementState playerMovementData,
    Vector3 vel,
    Vector3 pos,
    int depth,
    bool gravityPass,
    Vector3 initialVelocity)
  {
    if (depth >= playerMovementData.maxBounces) return Vector3.zero;

    float dist = vel.magnitude + playerMovementData.skinWidth;
    Debug.DrawLine(pos, pos + vel * dist, Color.blue);
    var count = _physicsScene.SphereCast(pos, playerMovementData.extents.x, vel.normalized, _hits, dist,
      _collisionLayerMask, QueryTriggerInteraction.Ignore);
    if (count == 0)
    {
      playerMovementData.isGrounded = false;
      return vel;
    }

    playerMovementData.isGrounded = true;
    var hit = _hits.SortRaycastHits(count);
    playerMovementData.lastHit = hit;

    playerMovementData.snapToSurface = vel.normalized * (hit.distance - playerMovementData.skinWidth);
    Vector3 leftOver = vel - playerMovementData.snapToSurface;
    float angle = Vector3.Angle(Vector3.up, hit.normal);

    if (playerMovementData.snapToSurface.sqrMagnitude <= playerMovementData.skinWidth)
      playerMovementData.snapToSurface = Vector3.zero;

    if (angle <= playerMovementData.maxSlopeAngle)
    {
      if (gravityPass) return playerMovementData.snapToSurface;

      leftOver = ProjectAndScale(leftOver, hit.normal);
    }
    // wall or steep slope.
    else
    {
      float scale = 1 - Vector3.Dot(new Vector3(hit.normal.x, 0, hit.normal.z).normalized,
        -new Vector3(initialVelocity.x, 0, initialVelocity.z).normalized);

      if (playerMovementData.isGrounded && !gravityPass)
      {
        leftOver = ProjectAndScale(new Vector3(leftOver.x, 0, leftOver.z),
          new Vector3(hit.normal.x, 0, hit.normal.z));
      }

      leftOver = ProjectAndScale(leftOver, hit.normal) * scale;
    }

    return playerMovementData.snapToSurface +
           CollideAndSlide(ref playerMovementData, leftOver, pos + playerMovementData.snapToSurface,
             depth + 1, gravityPass,
             initialVelocity);
  }

  private static Vector3 ProjectAndScale(Vector3 force, Vector3 normal)
  {
    float forceMagnitude = force.magnitude;
    force = Vector3.ProjectOnPlane(force, normal).normalized;
    force *= forceMagnitude;

    return force;
  }
}