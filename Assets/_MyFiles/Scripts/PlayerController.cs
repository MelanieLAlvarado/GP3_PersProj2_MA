using System;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.GridLayoutGroup;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    public delegate void TriggerPauseDelegate();
    public event TriggerPauseDelegate OnPauseTriggered;

    PlayerInput _playerInput;
    protected static string _keyboardFullScheme = "KeyboardFull";
    protected static string _keyboardLeftScheme = "KeyboardLeft";
    protected static string _keyboardRightScheme = "KeyboardRight";
    protected static string _controllerScheme = "Controller";

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
    public void SetControlledCharacter(CharacterBase characterBase) 
    {
        _characterBase = characterBase;
        _characterController = _characterBase.GetComponent<CharacterController>();
        _moveSpeed = characterBase.GetMaxSpeed();
        _moveSpeed = characterBase.GetJumpHeight();
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
        //Vector3 moveDirection = GameManager.m_Instance.GetMainCamera().InputToWorldDir();
        ProcessMovement();
    }
    private void ProcessMovement() 
    {
        if (!_characterController) { return; }
        //Debug This (Won't swap control scheme, is checking all schemes)
        /*if (_playerInput.currentControlScheme == _keyboardRightScheme)
        { 
            
        }*/

        Vector2 rawInput = _playerInputActions.Player.Move.ReadValue<Vector2>(); //Debug; will read all schemes!!

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
    public void PauseAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            FightManager fightManager = GameManager.m_Instance.GetFightManager();
            if (fightManager)
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
        {//may need to change _characterBase to the gameobject of the player when creating classes
            _characterBase.gameObject.GetComponent<IAttackInterface>().StartAttack2();
        }
    }
}
