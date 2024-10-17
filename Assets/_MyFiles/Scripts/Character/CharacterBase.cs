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
    [SerializeField] private float speed = 3f;//will probably have it changed in CharacterChild class
    [SerializeField] private float jumpHeight = 3f;

    [SerializeField] private Transform[] _attackColliderLocations;

    GameObject _owner;
    public void SetOwner(GameObject owner) { _owner = owner; } //player will pass this in on spawn
    public float GetSpeed() { return speed; }
    public float GetJumpHeight() {  return jumpHeight; }

    //Different Attack inputs (will be overridden in child classes)
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        _prevPosition = _owner.transform.position;
        float currentSpeed = (_owner.transform.position - _prevPosition).magnitude / Time.deltaTime;
        _animator.SetFloat(_speedId, currentSpeed);
        Debug.Log(currentSpeed);
    }
    public virtual void Attack1() //attacks will be overridden to deal damage in child class
    {
        PlayAnimation(_attack1Id);
    }
    public virtual void Attack2()
    {
        PlayAnimation(_attack2Id);
    }
    protected void PlayAnimation(int animationId) 
    {
        if (_animator && _animator.GetParameter(animationId) != null)
        {
            _animator.SetTrigger(animationId);
        }
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
