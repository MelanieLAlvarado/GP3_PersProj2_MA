using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class CharacterBase : MonoBehaviour
{
    Animator _animator;
    private static readonly int _attack1Id = Animator.StringToHash("Attack1");
    private static readonly int _attack2Id = Animator.StringToHash("Attack2");

    private static readonly int _deadId = Animator.StringToHash("Dead");

    [SerializeField] private Transform[] _attackColliderLocations;

    GameObject _owner;
    public void SetOwner(GameObject owner) { _owner = owner; } //player will pass this in on spawn
    
    //Different Attack inputs (will be overridden in child classes)
    public virtual void Attack1() 
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
    public void EndDeath() //used as an event in death animation 
    {
        Destroy(gameObject);
    }
}
