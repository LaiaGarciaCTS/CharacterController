using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterControllerRepaso : MonoBehaviour
{
    //Camara 1 punto
    //Animaciones 2 puntos
    //Movimiento 7 puntos


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

    Crear el void Awake()
    - Para nombrar las variables

    Crear el void Update() 
    - Poner un valor a _moveInputValue
    - Llamar a la función Movement()
    - Llamar a la función Gravity()
    - Llamar a la función Jump() solamente se ejecutará si se pulsa el botón de saltar y cuando esté en el suelo

    Crear el void Movement()
    - Para que el jugador se mueva
    - Poner Vector3 direction para poner una dirección de movimiento, que será hacia dónde te muevas
    - Para controlar la rotación del personaje y que se mueva hacia dónde mira la cámara
    - Hacer que pase de un ángulo a otro con x tiempo y que haya un movimiento suave
    - Hacer que rote sobre sí mismo
    - If para que no se mueva solo y solamente se mueva cuando toquemos alguna tecla
        - Hacer que el personaje se mueva a la dirección que está mirando
        - Hacer que el personaje se mueva en la dirección correspondiente con la velocidad (en X y en Z)
    
    Crear el void Gravity()
    - Crear 2 if's para que cuando esté en el suelo pueda saltar y cuando no esté en el suelo no pueda saltar
        - Cuando no esté en el suelo
            - Que la gravedad se sume
        - Cuando toque el suelo y la gravedad del personaje sea menor a 0
            - Poner fijo un valor de gravedad una vez que estás en el suelo (cuando no saltas o has dejado de saltar)
    - Mover el personaje hacia arriba o hacia abajo (en Y)

    Crear el void Jump()
    - Cambia el valor y hace que salte hacia arriba (porque la gravedad es negativa) y hace que salte a la altura que le pongas en el JumpForce y que no sea aleatoria
    
    Crear el bool IsGrounded()
    - Devuelve la información de si está en el suelo o no, inspeccionando si el suelo está dentro del radio del ground sensor y si el suelo está dentro de la capa de suelo


    //ANIMACIONES
    Buscar en Mixamo las animaciones y descargarlas
    - Idle - With Skin
    - Andar - Without Skin - In place
    - Andar izquierda - Without Skin - In place
    - Andar derecha - Without Skin - In place - Mirror
    - Andar hacia atrás - Without Skin - In place
    - Correr - Without Skin - In place
    - Correr izquierda - Without Skin - In place
    - Correr derecha - Without Skin - In place - Mirror
    - Correr hacia atrás - Without Skin - In place

    Crear dónde las animaciones en la parte baja de Unity una caja de animación
    Create - Animation - Animation Controller
    Añadir el Animator en la animación que tenga la Skin
    Doble click para abrir el Animator Controller
    - Create State - Blend Tree
    Poner tres parámetros:
    - Float - Vertical
    - Float - Horizontal
    - Bool - IsGrounded

    Doble click en el Blend Tree
    - Cambiar el Blend Type a 2D Freefrom Directional
    - Añadir las animaciones que nos hayamos descargado
    Seleccionar las animaciones
    - Las animaciones se llaman mixamo.com, les tenemos que cambiar el nombre con el nombre de la animación correspondiente
    - Ponerlas en Loop Time menos la animación de saltar

    Parámetros (0 = no moverse, 1 = correr (lo máximo)):
    - Idle - 0, 0
    - Andar - 0, 0.3
    - Andar izquierda - -0,3, 0
    - Andar derecha - 0.3, 0
    - Andar hacia atrás - 0, -0.3
    - Correr - 0, 1
    - Correr izquierda - -1, 0
    - Correr derecha - 0, -1
    - Correr hacia atrás - 0, -1

    Vincular los parámetros de Vertical y Horizontal al Blend Tree
    - Volvemos a la Base Layer
    - Arrastrar la animación de salto

    Hacer las transiciones
    - Transición de Blend Tree a Jump - Is Grounded - false
    - Transición de Jump a Blend Tree - IsGrounded - true
    - Quitar el Has Exit Time

    Arrastrar el Ground Sensor al script del personaje para que detecte el suelo
    Subir el radio a 0.5 del sensor radius


    Crear las variables
    - Animator

    En el void Awake()
    - Para nombrar las variables

    En el void Update()
    - Poner la animación de saltar
    
    En el void Movement()
    - Poner las animaciones de mover en X y en Y

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
    [SerializeField]private LayerMask _groundLayer;

    //Variable para la gravedad
    private float _gravity = -9.81f;
    private Vector3 _playerGravity;

    //Variables para controlar lo rápido que rotará eñ personaje
    [SerializeField]private float _smoothTime = 0.5f;
    private float _turnSmoothVelocity;

    //Variable cámara
    private Transform _cameraTransform;


    //Variables de animaciones
    private Animator _animator;


    //Función Awake (se ejecuta antes de empezar el juego)
    void Awake()
    {
        _controller = GetComponent<CharacterController>();

        _jumpAction = InputSystem.actions["Jump"];
        _moveAction = InputSystem.actions ["Move"];

        _cameraTransform = Camera.main.transform;

        _gravity = Physics.gravity.y;

        _animator = GetComponentInChildren<Animator>();
    }

    //Función Update (se ejecutará frame por frame (durante todo el tiempo))
    void Update()
    {
        _moveInputValue = _moveAction.ReadValue<Vector2>();

        //Llama a la función de Movement()
        Movement();

        //Llama a la función de Gravity()
        Gravity();

        //Llama a la función de Jump y solamente se ejecutará si se pulsa el botón de saltar y cuando esté en el suelo
        if(_jumpAction.WasPressedThisFrame() && IsGrounded())
        {
            Jump();
        }

        //Animación de saltar, cuando esté en el suelo puede saltar
        _animator.SetBool("IsGrounded", IsGrounded());
    }
    
    //Función de movimiento
    void Movement()
    {
        //Dirección del movimiento y hacia dónde te mueves
        Vector3 direction = new Vector3(_moveInputValue.x, 0, _moveInputValue.y);

        //Para controlar la rotación del personaje (controla la posición del Joystick) y que el personaje tenga en cuenta hacia dónde mira la cámara
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y;
        //Pasar de un ángulo a otro con x tiempo, para que no sea de golpe sino que haya un movimiento suave
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _cameraTransform.eulerAngles.y, ref _turnSmoothVelocity, _smoothTime);

        //Para que rote sobre sí mismo
        transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

        //Para que no se mueva solo y solamente se mueva cuando toquemos alguna tecla
        if(direction != Vector3.zero)
        {
            //Para que el personaje se mueva a la dirección que está mirando
            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            //Personaje se mueve en la dirección correspondiente con la velocidad (en X y en Z)
            _controller.Move(moveDirection * _movementSpeed * Time.deltaTime);
        }

        //Animaciones con transiciones para que no se cambie directamente de animación
        _animator.SetFloat("Horizontal", _moveInputValue.x, 0.2f, Time.deltaTime);
        _animator.SetFloat("Vertical", _moveInputValue.y, 0.2f, Time.deltaTime);
    }

    //Función de gravedad
    void Gravity()
    {
        //Cuando no toque el suelo
        if(!IsGrounded())
        {
            //Para que se sume la gravedad (ej: si te caes de una mesa la gravedad es mínima, en cambio si te caes de un acantilado la gravedad se suma y vas más rápido)
            _playerGravity.y += _gravity * Time.deltaTime;
        }

        //Cuando toque el suelo y la gravedad del personaje sea menor a 0
        //Para poner fijo un valor de gravedad una vez que estás en el suelo (cuando no saltas o has dejado de saltar)
        if(IsGrounded() && _playerGravity.y < 0)
        {
            //Para que se sume la gravedad
            _playerGravity.y = _gravity;
        }
        
        //Mueve el personaje hacia arriba o hacia abajo (en Y)
        _controller.Move(_playerGravity * Time.deltaTime);
    }

    //Función de salto
    void Jump()
    {
        //Cambia el valor y hace que salte hacia arriba (porque la gravedad es negativa) y hace que salte a la altura que le pongas en el JumpForce y que no sea aleatoria
        _playerGravity.y = Mathf.Sqrt(_jumpForce * -2 * _gravity);
    }

    //Función booleana de está en el suelo (SÍ o NO está en el suelo)
    bool IsGrounded()
    {
        //Devuelve la información de si está en el suelo o no, inspeccionando si el suelo está dentro del radio del ground sensor y si el suelo está dentro de la capa de suelo
        return Physics.CheckSphere(_sensorPosition.position, _sensorRadius, _groundLayer);
    }
}