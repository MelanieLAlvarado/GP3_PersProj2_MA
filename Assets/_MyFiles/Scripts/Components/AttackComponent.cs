using UnityEngine;

public class AttackComponent : MonoBehaviour
{
    private DamageColliderComponent _damageColliderComponent;

    Animator _animator;
    AttackInfo _currentAttack;
    protected int _currentAttackId;
    private bool _bCanAttack = true;

    [SerializeField] private bool bDrawDebugAttacks = false;

    private void Awake()
    {
        _damageColliderComponent = GetComponent<DamageColliderComponent>();
        _animator = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        if (_currentAttack.bIsAttackActive == true)
        {
            _damageColliderComponent.ProcessAttackType(_currentAttack);
        }
    }

    public void Attack() //attack physics will spawn (in Animation Events)
    {
        _currentAttack.bIsAttackActive = true;
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
        _damageColliderComponent.ClearHitTargets();
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
