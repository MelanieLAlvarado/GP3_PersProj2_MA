using UnityEngine;
using static UnityEngine.UI.Image;

public class DamageColliderComponent : MonoBehaviour
{
    public void HitColliderSphere(Vector3 origin, float radius, float damageToDeal) 
    {
        Collider[] hitObjects = Physics.OverlapSphere(origin, radius);
        ProcessHitObjects(hitObjects);
    }

    public void HitColliderCapsule(Vector3 beginPoint, Vector3 endPoint, float radius, float damageToDeal) 
    {
        Collider[] hitObjects = Physics.OverlapCapsule(beginPoint, endPoint, radius);
        ProcessHitObjects(hitObjects);
    }

    public void HitColliderBox(Vector3 origin, Vector3 halfExtents, Quaternion rotation, float damageToDeal) 
    {
        Collider[] hitObjects = Physics.OverlapBox(origin, halfExtents, rotation);
        ProcessHitObjects(hitObjects);
    }

    public void ProcessHitObjects(Collider[] hitObjects) 
    {
        foreach (Collider hitObject in hitObjects)
        {
            if (hitObject != this.gameObject && hitObject.GetComponent<CharacterBase>())//change to checking if damage component
            {
                Debug.Log("Deal Damage!!");
            }
        }
    }
}
