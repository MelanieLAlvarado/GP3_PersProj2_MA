using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAttackShapeType { Sphere, Capsule, Box}

public class DamageColliderComponent : DamageComponent
{
    GameObject _owner;
    Collider _attackCollider;
    AttackInfo _attack;
    HashSet<CharacterBase> _hitCharacters = new HashSet<CharacterBase>();
    Dictionary<CharacterController, Coroutine> _enableControllerDict = new Dictionary<CharacterController, Coroutine>();
    float controllerEnableTime = 1f;

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
        CharacterBase charBase = other.GetComponent<CharacterBase>();
        if (!charBase || other.gameObject == _owner)
        {
            return;
        }
        if (!_hitCharacters.Contains(charBase))
        {
            Debug.Log($"Hit {charBase.gameObject.name}");
            if (ShouldDamage(other.gameObject))
            {
                _hitCharacters.Add(charBase);

                ApplyDamage(other.gameObject, _attack.damageDealt);
                LaunchTarget(other.gameObject);
            }
        }
    }

    private void LaunchTarget(GameObject target)
    {
        LaunchComponent launchComponent = target.GetComponent<LaunchComponent>();
        if (launchComponent)
        {
            launchComponent.Launch(_owner.transform.forward, _attack.hitForce, true);
        }
    }
}
