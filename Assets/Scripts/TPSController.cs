using UnityEngine;
using UnityEngine.InputSystem;

public class TPSController : MonoBehaviour
{
    //Campos movimiento:
    private CharacterController _characterController;

    private InputAction _moveAction;
    private Vector2 _moveInput;
    [SerializeField] private float _movementSpeed = 10;

    //Campos gravedad:
    private float _gravity;
    [SerializeField]private Vector3 _playerGravity;

    [SerializeField]private Transform _sensorTransform;
    [SerializeField]private float _sensorRadius;
    [SerializeField]private LayerMask _groundLayer;

    //Campos saltar:
    [SerializeField]private float _jumpHeight = 2;

    private InputAction _jumpAction;

    //Campo camara:
    private Transform _cameraTransform;
    [SerializeField] private Transform _lookAtCamera;
    private float _xRotation;
    [SerializeField]private float _cameraSensitivity = 10;

    //Campo disparar:
    private InputAction _aimAction;

    //Campo raton:
    private InputAction _lookAction;
    private Vector2 _lookInput;


    void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        _moveAction = InputSystem.actions["Move"];
        _jumpAction = InputSystem.actions["Jump"];
        _aimAction = InputSystem.actions["Aim"];
        _lookAction = InputSystem.actions["Look"];

        _cameraTransform = Camera.main.transform;
    }

    void Start()
    {
        _gravity = Physics.gravity.y;
    }

    // Update is called once per frame
    void Update()
    {
        _moveInput = _moveAction.ReadValue<Vector2>();
        _lookInput = _lookAction.ReadValue<Vector2>();

        Gravity();

        if(_jumpAction.WasPressedThisFrame() && IsGrounded())
        {
            Jump();
        }

        TPSMovement();
    }

        void Gravity()
    {
        if(!IsGrounded())
        {
            _playerGravity.y += _gravity * Time.deltaTime;
        }

        else if(IsGrounded() && _playerGravity.y < 0)
        {
            _playerGravity.y = _gravity;
        }

        _characterController.Move(_playerGravity * Time.deltaTime);
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(_sensorTransform.position, _sensorRadius, _groundLayer);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_sensorTransform.position, _sensorRadius);
    }

    void Jump()
    {
        _playerGravity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity);
    }

    void TPSMovement()
    {
        Vector3 direction = new Vector3(_moveInput.x, 0, _moveInput.y);

        float MouseX = _lookInput.x * _cameraSensitivity * Time.deltaTime;
        float MouseY = _lookInput.x * _cameraSensitivity * Time.deltaTime;

        _xRotation -= MouseY;
        _xRotation = Mathf.Clamp(_xRotation, -89, -89);

        transform.Rotate(Vector3.up, MouseX);
        _lookAtCamera.localRotation = Quaternion.Euler(_xRotation, 0, 0);

        if(direction != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y;
            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            _characterController.Move(moveDirection* _movementSpeed * Time.deltaTime);
        }
    }
}
