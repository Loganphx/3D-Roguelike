using ECS.Core.Components;
using ECS.Movement.Services;
using UnityEngine;

// [RequireComponent(typeof(CollideAndSlideComponent))]
[RequireComponent(typeof(PlayerInputComponent))]
// [RequireComponent(typeof(TransformComponent))]
public class PlayerEntity : MonoBehaviour, IEntity
{
    public IComponent[] Components { get; set; }

    /// <summary>
    /// current goal is to get the player to move around the world.
    /// then we can start to add in the other components.
    /// </summary>
    ///
    /// for the player to move we need a transform, player input, and 
    public void Awake()
    {
        Components = transform.GetComponents<IComponent>();
    }

    public void FixedUpdate()
    {
        for(int i = 0; i < Components.Length; i++)
        {
            var component = Components[i];
            component.OnFixedUpdate();
        }
    }
}