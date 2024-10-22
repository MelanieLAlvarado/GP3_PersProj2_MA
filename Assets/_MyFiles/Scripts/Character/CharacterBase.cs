using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(DamageColliderComponent))]
[RequireComponent(typeof(HealthComponent))]
public class CharacterBase : MonoBehaviour, IAttackInterface
{
    private DamageColliderComponent _damageColliderComponent;
    private HealthComponent _healthComponent;

    PlayerController _playerController;
    Animator _animator;
    private static readonly int _speedId = Animator.StringToHash("Speed");
    protected static readonly int _attack1Id = Animator.StringToHash("Attack1");
    protected static readonly int _attack2Id = Animator.StringToHash("Attack2");

    protected static readonly int _deathId = Animator.StringToHash("Death");

    protected int _currentAttackId;

    [Header("Base Character Options")]
    private Vector3 _prevPosition;
    private Vector3 _faceDirection;

    [SerializeField] private float characterTurnSpeed = 10f;
    [SerializeField] private float animSpeedChangeRate = 0.4f;
    private float _animMoveSpeed = 0f;
    private float _currentSpeed = 0f;
    [SerializeField] private float maxSpeed = 3f;//will probably have it changed in CharacterChild class
    [SerializeField] private float jumpHeight = 3f;

    [Header("Attack Options")]
    private bool _canAttack = true;
    [SerializeField] private bool drawDebugAttacks = false;
    AttackInfo _currentAttack;
    [SerializeField] AttackInfo attack1;
    [SerializeField] AttackInfo attack2;

    GameObject _owner;
    public void SetOwner(GameObject owner) { _owner = owner; } //player will pass this in on spawn
    public DamageColliderComponent GetDamageColliderComponent() { return _damageColliderComponent; }
    public void SetFaceDirection(Vector3 directionToSet) { _faceDirection = directionToSet; }
    public float GetMaxSpeed() { return maxSpeed; }
    public float GetJumpHeight() {  return jumpHeight; }

    private void Awake()
    {
        _healthComponent = GetComponent<HealthComponent>();
        _healthComponent.OnDead += StartDeath;
        _animator = GetComponent<Animator>();
        _damageColliderComponent = GetComponent<DamageColliderComponent>();
        _prevPosition = transform.position;

        if (_faceDirection == new Vector3(0, 0, 0)) 
        {
            _faceDirection = new Vector3(90, 0, 0);
        }
    }
    private void Start()
    {
        attack1.isAttackActive = false;
        attack2.isAttackActive = false;
    }
    private void FixedUpdate()
    {
        Vector3 curMove = transform.position - _prevPosition;
        _currentSpeed = curMove.magnitude / Time.deltaTime;
        _prevPosition = transform.position;

        //Rotating whole character based on movement direction
        Quaternion goalRot = Quaternion.LookRotation(_faceDirection, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, goalRot, Time.deltaTime * characterTurnSpeed);
        //updating animating speed through a lerped value
        if (_animator)
        {
            _animMoveSpeed = Mathf.Lerp(_animMoveSpeed, _currentSpeed, Time.deltaTime * animSpeedChangeRate);
            _animator.SetFloat(_speedId, _animMoveSpeed);
        }

        if (_currentAttack.isAttackActive == true)
        {
            _damageColliderComponent.ProcessAttackType(_currentAttack);
        }
    }

    //Different Attack inputs (will be overridden in child classes)
    public void StartAttack1() 
    {
        AssignAttack(attack1, _attack1Id);
    }
    public void StartAttack2() 
    {
        AssignAttack(attack2, _attack2Id);
    }

    public virtual void Attack1()//attacks will be called in Animation Events (Change to "Attack" instead)
    {
        Debug.Log($"Attack1 '{_attack1Id}' Called");
        _currentAttack.isAttackActive = true;
    }
    public virtual void Attack2()
    {
        Debug.Log($"Attack2 '{_attack2Id}' Called");
        _currentAttack.isAttackActive = true;
    }
    public void Attack() //attack physics will spawn (in Animation Events)
    {
        _currentAttack.isAttackActive = true;
    }
    private void AssignAttack(AttackInfo attack, int animationId) 
    {
        if (_animator && _canAttack == true)
        {
            _canAttack = false;
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
        _currentAttack.isAttackActive = false;
        _damageColliderComponent.ClearHitTargets();
    }
    public void ResetAttack() //attack ability is reinstated (in Animation Events)
    {
        _canAttack = true;
    }
    private void StartDeath() 
    {
        PlayAnimation(_deathId);
    }
    public void EndDeath()
    {
        Player player = _owner.GetComponent<Player>();
        if (!player)
        {
            
        }
        player.RemoveLife();//probably move into health component or something?

        FightManager fightManager = GameManager.m_Instance.GetFightManager();
        if (!fightManager)
        {
            return;
        }
        fightManager.StartRespawnDelay(player);
        Destroy(gameObject);
    }
    private void OnDrawGizmos() 
    {
        if (drawDebugAttacks == true)
        {
            OnDrawAttacks();
        }    
    }
    public virtual void OnDrawAttacks() 
    {
        if (!_currentAttack.isAttackActive)
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
                _currentAttack.attackEnd = _currentAttack.origin.position + (_currentAttack.origin.forward * _currentAttack.rangeLength);
                Gizmos.DrawWireSphere(_currentAttack.origin.position, _currentAttack.radius);//start point of capsule collider
                Gizmos.DrawWireSphere(_currentAttack.attackEnd, _currentAttack.radius);      //end point of capsule collider
                break;
            case EAttackShapeType.Box:
                Debug.Log("Still need to program this one in case it gets used");
                break;
        }
    }
}
