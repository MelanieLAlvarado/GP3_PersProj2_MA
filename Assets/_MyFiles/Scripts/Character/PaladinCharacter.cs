using UnityEngine;

public class PaladinCharacter : CharacterBase
{
    [Header("Paladin Attack Options")]
    [SerializeField] AttackInfo attack1;
    [SerializeField] AttackInfo attack2;



    /*Vector3 origin, float radius, float damageToDeal
        Vector3 beginPoint, Vector3 endPoint, float radius, float damageToDeal
        Vector3 origin, Vector3 halfExtents, Quaternion rotation, float damageToDeal*/

    Vector3 _attackEnd;//debug rip

    private void Start()
    {
        Debug.Log($"DEBUG... start: {attack2.origin},end: {_attackEnd}");
        attack2.attackEnd = attack2.origin.forward * attack2.rangeLength;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            this.Attack2();
        }
    }
    public override void Attack1()
    {
        if (!attack1.origin) 
        {
            Debug.LogError("No Origin point! please check attack info...");
            return;
        }
        GetDamageColliderComponent().HitColliderSphere(attack1.origin.position, attack1.radius, attack1.damageDealt);
    }
    public override void Attack2()
    {
        //capsule collider is not workign... use other colliders for now.
        attack2.attackEnd = attack2.origin.forward * attack2.rangeLength;
        GetDamageColliderComponent().HitColliderCapsule(attack2.origin.position, attack2.attackEnd, attack2.radius, attack2.damageDealt);
    }
    public override void OnDrawAttacks()
    {
        if (attack1.origin)
        { 
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(attack1.origin.position, attack1.radius);
        }
       /* if (attack2.origin)
        {
            Gizmos.color = Color.cyan;
            GetDamageColliderComponent().DrawWireCapsule(attack2.origin.position, attack2.attackEnd, attack2.radius);
        }*/
    }
}
