using System;
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

public class DamageColliderComponent : MonoBehaviour
{
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
            if (hitObject != this.gameObject && hitObject.GetComponent<CharacterBase>())//change to checking if damage component
            {
                Debug.Log("Deal Damage!!");
            }
        }
    }
    /*public void DrawWireCapsule(Vector3 start, Vector3 end, float radius)
    {
        #if UNITY_EDITOR
        // Special case when both points are in the same position
        if (start == end)
        {
            // DrawWireSphere works only in gizmo methods
            Gizmos.DrawWireSphere(start, radius);
            return;
        }
        using (new UnityEditor.Handles.DrawingScope(Gizmos.color, Gizmos.matrix))
        {
            Quaternion p1Rotation = Quaternion.LookRotation(start - end);
            Quaternion p2Rotation = Quaternion.LookRotation(end - start);
            // Check if capsule direction is collinear to Vector.up
            float c = Vector3.Dot((start - end).normalized, Vector3.up);
            if (c == 1f || c == -1f)
            {
                // Fix rotation
                p2Rotation = Quaternion.Euler(p2Rotation.eulerAngles.x, p2Rotation.eulerAngles.y + 180f, p2Rotation.eulerAngles.z);
            }
            // First side
            UnityEditor.Handles.DrawWireArc(start, p1Rotation * Vector3.left, p1Rotation * Vector3.down, 180f, radius);
            UnityEditor.Handles.DrawWireArc(start, p1Rotation * Vector3.up, p1Rotation * Vector3.left, 180f, radius);
            UnityEditor.Handles.DrawWireDisc(start, (end - start).normalized, radius);
            // Second side
            UnityEditor.Handles.DrawWireArc(end, p2Rotation * Vector3.left, p2Rotation * Vector3.down, 180f, radius);
            UnityEditor.Handles.DrawWireArc(end, p2Rotation * Vector3.up, p2Rotation * Vector3.left, 180f, radius);
            UnityEditor.Handles.DrawWireDisc(end, (start - end).normalized, radius);
            // Lines
            UnityEditor.Handles.DrawLine(start + p1Rotation * Vector3.down * radius, end + p2Rotation * Vector3.down * radius);
            UnityEditor.Handles.DrawLine(start + p1Rotation * Vector3.left * radius, end + p2Rotation * Vector3.right * radius);
            UnityEditor.Handles.DrawLine(start + p1Rotation * Vector3.up * radius, end + p2Rotation * Vector3.up * radius);
            UnityEditor.Handles.DrawLine(start + p1Rotation * Vector3.right * radius, end + p2Rotation * Vector3.left * radius);
        }
        #endif
    }*/
}
