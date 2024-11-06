using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DamageComponent : MonoBehaviour
{
    public bool ShouldDamage(GameObject target) 
    {
        if (target.gameObject != this.gameObject && target.GetComponent<CharacterBase>())
        {
            return true;
        }
        return false;
    }
    public void ApplyDamage(GameObject target, float damageToDeal) 
    {
        Debug.Log($"target: '{target.name}' gets '{damageToDeal}'");
        HealthComponent _healthComponent = target.GetComponent<HealthComponent>();
        if (_healthComponent) 
        {
            _healthComponent.ChangeHealth(-damageToDeal, this.gameObject);
        }
        CharacterBase targetCharBase = target.GetComponent<CharacterBase>();
        if (targetCharBase) 
        {
            targetCharBase.HitReaction();
        }

        //LaunchTarget(target);
    }
    /*private void LaunchTarget(GameObject target) 
    {
        LaunchComponent launchComponent = target.GetComponent<LaunchComponent>();
        if (launchComponent)
        {
            float hitForce = GetComponent<CharacterBase>().GetHitForce();
            launchComponent.Launch(transform.forward, hitForce, true);
        }
    }*/
}
