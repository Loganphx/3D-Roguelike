using System.Collections;
using System.Collections.Generic;
using ECS.Movement.Services;
using UnityEngine;

// TODO: add a component that can be used to emit changes to the network.

/// <summary>
/// If we use components we can loop them and grab the network interface off of them, then use that to emit changes to clients.
/// </summary>
///
[RequireComponent(typeof(CollideAndSlideComponent))]
[RequireComponent(typeof(PlayerInputComponent))]
[RequireComponent(typeof(TransformComponent))]
public class Player : MonoBehaviour
{
    /// <summary>
    /// current goal is to get the player to move around the world.
    /// then we can start to add in the other components.
    /// </summary>
    ///
    /// for the player to move we need a transform, player input, and 
    public void Awake()
    {
        
    }
}
