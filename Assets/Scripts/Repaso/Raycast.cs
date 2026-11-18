using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Raycast : MonoBehaviour
{
    //Hacer que caiga un rayo y que cuando toque algo pasa algo
    //Detecta colisiones (ej: cuando haces click en la pantalla y el player va a esa direccion)


    /*Crear un input nuevo:
    - MousePosition, poner el Action Type en Value y el Control Type en Vector 2 (porque es un input de movimiento)
    - En el Binding poner en el Path Position [Mouse]

    Poner otro using Engine para poder usar el Input System:
    using UnityEngine.inputSystem;

    Poner variables InputAction para poder usar el Input System (InputSystem;).

    En el void Awake poner los Input Action para que hagan la acción del Input System correspondiente

    En el void Update:
    - Detecta botones y hace que hagan su función

    En el void ShotRayCast:
    - Rellenar el Vector2
    - Des de la camara se hace click y dispara un rayo, coge la información de la posición del ratón
    - Guarda la informacuón de la posición del ratón
    - Hacer los if, que va a hacer que el rayo se dispare y que al colisionar con algo que pase x
        - Si colisiona con "objeto 1" cambia de escena a "Escena 1"
    
    Poner otro Unity Engine para poder usar el el cambio de escenas (SceneManagement):
    - File - Build Profiles - Scene List - Arrastrar las escenas

    En el void Update:
    - Llamar a la función de ShotRayCast()
    */

    //Variable que utiliza el Input System, que utilizaremos para que al hacer click pase algo
    private InputAction _clickInput;
    //Variable que utiliza el Input System, que utilizaremos para la dirección del ratón
    private InputAction _positionInput;
    //Variable que utiliza el Input System, que utilizaremos para la posición del ratón
    private Vector2 _mousePosition;


    void Awake()
    {
        //Abre el Input System y busca algo que se llame como lo que está dentro de [""]
        _clickInput = InputSystem.actions["Attack"];
        _positionInput = InputSystem.actions["MousePosition2"];
    }

    //El que detecta botones
    void Update()
    {
        //Cuando se ejecute el Input de _clickInput que se ejecute la función de ShotRayCast
        if(_clickInput.WasPressedThisFrame())
        {
            ShotRayCast();
        }
    }

    //Función: DisparaRayCast
    void ShotRayCast()
    {
        //El Vector2 se poner dentro de ésta función porque solamente se activa cuando haces click al ratón, no hace falta ponerlo en el Update porque además pierde rendimiento.
        _mousePosition = _positionInput.ReadValue<Vector2>();

        //Des de la camera dispara un rayo hacia delante y pilla la posición del ratón (cuando haces click dentro de la pantalla, solamente se puede dentro del límite de la cámara)
        Ray ray = Camera.main.ScreenPointToRay(_mousePosition);

        //Se guarda la información de dónde impacta el rayo
        RaycastHit hit;

        //El if va a hacer que dispare el rayo
        // Dispara el rayo con una distancia y un tamaño (como no sabemos la distancia hacemos que sea un rayo infinito), se choca con algo? se ejecuta el if, no se choca con algo) no se ejectura el if
        if(Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            //Para saber con qué ha chocado el rayo
            if(hit.collider.name == "objeto1")
            {
                //Cargar la Escena 1
                SceneManager.LoadScene("Escena 1");
            }

            /* Otras formas de hacerlo, solo que a partir de un Tag o una Layer
            if(hit.collider.CompareTag("Tag1"))
            {
            
            }
            
            if(hit.collider.gameObject.layer == 3)
            {
            
            }
            */
        }
    }
}