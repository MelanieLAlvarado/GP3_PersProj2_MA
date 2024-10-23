using System;
using UnityEngine;

[Serializable]
public struct AttackInfo
{
    public EAttackShapeType attackShape;
    public Transform origin; //get postition off of this
    /*[SerializeField]*/
    public Vector3 attackEnd;//attack end
    public float radius; //capsules and spheres
    public float rangeLength; //for capsules
    /*[SerializeField]*/
    private Quaternion _attackDirection; //if dir needed (capsule & box colliders)

    public float damageDealt;
    public bool bIsAttackActive;
}

public class AttackComponent : MonoBehaviour, IAttackInterface
{
    private DamageColliderComponent _damageColliderComponent;

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

    //DEbug
    [Header("DEbug CAst")]
    private RaycastHit _raycastHit;
    [SerializeField] private float sphereCastRadius = 12.0f;
    Vector3 _castPos;

    private void Awake()
    {
        _damageColliderComponent = GetComponent<DamageColliderComponent>();
        _animator = GetComponent<Animator>();
    }
    private void Start()
    {
        attack1.bIsAttackActive = false;
        attack2.bIsAttackActive = false;
    }
    private void FixedUpdate()
    {
        if (_currentAttack.bIsAttackActive == true)
        {
            _damageColliderComponent.ProcessAttackType(_currentAttack);
        }
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

        //DEbug
        /*Ray ray = new Ray(_currentAttack.origin.position, _currentAttack.origin.forward);
        if (Physics.SphereCast(ray, sphereCastRadius, out _raycastHit))
        {
            GameObject sphereGameObj = _raycastHit.transform.gameObject;
            _castPos = sphereGameObj.transform.position;
            Debug.Log($"{_castPos.x}, {_castPos.y}, {_castPos.z}");
        }
        Debug.Log("CastDone");*/

        /*SphereCollider sphereCollider = this.gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        Collider collider = sphereCollider;*/

        // IN DAMAGE COLLIDER
        /* SetCollider(Collider colliderToSet)
         * private void OnTriggerEnter(Collider other)
         * {
         *      if (other.GetComponent<BaseCharacter>())
         *      {
         *          _damageColliderComponent.ProcessAttackType(_currentAttack);
         *      }
         * }*/
        /* 
            ProcessHitObjects(other);
         */
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

        //DEbug
        /*Gizmos.color = Color.red;
        
        Gizmos.DrawWireSphere(_castPos, sphereCastRadius);*/
        //Gizmos.DrawWireSphere(_castPos, sphereCastRadius);



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
