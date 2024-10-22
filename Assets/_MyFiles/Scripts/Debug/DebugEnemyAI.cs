using UnityEngine;

public class DebugEnemyAI : MonoBehaviour
{
    private IAttackInterface _attackInterface;

    [SerializeField] private bool _bAttack1 = false;
    [SerializeField] private bool _bAttack2 = false;
    private void Start()
    {
        _attackInterface = GetComponent<IAttackInterface>();
    }
    private void Update()
    {
        if (_bAttack1)
        {
            _attackInterface.StartAttack1();
        }
        else if(_bAttack2)
        {
            _attackInterface.StartAttack2();
        }
    }
}
