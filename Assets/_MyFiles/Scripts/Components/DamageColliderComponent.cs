using UnityEngine;

public class DamageColliderComponent : MonoBehaviour
{
    public void HitColliderSphere(Vector3 origin, float radius) 
    {
        
    }

    public void HitColliderCapsule(Vector3 beginPoint, Vector3 endPoint, float radius) 
    {
        
    }

    public void HitColliderBox(Vector3 origin, Vector3 halfExtents, Quaternion rotation) 
    {
        
    }

    public void HitCollider(Transform origin, float range) 
    {
        //Physics.OverlapSphere(position, radius, layerMask, [querytriggerinteraction])

        //Physics.OverlapCapsule(point0, point1, radius, layerMask, [querytriggerinteraction])

        //Physics.OverlapBox(center, halfExtents, orientation, layerMask, [querytriggerinteraction]);
    }
}
