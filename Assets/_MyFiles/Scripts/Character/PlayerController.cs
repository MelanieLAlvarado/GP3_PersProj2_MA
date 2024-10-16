using System;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private GameObject _playerOwner;
    private PlayerInputActions _playerInputActions;

    private CharacterController _characterController;
    private float _moveInput; //change to a Vector2 later???
    [SerializeField] private float _moveSpeed = 3f;//will probably have it changed in CharacterChild class
    [SerializeField] private float _jumpHeight = 3f;

    private Vector3 _playerVelocity;
    private bool _isGrounded;
    private float _gravity = -9.81f;
    public void SetPlayerOwner(GameObject player) 
    {
        _playerOwner = player;
    }
    public void SetCharacterController(CharacterController charCtrl) { _characterController = charCtrl; }
    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();
    }
    private void Update()
    {
        if (!_characterController) { return; }

        _isGrounded = _characterController.isGrounded;
    }
    private void FixedUpdate()
    {
        //Vector3 moveDirection = GameManager.m_Instance.GetMainCamera().InputToWorldDir();
        ProcessMovement();
    }
    private void ProcessMovement() 
    {
        if (!_characterController) { return; }

        _moveInput = Input.GetAxisRaw("Horizontal"); //will change later to be more optimized
        Vector3 movementVal = new Vector3(/*_moveInput.x*/ _moveInput, 0, 0);
        Vector3 moveInDir= transform.TransformDirection(movementVal);
        _characterController.Move(moveInDir * (_moveSpeed * Time.deltaTime));

        ProcessGravity();
    }

    private void ProcessGravity()
    {
        _playerVelocity.y += Time.deltaTime * _gravity;
        if (_isGrounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = -2f;
        }
        _characterController.Move(Time.deltaTime * _playerVelocity);
    }
    public void Jump(InputAction.CallbackContext context) 
    {
        if (_isGrounded) 
        {
            _playerVelocity.y = Mathf.Sqrt(_jumpHeight * -3.0f * _gravity);
        }
    }
}
