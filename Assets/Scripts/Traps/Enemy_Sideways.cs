using UnityEditor;
using UnityEngine;

public class Enemy_Sideways : MonoBehaviour
{
    [SerializeField] private float movementDistance; // how far the object move
    [SerializeField] private float speed; // speed of movement
    [SerializeField] private float damage; //daño hecho
    private bool movingLeaft;
    private float leftEdge;
    private float rightEdge;

    private void Awake()
    {
        leftEdge = transform.position.x - movementDistance; //negativo apra que sea izquierda
        rightEdge = transform.position.x + movementDistance; // positivo para derecha
    }

    private void Update()
    {
        if (movingLeaft)
        {
            if(transform.position.x > leftEdge)
            {
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
                movingLeaft = false;
        }
        else
        {
            if (transform.position.x < rightEdge)
            {
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
                movingLeaft = true;

        }
    }

    //Si coliciona se envia el daño
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag  == "Player")
        {
            //pasa el daño al script de health
            collision.GetComponent<Health>().TakeDamage(damage);
        }
    }

}
