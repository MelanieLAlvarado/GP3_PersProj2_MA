using UnityEngine;

public class LaunchComponent : MonoBehaviour
{
    Rigidbody _rigidBody;
    PlayerController _playerController;
    public void Launch(Vector3 launchDirection, float launchVelocity, bool bFlattenZ = false, bool bFlattenY = false) 
    {
        //Debug.Log(launchDirection);
        Vector3 finalVelocity = launchDirection * launchVelocity;
        if (bFlattenZ)
        {
            finalVelocity.z = 0;
        }
        if (bFlattenY)
        {
            finalVelocity.y = 0;
        }

        if (CanLaunchRigidBody()) //checks if a rigidbody is detected
        {
            Debug.Log($"can launch rigidbody. Final Velocity: {finalVelocity}");
            _rigidBody.AddForce(finalVelocity, ForceMode.Impulse);
            return;
        }

        if (CanLaunchCharacter()) //checks if a CharacterBase is detected
        {
            finalVelocity.x = launchVelocity/2 * finalVelocity.x;
            finalVelocity.y = Mathf.Sqrt(launchVelocity/2 * -3.0f * _playerController.GetGravity());
            _playerController.LaunchCharacter(finalVelocity);
        }
    }

    private bool CanLaunchCharacter()
    {
        if (_playerController)
        {
            return true;
        }

        CharacterBase characterBase = GetComponent<CharacterBase>();
        if (!characterBase)
        {
            return false;
        }
        _playerController = characterBase.GetOwnerPlayer().GetComponent<PlayerController>();
        return true;
    }

    private bool CanLaunchRigidBody()
    {
        if (_rigidBody)
        {
            return true;
        }
        
        _rigidBody = GetComponent<Rigidbody>();
        if (!_rigidBody)
        {
            return false;
        }
        SetUpRigidBody();
        return true;
    }

    private void SetUpRigidBody()
    {
        _rigidBody.useGravity = true;
        _rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
    }
}
