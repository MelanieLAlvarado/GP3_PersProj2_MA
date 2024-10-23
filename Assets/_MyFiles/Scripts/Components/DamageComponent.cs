using UnityEngine;

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
        CharacterBase charBase = target.GetComponent<CharacterBase>();
        if (charBase) 
        {
            charBase.HitReaction();
        }
    }
}
