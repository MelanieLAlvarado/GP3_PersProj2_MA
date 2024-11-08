using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Android.Gradle.Manifest;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(HealthComponent))]
[RequireComponent(typeof(AttackComponent))]
public abstract class CharacterBase : MonoBehaviour
{
    private AttackComponent _attackComponent;
    private HealthComponent _healthComponent;

    Animator _animator;
    private static readonly int _speedId = Animator.StringToHash("Speed");

    protected static readonly int _hitId = Animator.StringToHash("Hit");
    protected static readonly int _hasJumpedId = Animator.StringToHash("HasJumped");
    protected static readonly int _isGroundedId = Animator.StringToHash("IsGrounded");
    protected static readonly int _deathId = Animator.StringToHash("Death");
    protected bool _bIsDead = false;

    [Header("Base Character Options")]
    private Vector3 _prevPosition;
    private Vector3 _faceDirection;

    [SerializeField] private float characterTurnSpeed = 10f;
    [SerializeField] private float animSpeedChangeRate = 0.4f;
    private float _animMoveSpeed = 0f;
    private float _currentSpeed = 0f;
    [SerializeField] private float maxSpeed = 3f;
    [SerializeField] private float jumpHeight = 3f;

    Player _ownerPlayer;
    protected PlayerController _ownerController;
    public Player GetOwnerPlayer() { return _ownerPlayer; }
    public void SetOwnerPlayer(Player owner) { _ownerPlayer = owner; } //player will pass this in on spawn
    public PlayerController GetOwnerController() { return _ownerController;}
    public void SetFaceDirection(Vector3 directionToSet) { _faceDirection = directionToSet; }
    public float GetMaxSpeed() { return maxSpeed; }
    public float GetJumpHeight() {  return jumpHeight; }

    private void Awake()
    {
        _healthComponent = GetComponent<HealthComponent>();
        _healthComponent.OnDead += StartDeath;
        _animator = GetComponent<Animator>();
        _attackComponent = GetComponent<AttackComponent>();
        _prevPosition = transform.position;

        if (_faceDirection == new Vector3(0, 0, 0)) 
        {
            _faceDirection = new Vector3(90, 0, 0);
        }
    }
    private void Start()
    {
        FightManager fightManager = GameManager.m_Instance.GetFightManager();
        if (fightManager != null)
        {
            fightManager.GetBattleCam().AddToFollowObjects(this.gameObject);
        }

        if (_ownerPlayer)
        {
            _ownerController = _ownerPlayer.GetComponent<PlayerController>();
            _healthComponent.OnDead += _ownerController.ClearController;
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
            if (!_ownerController)
                _ownerController = _ownerPlayer.GetComponent<PlayerController>();
            _animator.SetBool(_isGroundedId, _ownerController.GetIsGrounded());
            _animator.SetBool(_hasJumpedId, _ownerController.GetHasJumped());
        }
    }

    public void HitReaction() 
    {
        _animator.SetTrigger(_hitId);
        _ownerController.ResetVelocityTimer();
    }

    private void StartDeath() 
    {
        _bIsDead = true;
        _animator.SetTrigger(_deathId);
    }
    public void EndDeath() //triggered in animation events
    {
        Player player = _ownerPlayer.GetComponent<Player>();
        if (!player)
        {
            return;
        }
        player.RemoveLife();

        FightManager fightManager = GameManager.m_Instance.GetFightManager();
        if (!fightManager)
        {
            return;
        }
        fightManager.StartRespawnDelay(player);
        fightManager.GetBattleCam().RemoveFromFollowObjects(this.gameObject);

        Destroy(gameObject);
    }
}
