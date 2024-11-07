using System;
using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(DamageColliderComponent))]
public class Projectile : MonoBehaviour
{
    GameObject _owner;
    [SerializeField] private GameObject activeParticle;
    [SerializeField] private ParticleSystem impactEffect;
    public void SetOwner(GameObject owner) { _owner = owner; }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _owner || other.gameObject == gameObject)
        {
            return;
        }

        if (activeParticle)
        { 
            Destroy(activeParticle);
        }
        Destroy(gameObject.GetComponent<Collider>());
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        if (impactEffect)
        { 
            Instantiate(impactEffect.gameObject, gameObject.transform);
        }
        StartCoroutine(DelayDestroyProjectile());
    }

    private IEnumerator DelayDestroyProjectile()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
