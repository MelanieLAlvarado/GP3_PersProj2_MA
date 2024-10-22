using System;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.GridLayoutGroup;

public class PlayerController : MonoBehaviour
{
    private CharacterBase _characterBase;
    private PlayerInputActions _playerInputActions;
    private CharacterController _characterController;

    private float _currentSpeed;
    private float _moveInput; //change to a Vector2 later???
    private float _moveSpeed = 3f;//will probably have it changed in CharacterChild class
    private float _jumpHeight = 3f;

    private Vector3 _prevPosition;
    private Vector3 _playerVelocity;
    private bool _bIsGrounded;
    private float _gravity = -9.81f;
    public float GetCurrentSpeed() { return _currentSpeed; }
    public void SetCharacter(CharacterBase characterBase) 
    {
        _characterBase = characterBase;
        _characterController = _characterBase.GetComponent<CharacterController>();
        _moveSpeed = characterBase.GetMaxSpeed();
        _moveSpeed = characterBase.GetJumpHeight();
    }
    private void Start()
    {
        _playerInputActions = new PlayerInputActions();
        //_playerInputActions = GetComponent<PlayerInput>().actions;
        _playerInputActions.Enable();
    }
    private void Update()
    {
        if (!_characterController) { return; }

        _bIsGrounded = _characterController.isGrounded;
    }
    private void FixedUpdate()
    {
        //Vector3 moveDirection = GameManager.m_Instance.GetMainCamera().InputToWorldDir();
        ProcessMovement();
    }
    private void ProcessMovement() 
    {
        if (!_characterController) { return; }

        Vector2 rawInput = _playerInputActions.Player.Move.ReadValue<Vector2>(); //will change later to be more optimized
        
        Vector3 movementVal = new Vector3(/*_moveInput.x*/ rawInput.x, 0, 0);
        Vector3 moveInDir= transform.TransformDirection(movementVal);
        if (moveInDir.normalized.x != 0)
        { 
            _characterBase.SetFaceDirection(moveInDir.normalized);
        }

        //Debug.Log($"process movement: {moveInDir * (_moveSpeed * Time.deltaTime)}");

        _characterController.Move(moveInDir * (_moveSpeed * Time.deltaTime));

        ProcessGravity();
    }

    private void ProcessGravity()
    {
        _playerVelocity.y += Time.deltaTime * _gravity;
        if (_bIsGrounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = -2f;
        }
        _characterController.Move(Time.deltaTime * _playerVelocity);
    }
    public void JumpAction(InputAction.CallbackContext context) 
    {
        if (context.started && _bIsGrounded) 
        {
            _playerVelocity.y = Mathf.Sqrt(_jumpHeight * -3.0f * _gravity);
        }
    }
    public void Attack1Action(InputAction.CallbackContext context)
    {
        if (context.started && _characterBase) 
        {
            _characterBase.gameObject.GetComponent<IAttackInterface>().StartAttack1();
        }
    }
    public void Attack2Action(InputAction.CallbackContext context)
    {
        if (context.started && _characterBase)
        {//may need to change _characterBase to the gameobject of the player when creating classes
            _characterBase.gameObject.GetComponent<IAttackInterface>().StartAttack2();
        }
    }
}
