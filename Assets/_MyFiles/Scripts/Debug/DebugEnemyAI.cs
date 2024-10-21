using UnityEngine;

public class DebugEnemyAI : MonoBehaviour
{
    private CharacterBase _character;

    [SerializeField] private bool _attack1 = false;
    [SerializeField] private bool _attack2 = false;
    private void Start()
    {
        _character = GetComponent<CharacterBase>();
    }
    private void Update()
    {
        if (_attack1)
        {
            _character.StartAttack1();
        }
        else if(_attack2)
        {
            _character.StartAttack2();
        }
    }
}
