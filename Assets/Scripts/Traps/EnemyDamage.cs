
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] protected float damage; //daño hecho

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player") // si coliciona con el tag player
        {
            collision.GetComponent<Health>().TakeDamage(damage); // llama al health y recibe daño
        }
    }
}
