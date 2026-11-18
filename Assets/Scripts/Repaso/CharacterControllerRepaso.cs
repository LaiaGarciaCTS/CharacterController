using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterControllerRepaso : MonoBehaviour
{
    // Añadir el script al personaje

    /*Camera
    Instalar el paquete de Cinemachine
    Agregar una CameraCinemachine
    - Cinemachine - Targeted Cameras - FreeLookCamera
    - Si la camara no se mueve ir al inspector de la camara y en Cinmemachine Input Axis Controller, y poner Player Look en Look X e y
    - Si los controles estan invertidos cambiarlos en Cinemachine Input Axis Conrtoller, cambiar el Gain de positivo a negativo, también se puede cambiar la velocidad de la camara en Gain. Poner Enable en el eje x y en la y.
    - Para cambiar los parámetros de la cámara (modificar las redondas), en Cinemachine Orbital Follow cambiar 
    - Cinemachine Rotation Composer cambiar el Target Offset para cambiar el punto de mira.

    //CHARACTER CONTROLLER
    Añadir al personaje el componente de rigidbody y el de collider
    Añadir al personaje un Empty con el nombre Ground Sensor y añadir un collider
 (Puntuación máxima en el examen movimiento de AimMovement)
  
    Crear las variables
    Poner el using UnityEngine.InputSystem para poder acceder al Input System

    Crear el void Awake() para nombrar las variables

    Crear el void Update() 
    - Poner un valor a _moveInputValue
    - Llamar a la función Movement()

    Crear el void Movement() para que el jugador se mueva
    


    */
    //Variable de CharacterController
    private CharacterController _controller;

    //Variables InputAction de mover y saltar
    private InputAction _moveAction;
    private InputAction _jumpAction;

    //Variable Vector2 para la dirección
    private Vector2 _moveInputValue;

    //Variables velocidad y fuerza de salto
    private float _movementSpeed = 12;
    private float _jumpForce = 20;

    //Variables para el ground sensor
    [SerializeField]private Transform _sensorPosition;
    [SerializeField]private float _sensorRadius;
    [SerializeField]private LayerMask _groundSensor;

    //Variable para la gravedad
    private float _gravity;

    //Variables para controlar lo rápido que rotará eñ personaje
    [SerializeField]private float _smoothTime = 0.5f;
    private float _turnSmoothVelocity;

    void Awake()
    {
        _controller = GetComponent<CharacterController>();

        _jumpAction = InputSystem.actions["Jump"];
        _moveAction = InputSystem.actions ["Move"];
    }

    void Update()
    {
        _moveInputValue = _moveAction.ReadValue<Vector2>();

        Movement();
    }
    
    void Movement()
    {
        //Dirección del movimiento y hacia dónde te mueves
        Vector3 direction = new Vector3(_moveInputValue.x, 0, _moveInputValue.y);

        //Para controlar la rotación del personaje (controla la posición del Joystick)
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        //Pasar de un ángulo a otro con x tiempo, para que no sea de golpe sino que haya un movimiento suave
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _smoothTime);

        //Para que rote sobre sí mismo
        transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

        //Personaje se mueve en la dirección correspondiente con la velocidad 
        _controller.Move(direction * _movementSpeed * Time.deltaTime);
    }
}
