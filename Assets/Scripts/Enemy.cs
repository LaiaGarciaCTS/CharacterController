using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public float _maxHealth;

    public float _attackDamage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Funcion buscar al personaje
    public void FindPlayer()
    {

    }

    //Funcion patrullar
    public void Patrol()
    {

    }

    public virtual void Attack()
    {
        Debug.Log("ataque generico");
    }

    public void TakeDamage()
    {
        Debug.Log("recibiendo daño");
    }
}
