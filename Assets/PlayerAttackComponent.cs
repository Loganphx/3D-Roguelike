using ECS.Movement.Services;
using UnityEngine;

internal class PlayerAttackComponent
{
    private readonly IPlayer _player;
    private readonly Transform _cameraTransform;
    private readonly PhysicsScene _physicsScene;
    private readonly RaycastHit[] _hits = new RaycastHit[5];

    public PlayerAttackComponent(IPlayer player, Transform cameraTransform,
        PhysicsScene physicsScene)
    {
        _player = player;
        _cameraTransform = cameraTransform;
        _physicsScene = physicsScene;
    }
    public void ProcessInput(ref GameplayInput input)
    {
        if (input.PrimaryAttackClicked)
        {
            Debug.Log("Primary Attack Clicked");
            var hits = _physicsScene.Raycast(_cameraTransform.position, _cameraTransform.forward, _hits, 5f,
                1 << LayerMask.NameToLayer("Damagable"), QueryTriggerInteraction.Collide);
            if(hits == 0) return;
            
            var hit = _hits[0];
            var damagable = hit.collider.transform.root.GetComponent<IDamagable>();
            damagable?.TakeDamage(_player, 10);
        }
    }
}