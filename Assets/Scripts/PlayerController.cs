using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Campos movimiento:
    private CharacterController _characterController;

    private InputAction _moveAction;
    private Vector2 _moveInput;
    [SerializeField] private float _movementSpeed = 10;

    private float _turnSmoothVelocity;
    [SerializeField]float _smoothTime = 1;

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

    //Campo disparar:
    private InputAction _aimAction;

    //Campo animacion:
    private Animator _animator;


    void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        _moveAction = InputSystem.actions["Move"];
        _jumpAction = InputSystem.actions["Jump"];
        _aimAction = InputSystem.actions["Aim"];

        _cameraTransform = Camera.main.transform;

        /*Get Component: Busca componentes que esten en el inspector del personaje que tiene el script asignado.
        Get Component InChildren (buscar dentro de los hijos): busca dentro del objeto (o lo que sea) que tenga el script asignado.
        Ej: En el inspector, player (Get Component) tiene un objeto dentro que se llama Cube (Get Component InChildren).*/
        _animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        _gravity = Physics.gravity.y;

        _cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        _moveInput = _moveAction.ReadValue<Vector2>();

        Gravity();

        if(_jumpAction.WasPressedThisFrame() && IsGrounded())
        {
            Jump();
        }

        _animator.SetBool("IsJumping", !IsGrounded());

        if(_aimAction.IsPressed())
        {
            AimMovement();
        }

        /*else
        {
            TPMovement();
        }*/

        AimMovement();
    }

    //Primer tipo de movimiento
    /*void TopDownMovement()
    {
        Vector3 moveDirection = new Vector3(_moveInput.x, 0, _moveInput.y);

        if(moveDirection != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _smoothTime);

            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

            _characterController.Move(moveDirection * _movementSpeed * Time.deltaTime);
        }
    }*/


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

    //Segundo tipo de movimiento
    void TPMovement()
    {
        Vector3 direction = new Vector3(_moveInput.x, 0, _moveInput.y);

        _animator.SetFloat("Vertical", direction.magnitude, 0.2f, Time.deltaTime);
        _animator.SetFloat("Horizontal", 0);

        if(direction != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _smoothTime);

            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            _characterController.Move(moveDirection * _movementSpeed * Time.deltaTime);
        }
    }

    //Tercer tipo de movimiento
    void AimMovement()
    {
        Vector3 direction = new Vector3(_moveInput.x, 0, _moveInput.y);

        _animator.SetFloat("Horizontal", _moveInput.x, 0.2f, Time.deltaTime);
        _animator.SetFloat("Vertical", _moveInput.y, 0.2f, Time.deltaTime);

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y;
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _cameraTransform.eulerAngles.y, ref _turnSmoothVelocity, _smoothTime);

        transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

        if(direction != Vector3.zero)
        {
            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            _characterController.Move(moveDirection * _movementSpeed * Time.deltaTime);
        }
    }
}
