using UnityEngine;

[RequireComponent(typeof(LaunchComponent))]
public class MutantCharacter : CharacterBase
{
    [Header("Mutant Jump Options")]
    [SerializeField] private float jumpVelocity = 2f;
    LaunchComponent _launchComponent;
    public void AttackJump() 
    {
        if (!_launchComponent)
        { 
            _launchComponent = GetComponent<LaunchComponent>();
        }
        if (_launchComponent)
        {
            _launchComponent.Launch(transform.forward, jumpVelocity, true);
        }
    }
    public void EndAttackJump() 
    {
        _ownerController.ResetPlayerVelocity();
    }
}
