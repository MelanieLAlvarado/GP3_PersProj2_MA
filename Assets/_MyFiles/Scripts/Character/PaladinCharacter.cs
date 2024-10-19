using UnityEngine;

public class PaladinCharacter : CharacterBase
{
    [Header("Paladin Attack Options")]
    [SerializeField] AttackOptions attack1;



    /*Vector3 origin, float radius, float damageToDeal
        Vector3 beginPoint, Vector3 endPoint, float radius, float damageToDeal
        Vector3 origin, Vector3 halfExtents, Quaternion rotation, float damageToDeal*/

    public override void Attack1()
    {
        //GetDamageColliderComponent().HitColliderCapsule();
    }
    public override void Attack2()
    {

    }
}
