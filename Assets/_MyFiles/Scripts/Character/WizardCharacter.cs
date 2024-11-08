using UnityEngine;

public class WizardCharacter : CharacterBase
{
    [Header("Projectile Options")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileVelocity = 2f;

    public void Shoot() 
    {
        if (!GetComponent<AttackComponent>() || !projectilePrefab || _bIsDead == true) 
        {
            return;
        }

        AttackInfo attack = GetComponent<AttackComponent>().GetCurrentAttackInfo();
        GameObject projectile = Instantiate(projectilePrefab, attack.origin.position, Quaternion.identity);
        
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript)
        {
            projectileScript.SetOwner(gameObject);
        }

        DamageColliderComponent damageColliderComp = projectile.GetComponent<DamageColliderComponent>();
        if (damageColliderComp)
        { 
            damageColliderComp.SetOwner(gameObject);
            damageColliderComp.SpawnAttackCollider(attack);
        }

        LaunchComponent launchComp = projectile.GetComponent<LaunchComponent>();
        if (launchComp)
        {
            launchComp.Launch(transform.forward, projectileVelocity, true, true);
        }
    }
}
