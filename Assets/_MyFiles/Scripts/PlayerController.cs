using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    public delegate void OnTriggerPauseDelegate();
    public event OnTriggerPauseDelegate OnPauseTriggered;

    PlayerInput _playerInput;
    protected static string _keyboardLeftScheme = "KeyboardLeft";
    protected static string _keyboardRightScheme = "KeyboardRight";

    private CharacterBase _characterBase;
    private PlayerInputActions _playerInputActions;
    private CharacterController _characterController;

    bool _bCanMove = true;
    Vector2 _rawInput;

    private float _moveSpeed = 3f;  //changed by Character
    private float _jumpHeight = 3f; //changed by Character

    private bool _bHasJumped = false;

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
    public bool GetIsGrounded() { return _bIsGrounded; }
    public bool GetHasJumped() { return _bHasJumped; }
    public void ClearController() { _characterController = null; }
    public void DisablePlayerInputActions() 
    { 
        _playerInputActions.Disable(); 
    }
    public void ResetPlayerVelocity() { _playerVelocity = Vector3.zero; }
    public void LaunchCharacter(Vector3 velocityToSet) 
    {
        _playerVelocity = velocityToSet;
    }
    public void ResetVelocityTimer()
    {
        StartCoroutine(ResetVelocityTimerCoroutine(1.5f));
    }
    private IEnumerator ResetVelocityTimerCoroutine(float time) 
    {
        yield return new WaitForSeconds(time);
        ResetPlayerVelocity();
    }
    public void SetCanMove(bool bCanMove) { _bCanMove = bCanMove; }

    private void Start()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();
        _playerInput = GetComponent<PlayerInput>();
        _bCanMove = true;
    }
    private void Update()
    {
        if (!_characterController) { return; }

        _bIsGrounded = _characterController.isGrounded;
    }
    private void FixedUpdate()
    {
        ProcessMovement();
        ProcessGravity();
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
        if (!_characterController || !_bCanMove) { return; }

        Vector3 movementVal = new Vector3(_rawInput.x, 0, 0);
        Vector3 moveInDir= transform.TransformDirection(movementVal);
        if (moveInDir.normalized.x != 0)
        { 
            _characterBase.SetFaceDirection(moveInDir.normalized);
        }
        _characterController.Move(moveInDir * (_moveSpeed * Time.deltaTime));
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
        if (context.started && _bIsGrounded && _characterBase && _bCanMove) 
        {
            _bHasJumped = true;
            _playerVelocity.y = Mathf.Sqrt(_jumpHeight * -3.0f * _gravity);
            StartCoroutine(HasJumpedResetDelay());//for animator
        }
    }

    private IEnumerator HasJumpedResetDelay()
    {
        yield return new WaitForSeconds(0.5f);
        _bHasJumped = false;
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
            //determine when to change the keyboard scheme through a bool on DataManager
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
    public void Attack3Action(InputAction.CallbackContext context)
    {
        if (context.started && _characterBase)
        {
            _characterBase.gameObject.GetComponent<IAttackInterface>().StartAttack3();
        }
    }
}
