using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct AttackInfo 
{
    public Transform origin; //get postition off of this
    /*[SerializeField]*/ public Vector3 attackEnd;//if length needed (capsule collider)
    public float radius; //capsules and spheres
    public float rangeLength; //for capsules
    /*[SerializeField]*/ private Quaternion _attackDirection; //if dir needed (capsule & box colliders)

    public float damageDealt;
}


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(DamageColliderComponent))]
public class CharacterBase : MonoBehaviour, IAttackInterface
{
    private DamageColliderComponent _damageColliderComponent;
    PlayerController _playerController;
    Animator _animator;
    private static readonly int _speedId = Animator.StringToHash("Speed");
    protected static readonly int _attack1Id = Animator.StringToHash("Attack1");
    protected static readonly int _attack2Id = Animator.StringToHash("Attack2");

    protected static readonly int _deadId = Animator.StringToHash("Dead");

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

    private bool _canAttack = true;
    [SerializeField] private bool drawDebugAttacks = false;

    GameObject _owner;
    public void SetOwner(GameObject owner) { _owner = owner; } //player will pass this in on spawn
    public DamageColliderComponent GetDamageColliderComponent() { return _damageColliderComponent; }
    public void SetFaceDirection(Vector3 directionToSet) { _faceDirection = directionToSet; }
    public float GetMaxSpeed() { return maxSpeed; }
    public float GetJumpHeight() {  return jumpHeight; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _damageColliderComponent = GetComponent<DamageColliderComponent>();
        _prevPosition = transform.position;

        if (_faceDirection == new Vector3(0, 0, 0)) 
        {
            _faceDirection = new Vector3(90, 0, 0);
        }
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
    }

    //Different Attack inputs (will be overridden in child classes)
    public void StartAttack1() 
    {
        PlayAnimation(_attack1Id);
    }
    public void StartAttack2() 
    {
        PlayAnimation(_attack2Id);
    }

    public virtual void Attack1()//attacks will be overridden to deal damage in child class. Called in Animation Events
    {
        Debug.Log($"Attack1 '{_attack1Id}' Called");
    }
    public virtual void Attack2()
    {
        Debug.Log($"Attack2 '{_attack2Id}' Called");
    }
    protected void PlayAnimation(int animationId) 
    {
        if (_animator && _canAttack == true)
        {
            _canAttack = false;
            _currentAttackId = animationId;
            _animator.SetTrigger(animationId);
        }
    }
    public void ResetAttack() 
    {
        _canAttack = true;
    }
    private void StartDeath() 
    {
        PlayAnimation(_deadId);
    }
    public void EndDeath()
    {
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
        
    }
    
}
