using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    GameObject _owner;
    Collider _attackCollider;
    HashSet<CharacterBase> _hitCharacters = new HashSet<CharacterBase>();

    public void SetOwner(GameObject ownerObject) { _owner = ownerObject; }
    public void SpawnAttackCollider(AttackInfo attack)
    {
        if (attack.attackShape == EAttackShapeType.Sphere)
        {
            SphereCollider sphereAttack = gameObject.AddComponent<SphereCollider>();
            sphereAttack.radius = attack.radius;
            _attackCollider = sphereAttack;
        }
        else if (attack.attackShape == EAttackShapeType.Capsule)
        { 
            CapsuleCollider capsuleAttack = gameObject.AddComponent<CapsuleCollider>();
            capsuleAttack.radius = attack.radius;

            attack.attackEnd = attack.origin.position + (attack.origin.forward * attack.rangeLength);

            capsuleAttack.direction = 2;
            capsuleAttack.height = 1 + attack.rangeLength;
            Vector3 center = attack.origin.position + attack.attackEnd;
            Debug.Log($"Center of attack capsule: origin - {attack.origin.position}, end - {attack.attackEnd}, center - {center}");

            capsuleAttack.center = new Vector3(0,0,attack.rangeLength/2);//calculate here
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
            _hitCharacters.Add(charBase);
            Debug.Log($"Hit {charBase.gameObject.name}");
        }
    }

}
