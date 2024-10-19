using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class CharacterBase : MonoBehaviour
{
    PlayerController _playerController;
    Animator _animator;
    private static readonly int _speedId = Animator.StringToHash("Speed");
    private static readonly int _attack1Id = Animator.StringToHash("Attack1");
    private static readonly int _attack2Id = Animator.StringToHash("Attack2");

    private static readonly int _deadId = Animator.StringToHash("Dead");

    private Vector3 _prevPosition;
    private Vector3 _faceDirection;

    [SerializeField] private float characterTurnSpeed = 3f;
    [SerializeField] private float animSpeedChangeRate = 0.4f;
    private float _animMoveSpeed = 0f;
    private float _currentSpeed = 0f;
    [SerializeField] private float maxSpeed = 3f;//will probably have it changed in CharacterChild class
    [SerializeField] private float jumpHeight = 3f;

    private bool _canAttack = true;
    [SerializeField] private Transform[] _attackColliderLocations;

    GameObject _owner;
    public void SetOwner(GameObject owner) { _owner = owner; } //player will pass this in on spawn
    public void SetFaceDirection(Vector3 directionToSet) { _faceDirection = directionToSet; }
    public float GetMaxSpeed() { return maxSpeed; }
    public float GetJumpHeight() {  return jumpHeight; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        Debug.Log($"animator is: {_animator}");
        _prevPosition = transform.position;
    }
    private void FixedUpdate()
    {
        Vector3 curMove = transform.position - _prevPosition;
        _currentSpeed = curMove.magnitude / Time.deltaTime;
        _prevPosition = transform.position;

        //Rotating whole character based on movement direction
        if (_currentSpeed != 0)
        {
            Quaternion goalRot = Quaternion.LookRotation(_faceDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, goalRot, Time.deltaTime * characterTurnSpeed);
        }
        
        //updating animating speed through a lerped value
        if (_animator)
        {
            _animMoveSpeed = Mathf.Lerp(_animMoveSpeed, _currentSpeed, Time.deltaTime * animSpeedChangeRate);
            _animator.SetFloat(_speedId, _animMoveSpeed);
        }
    }

    //Different Attack inputs (will be overridden in child classes)
    public virtual void Attack1() //attacks will be overridden to deal damage in child class
    {
        Debug.Log($"Attack1 '{_attack1Id}' Called");
        PlayAnimation(_attack1Id);
    }
    public virtual void Attack2()
    {
        Debug.Log($"Attack2 '{_attack2Id}' Called");
        PlayAnimation(_attack2Id);
    }
    protected void PlayAnimation(int animationId) 
    {
        //Debug.Log($"anim param: {_animator.GetParameter(animationId)}");
        if (_animator && _canAttack == true)
        {
            _canAttack = false;
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
}
