using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[Serializable]
public struct AttackInfo
{
    private DamageColliderComponent _damageColliderComponent;
    public EAttackShapeType attackShape;
    public Transform origin; //get postition off of this
    /*[SerializeField]*/
    public Vector3 attackEnd;//attack end
    public float radius; //capsules and spheres
    public float rangeLength; //for capsules
    /*[SerializeField]*/
    //private Quaternion _attackDirection; //if dir needed (capsule & box colliders)

    public float damageDealt;
    public bool bIsAttackActive;

    public void InitializeAttack(GameObject owner) 
    {
        bIsAttackActive = false;
        DamageColliderComponent attack = origin.GetComponent<DamageColliderComponent>();
        if (attack)
        {
            _damageColliderComponent = attack;
        }
        else
        {
            _damageColliderComponent = origin.AddComponent<DamageColliderComponent>();
        }
        _damageColliderComponent.SetOwner(owner);
    }
    public DamageColliderComponent GetAttack() { return _damageColliderComponent; }
}

public class AttackComponent : MonoBehaviour, IAttackInterface
{
    Animator _animator;
    AttackInfo _currentAttack;
    protected int _currentAttackId;
    private bool _bCanAttack = true;

    [SerializeField] private bool bDrawDebugAttacks = false;
    [Header("Attack Options")]
    [SerializeField] AttackInfo attack1;
    [SerializeField] AttackInfo attack2;

    protected static readonly int _attack1Id = Animator.StringToHash("Attack1");
    protected static readonly int _attack2Id = Animator.StringToHash("Attack2");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void Start()
    {
        attack1.InitializeAttack(gameObject);
        attack2.InitializeAttack(gameObject);
    }

    public void StartAttack1()
    {
        AssignAttack(attack1, _attack1Id);
    }
    public void StartAttack2()
    {
        AssignAttack(attack2, _attack2Id);
    }

    public void Attack() //attack physics will spawn (in Animation Events)
    {
        _currentAttack.bIsAttackActive = true;
        if (IsAttackValid())
        { 
            _currentAttack.GetAttack().SpawnAttackCollider(_currentAttack);
        }
    }
    private bool IsAttackValid() 
    {
        if (!_currentAttack.origin)
        {
            Debug.LogError("There is no origin on this attack!\n Please assign an origin");
            return false;
        }

        if (_currentAttack.GetAttack().gameObject != _currentAttack.origin.gameObject)
        {
            Debug.LogError("Attack Script not on origin! fixing...");
            Destroy(_currentAttack.GetAttack());
            _currentAttack.InitializeAttack(gameObject);
        }
        return true;
    }
    public void AssignAttack(AttackInfo attack, int animationId)
    {
        if (_animator && _bCanAttack == true)
        {
            _bCanAttack = false;
            _currentAttack = attack;
            PlayAnimation(animationId);
        }
    }
    protected void PlayAnimation(int animationId)
    {
        _currentAttackId = animationId;
        _animator.SetTrigger(_currentAttackId);
    }
    public void EndAttack() //attack physics wont spawn anymore (in Animation Events)
    {
        _currentAttack.bIsAttackActive = false;

        _currentAttack.GetAttack().RemoveAttackCollider();
    }
    public void ResetAttack() //attack ability is reinstated (in Animation Events)
    {
        _bCanAttack = true;
    }
    private void OnDrawGizmos()
    {
        if (bDrawDebugAttacks == true)
        {
            OnDrawAttacks();
        }
    }
    public virtual void OnDrawAttacks()
    {
        if (!_currentAttack.bIsAttackActive)
        {
            return;
        }

        switch (_currentAttack.attackShape)
        {
            case EAttackShapeType.Sphere:
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(_currentAttack.origin.position, _currentAttack.radius);
                break;
            case EAttackShapeType.Capsule:
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(_currentAttack.origin.position, _currentAttack.radius);//start point of capsule collider
                _currentAttack.attackEnd = _currentAttack.origin.position + (_currentAttack.origin.forward * _currentAttack.rangeLength);
                Gizmos.DrawWireSphere(_currentAttack.attackEnd, _currentAttack.radius);      //end point of capsule collider
                break;
            case EAttackShapeType.Box:
                Debug.Log("Still need to program this one in case it gets used");
                break;
        }
    }
}
