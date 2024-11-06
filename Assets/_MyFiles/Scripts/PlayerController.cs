using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.GridLayoutGroup;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    public delegate void OnTriggerPauseDelegate();
    public event OnTriggerPauseDelegate OnPauseTriggered;

    PlayerInput _playerInput;
    protected static string _keyboardFullScheme = "KeyboardFull";
    protected static string _keyboardLeftScheme = "KeyboardLeft";
    protected static string _keyboardRightScheme = "KeyboardRight";
    protected static string _controllerScheme = "Controller";

    private CharacterBase _characterBase;
    private PlayerInputActions _playerInputActions;
    private CharacterController _characterController;

    Vector2 _rawInput;
    private float _moveSpeed = 3f;//will probably have it changed in CharacterChild class
    private float _jumpHeight = 3f;

    private Vector3 _playerVelocity;
    private bool _bIsGrounded;
    private float _gravity = -9.81f;

    public void SetControlledCharacter(CharacterBase characterBase) 
    {
        _characterBase = characterBase;
        _characterController = _characterBase.GetComponent<CharacterController>();
        _moveSpeed = characterBase.GetMaxSpeed();
        _jumpHeight = characterBase.GetJumpHeight();
    }
    public float GetGravity() { return _gravity; }
    public void ClearController() { _characterController = null; }
    public void DisablePlayerInputActions() 
    { 
        _playerInputActions.Disable(); 
    }
    public void LaunchCharacter(Vector3 velocityToSet) 
    {
        _playerVelocity = velocityToSet;
        StartCoroutine(ResetVelocity());
    }
    private IEnumerator ResetVelocity() 
    {
        yield return new WaitForSeconds(1f);
        _playerVelocity = Vector3.zero;
    }

    private void Start()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();
        _playerInput = GetComponent<PlayerInput>();
    }
    private void Update()
    {
        if (!_characterController) { return; }

        _bIsGrounded = _characterController.isGrounded;
    }
    private void FixedUpdate()
    {
        ProcessMovement();
    }
    public void MovementAction(InputAction.CallbackContext context)
    {
        if (context.started || context.canceled)
        { 
            _rawInput = context.ReadValue<Vector2>();
        }
    }
    public void ProcessMovement() 
    {
        if (!_characterController) { return; }

        Vector3 movementVal = new Vector3(_rawInput.x, 0, 0);
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
        if (!_characterController) { return; }

        _playerVelocity.y += Time.deltaTime * _gravity;
        if (_bIsGrounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = -2f;
        }
        _characterController.Move(Time.deltaTime * _playerVelocity);
    }

    public void JumpAction(InputAction.CallbackContext context) 
    {
        if (context.started && _bIsGrounded && _characterBase) 
        {
            /*_characterBase.SetGravity(_gravity);
            _characterBase.CharacterJump();*/
            _playerVelocity.y = Mathf.Sqrt(_jumpHeight * -3.0f * _gravity);
        }
    }
    public void PauseAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            FightManager fightManager = GameManager.m_Instance.GetFightManager();
            if (fightManager && fightManager.GetIsFightActive())
            { 
                OnPauseTriggered?.Invoke();
            }
        }
    }
    public void TriggerHalfKeyboardAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //determine when to change the schen through a bool
            GameManager.m_Instance.ProcessKeyboardPlayers(_playerInput, _keyboardLeftScheme, _keyboardRightScheme);
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
        {
            _characterBase.gameObject.GetComponent<IAttackInterface>().StartAttack2();
        }
    }
}
