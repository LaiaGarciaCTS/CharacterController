using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class RayCast : MonoBehaviour
{
    //Campo Click raton;
    private InputAction _clickAction;

    //Campos movimiento:
    //private CharacterController _characterController;
    private NavMeshAgent _agent;

    private InputAction _moveAction;
    private Vector2 _moveInput;
    [SerializeField] private float _movementSpeed = 10;

    //Campos gravedad:
    private float _gravity;
    [SerializeField]private Vector3 _playerGravity;

    [SerializeField]private Transform _sensorTransform;
    [SerializeField]private float _sensorRadius;
    [SerializeField]private LayerMask _groundLayer;

    //Campo para ver donde esta el raton:
    private InputAction _lookInput;
    private Vector2 _mousePosition;


    void Awake()
    {
        _clickAction = InputSystem.actions["Attack"];
        _moveAction = InputSystem.actions["Move"];
        _lookInput = InputSystem.actions["MousePosition"];

        //_characterController = GetComponent<CharacterController>();

        _agent = GetComponent<NavMeshAgent>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _gravity = Physics.gravity.y;
    }

    // Update is called once per frame
    void Update()
    {
        _moveInput = _moveAction.ReadValue<Vector2>();

        //TopDownMovement();

        /*Debug.DrawRay(transform.position, transform.forward * 10, color.red);

        if(_clickAction.WasPressedThisFrame())
        {
            ShotRayCast();
        }*/

        Gravity();

        if(_clickAction.WasPressedThisFrame())
        {
            MoveToClickPoint();
        }
    }

    void ShotRayCast()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, 10))
        {
            Debug.Log(hit.transform.name);
            Debug.Log(hit.transform.gameObject.layer);
            Debug.Log(hit.transform.tag);
            Destroy(hit.transform.gameObject);
        }
    }

    /*void TopDownMovement()
    {
        Vector3 moveDirection = new Vector3(_moveInput.x, 0, _moveInput.y);
        _mousePosition = _lookInput.ReadValue<Vector2>();

        Ray ray = Camera.main.ScreenPointToRay(_mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector3 playerForward = hit.point - transform.position;
            playerForward.y = 0;
            transform.forward = playerForward;
        }

        if(moveDirection != Vector3.zero)
        {
            _characterController.Move(moveDirection * _movementSpeed * Time.deltaTime);
        }
    }*/

    void MoveToClickPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(_mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            _agent.SetDestination(hit.point);
        }
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

        //_characterController.Move(_playerGravity * Time.deltaTime);
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(_sensorTransform.position, _sensorRadius, _groundLayer);
    }
}
