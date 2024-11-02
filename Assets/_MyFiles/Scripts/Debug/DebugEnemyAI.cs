using UnityEngine;

public class DebugEnemyAI : MonoBehaviour
{
    private IAttackInterface _attackInterface;

    [SerializeField] private bool bAttack1 = false;
    [SerializeField] private bool bAttack2 = false;
    private void Start()
    {
        _attackInterface = GetComponent<IAttackInterface>();
    }
    private void Update()
    {
        if (bAttack1)
        {
            _attackInterface.StartAttack1();
        }
        else if(bAttack2)
        {
            _attackInterface.StartAttack2();
        }
    }
}
