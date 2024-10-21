using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

[Serializable]
public struct AttackInfo
{
    public EAttackShapeType attackShape;
    public Transform origin; //get postition off of this
    /*[SerializeField]*/
    public Vector3 attackEnd;//attack end
    public float radius; //capsules and spheres
    public float rangeLength; //for capsules
    /*[SerializeField]*/
    private Quaternion _attackDirection; //if dir needed (capsule & box colliders)

    public float damageDealt;
    public bool isAttackActive;
}

public enum EAttackShapeType { Sphere, Capsule, Box}

public class DamageColliderComponent : DamageComponent
{
    private HashSet<GameObject> _hitTargets = new HashSet<GameObject>();
    public void ClearHitTargets() { _hitTargets.Clear(); }
    public void ProcessAttackType(AttackInfo attack) 
    {
        if (!attack.origin)
        {
            Debug.LogError("No Origin point! please check attack info...");
            return;
        }

        switch (attack.attackShape) 
        {
            case EAttackShapeType.Sphere:
                HitColliderSphere(attack.origin.position, attack.radius, attack.damageDealt);
                break;
            case EAttackShapeType.Capsule:
                //Transform origin = attack.origin;//used to shorten the name
                attack.attackEnd = attack.origin.position + (attack.origin.forward * attack.rangeLength);
                HitColliderCapsule(attack.origin.position, attack.attackEnd, attack.radius, attack.damageDealt);
                break;
            case EAttackShapeType.Box:
                //HitColliderBox()
                break;
        }
    }
    public void HitColliderSphere(Vector3 origin, float radius, float damageToDeal) 
    {
        Collider[] hitObjects = Physics.OverlapSphere(origin, radius);
        ProcessHitObjects(hitObjects, damageToDeal);
    }

    public void HitColliderCapsule(Vector3 beginPoint, Vector3 endPoint, float radius, float damageToDeal) 
    {
        Collider[] hitObjects = Physics.OverlapCapsule(beginPoint, endPoint, radius);
        ProcessHitObjects(hitObjects, damageToDeal);
    }

    public void HitColliderBox(Vector3 origin, Vector3 halfExtents, Quaternion rotation, float damageToDeal) 
    {
        Collider[] hitObjects = Physics.OverlapBox(origin, halfExtents, rotation);
        ProcessHitObjects(hitObjects, damageToDeal);
    }

    public void ProcessHitObjects(Collider[] hitObjects, float damageToDeal) 
    {
        foreach (Collider hitObject in hitObjects)
        {
            if (_hitTargets.Contains(hitObject.gameObject))
            {
                return;
            }
            if (ShouldDamage(hitObject.gameObject))
            {
                _hitTargets.Add(hitObject.gameObject);

                ApplyDamage(hitObject.gameObject, damageToDeal);
            }
        }
    }
}
