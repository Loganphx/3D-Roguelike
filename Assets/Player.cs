using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using ECS.Movement.Services;
using Unity.VisualScripting;
using UnityEngine;

// TODO: add a component that can be used to emit changes to the network.

/// <summary>
/// If we use components we can loop them and grab the network interface off of them, then use that to emit changes to clients.
/// </summary>
///


public class Player : MonoBehaviour
{
    private PhysicsScene _physicsScene;
    private Transform    _playerTransform;

    private PlayerMovementData    _playerMovementData;
    private PlayerInputComponent  _playerInput;
    private PlayerCameraComponent _playerCameraComponent;
    private RaycastHit[]          _hits = new RaycastHit[5];
    private LayerMask             _collisionLayerMask;


    private void Awake()
    {
        _playerTransform = transform;
        _playerMovementData = new PlayerMovementData()
        {
            maxBounces              = 5,
            skinWidth               = 0.2f,
            maxSlopeAngle           = 45,
            gravity                 = new Vector3(0, -15 / 50f, 0),
            currentGravityForce     = new Vector3(0, 0,         0),
            movementSpeed           = 3.2f,
            movementSpeedMultiplier = 1f,
            jumpHeight              = 2,
        };
        var collider = GetComponentsInChildren<Collider>()[0];
        CalculateBounds(collider, ref _playerMovementData);

        _physicsScene       = gameObject.scene.GetPhysicsScene();
        _collisionLayerMask = ~(1 << LayerMask.NameToLayer("Player"));
    }

    private void Start()
    {
        _playerInput = new PlayerInputComponent();
        var cameraTransform = transform.GetComponentInChildren<Camera>().transform;
        _playerCameraComponent = new PlayerCameraComponent(_playerTransform, cameraTransform);
    }

    private void Update()
    {
        _playerInput.OnUpdate();

        _playerCameraComponent.ProcessInput(ref _playerInput.InputState);
    }

    private void FixedUpdate()
    {
        _playerInput.OnFixedUpdate();

        ProcessInput();
    }

    private void ProcessInput()
    {
        Move();
    }

    private void Move()
    {
        var moveDirection = _playerInput.State.Input.moveDirection;
        if (_playerInput.State.Input.Jump && _playerMovementData.isGrounded)
        {
            Jump(ref _playerMovementData, transform);
            return;
        }

        var velocity = _playerTransform.forward  * moveDirection.y +
                       _playerTransform.rotation * Vector3.right * moveDirection.x;
        Move(ref _playerMovementData,
            _playerTransform,
            velocity * (_playerMovementData.movementSpeed * _playerMovementData.movementSpeedMultiplier *
                        Time.fixedDeltaTime));
    }

    private void Jump(ref PlayerMovementData playerMovementData, Transform transformComponent)
    {
        playerMovementData.currentGravityForce =
            new Vector3(0, playerMovementData.jumpHeight * 175 * Time.fixedDeltaTime, 0);
        var transformPos = transformComponent.position;
        var moveAmount   = HandleGravity(ref playerMovementData, transformPos, Vector3.zero);
        transformComponent.position = transformPos + moveAmount;
    }

    public void Move(
        ref PlayerMovementData playerMovementData,
        Transform              transform,
        Vector3                moveAmount
    )
    {
        var transformPos = transform.position;
        moveAmount = CollideAndSlide(ref playerMovementData, moveAmount, transformPos, 0,
            false,                                           moveAmount);

        if (!playerMovementData.isGrounded)
        {
            moveAmount += HandleGravity(ref playerMovementData, transformPos, moveAmount);
        }

        transform.position = transformPos + moveAmount;
    }

    private Vector3 HandleGravity(ref PlayerMovementData playerMovementData,
        Vector3                                          position,
        Vector3                                          moveAmount)
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

    private void CalculateBounds(Collider collider, ref PlayerMovementData playerMovementData)
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
        ref PlayerMovementData playerMovementData,
        Vector3                vel,
        Vector3                pos,
        int                    depth,
        bool                   gravityPass,
        Vector3                initialVelocity)
    {
        if (depth >= playerMovementData.maxBounces) return Vector3.zero;

        float dist = vel.magnitude + playerMovementData.skinWidth;
        Debug.DrawLine(pos, pos    + vel * dist, Color.blue);
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
        Vector3 leftOver = vel                                            - playerMovementData.snapToSurface;
        float   angle    = Vector3.Angle(Vector3.up, hit.normal);

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
                    new Vector3(hit.normal.x,                      0, hit.normal.z));
            }

            leftOver = ProjectAndScale(leftOver, hit.normal) * scale;
        }

        return playerMovementData.snapToSurface +
               CollideAndSlide(ref playerMovementData, leftOver, pos + playerMovementData.snapToSurface,
                   depth                                             + 1, gravityPass,
                   initialVelocity);
    }

    private static Vector3 ProjectAndScale(Vector3 force, Vector3 normal)
    {
        float forceMagnitude = force.magnitude;
        force =  Vector3.ProjectOnPlane(force, normal).normalized;
        force *= forceMagnitude;

        return force;
    }
}