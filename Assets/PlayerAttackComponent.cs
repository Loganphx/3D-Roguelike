using ECS.Movement.Services;
using UnityEngine;

internal struct PlayerAttackState : IState
{
    public float RemainingCooldown;
    public float Cooldown;
}
internal class PlayerAttackComponent : IComponent<PlayerAttackState>
{
    private readonly IPlayer           _player;
    private readonly Transform         _cameraTransform;
    private readonly PhysicsScene      _physicsScene;
    private readonly RaycastHit[]      _hits = new RaycastHit[5];
    
    private PlayerAttackState _state = new PlayerAttackState()
    {
        Cooldown = 0.5f,
        RemainingCooldown = 0,
    };
    
    public PlayerAttackComponent(IPlayer player, Transform cameraTransform,
        PhysicsScene physicsScene)
    {
        _player = player;
        _cameraTransform = cameraTransform;
        _physicsScene = physicsScene;
    }
    public void ProcessInput(ref GameplayInput input)
    {
        ref var state = ref _state;
        
        if (input.PrimaryAttackClicked && state.RemainingCooldown < 0)
        {
            var hits = _physicsScene.Raycast(_cameraTransform.position, _cameraTransform.forward, _hits, 5f,
                1 << LayerMask.NameToLayer("Damagable"), QueryTriggerInteraction.Collide);
            Debug.Log($"Primary Attack Clicked: {hits}");
            if(hits == 0) return;
            
            var hit = _hits[0];
            var damagable = hit.collider.transform.root.GetComponent<IDamagable>();
            damagable?.TakeDamage(_player, 10);
            state.RemainingCooldown = state.Cooldown;
        }
        
        if (state.RemainingCooldown >= 0)
        {
            state.RemainingCooldown -= Time.deltaTime;
            // Debug.Log($"Cooldown: {state.RemainingCooldown}");
            return;
        }

    }

    public PlayerAttackState State => _state;
}