using UnityEngine;

public class PaladinCharacter : CharacterBase
{
    /*[Header("Attack Options")]
    AttackInfo _currentAttack;
    [SerializeField] AttackInfo attack1;
    [SerializeField] AttackInfo attack2;



    /*Vector3 origin, float radius, float damageToDeal
        Vector3 beginPoint, Vector3 endPoint, float radius, float damageToDeal
        Vector3 origin, Vector3 halfExtents, Quaternion rotation, float damageToDeal

    private void Start()
    {
        attack2.attackEnd = attack2.origin.position + (attack2.origin.forward * attack2.rangeLength);

        attack1.isAttackActive = false;
        attack2.isAttackActive = false;
    }
    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.E))
        {
            this.Attack2();
        }
        if (_currentAttack.isAttackActive == true) 
        {
            GetDamageColliderComponent().ProcessAttackType(_currentAttack);
        }
    }
    public override void Attack1()
    {
        if (!attack1.origin) 
        {
            Debug.LogError("No Origin point! please check attack info...");
            return;
        }

        _currentAttack = attack1;
        _currentAttack.isAttackActive = true;
    }
    public override void Attack2()
    {
        if (!attack2.origin)
        {
            Debug.LogError("No Origin point! please check attack info...");
            return;
        }
        _currentAttack = attack2;
        _currentAttack.isAttackActive = true;
        /*attack2.attackEnd = attack2.origin.position + (attack2.origin.forward * attack2.rangeLength);
        GetDamageColliderComponent().HitColliderCapsule(attack2.origin.position, attack2.attackEnd, attack2.radius, attack2.damageDealt);
    
    }
    public override void OnDrawAttacks()
    {
        if (attack1.origin)
        { 
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(attack2.origin.position, attack2.radius);//start point of capsule collider
            Gizmos.DrawWireSphere(attack2.attackEnd, attack2.radius);      //end point of capsule collider
        }
        if (attack2.origin)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(attack2.origin.position, attack2.radius);//start point of capsule collider
            Gizmos.DrawWireSphere(attack2.attackEnd, attack2.radius);      //end point of capsule collider
        }
    }*/
}
