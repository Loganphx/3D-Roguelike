using UnityEngine;

public class DamagableResource : Damagable
{
    private TOOL_TYPE requiredToolType;
    private int minimumToolDamage;
    
    public void Initialize(TOOL_TYPE requiredToolType, int minimumToolDamage)
    {
        this.requiredToolType = requiredToolType;
        this.minimumToolDamage = minimumToolDamage;
    }
    public override void OnHit(IDamager damager, Vector3 hitDirection, Vector3 hitPosition, TOOL_TYPE toolType, int damage)
    {
        if(!toolType.HasFlag(requiredToolType)) return;
        if(damage < minimumToolDamage) return;
        
        base.OnHit(damager, hitDirection, hitPosition, toolType, damage);
    }
}