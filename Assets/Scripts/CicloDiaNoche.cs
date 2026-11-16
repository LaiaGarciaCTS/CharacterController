using UnityEngine;

public class CicloDiaNoche : MonoBehaviour
{
    [SerializeField] int _velocidadRotacionSol = 10;

    void Update()
    {
        transform.Rotate(_velocidadRotacionSol * Time.deltaTime, 0, 0);
    }
}
