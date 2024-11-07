using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAttackShapeType { Sphere, Capsule, Box}

[RequireComponent(typeof(Rigidbody))]
public class DamageColliderComponent : DamageComponent
{
    Rigidbody _rigidBody;
    GameObject _owner;
    Collider _attackCollider;
    AttackInfo _attack;
    HashSet<CharacterBase> _hitCharacters = new HashSet<CharacterBase>();
    Dictionary<CharacterController, Coroutine> _enableControllerDict = new Dictionary<CharacterController, Coroutine>();
    Dictionary<ParticleSystem, Coroutine> _particleCoroutineDict = new Dictionary<ParticleSystem, Coroutine>();
    
    float controllerEnableTime = 1f;
    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.useGravity = false;
        _rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        //_rigidBody.isKinematic = true;
    }
    public void SetOwner(GameObject ownerObject) { _owner = ownerObject; }
    public void SpawnAttackCollider(AttackInfo attack)
    {
        _attack = attack;
        if (_attack.attackShape == EAttackShapeType.Sphere)
        {
            SphereCollider sphereAttack = gameObject.AddComponent<SphereCollider>();
            sphereAttack.radius = _attack.radius;
            _attackCollider = sphereAttack;
        }
        else if (_attack.attackShape == EAttackShapeType.Capsule)
        {
            CapsuleCollider capsuleAttack = gameObject.AddComponent<CapsuleCollider>();
            capsuleAttack.radius = _attack.radius;

            _attack.attackEnd = _attack.origin.position + (_attack.origin.forward * _attack.rangeLength);

            capsuleAttack.direction = 2;
            capsuleAttack.height = (_attack.radius * 2) + _attack.rangeLength;
            capsuleAttack.center = new Vector3(0, 0, _attack.rangeLength / 2);
            _attackCollider = capsuleAttack;

        }
        _attackCollider.isTrigger = true;
        _attackCollider.enabled = true;
    }
    public void RemoveAttackCollider()
    {
        _hitCharacters.Clear();
        Destroy(_attackCollider);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _owner)
        {
            return;
        }
        CharacterBase charBase = other.GetComponent<CharacterBase>();
        if (charBase && !_hitCharacters.Contains(charBase))
        {
            Debug.Log($"Hit {charBase.gameObject.name}");
            if (ShouldDamage(other.gameObject))
            {
                _hitCharacters.Add(charBase);

                ApplyDamage(other.gameObject, _attack.damageDealt);
                LaunchTarget(other.gameObject);
            }
        }
        Vector3 impactPoint = other.ClosestPoint(_attack.origin.position);
        SpawnVfx(impactPoint);
    }

    private void LaunchTarget(GameObject target)
    {
        LaunchComponent launchComponent = target.GetComponent<LaunchComponent>();
        if (launchComponent)
        {
            launchComponent.Launch(_owner.transform.forward, _attack.hitForce, true);
        }
    }
    private void SpawnVfx(Vector3 vfxSpawnPos)
    {
        if (!_attack.vfx)
        {
            return;
        }
        ParticleSystem newVfx;
        if (_attack.overrideVfxSpawnPoint)
        {
            Vector3 overridePos = _attack.overrideVfxSpawnPoint.position;
            newVfx = Instantiate(_attack.vfx, overridePos, Quaternion.identity);
            newVfx.transform.parent = _attack.overrideVfxSpawnPoint.transform;
        }
        else
        {
            newVfx = Instantiate(_attack.vfx, vfxSpawnPos, Quaternion.identity);
            newVfx.transform.parent = _attack.origin.transform;
        }

        Coroutine particleCoroutine = StartCoroutine(VfxDiscardTimer(newVfx));
        _particleCoroutineDict.Add(newVfx, particleCoroutine);
    }
    private IEnumerator VfxDiscardTimer(ParticleSystem newVfx) 
    {
        yield return new WaitForSeconds(1.0f);
        if (newVfx != null)
        { 
            _particleCoroutineDict.Remove(newVfx);
            Destroy(newVfx.gameObject);
        }
        Debug.Log("VFX Has been removed!");
    }
}
