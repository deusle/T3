
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]private float speed; //velocidad del proyectile
    private float direction;
    private bool hit; //golpeo o no?
    private float lifetime; // duracion en juego

    private BoxCollider2D boxCollider; //llamando al collider
    private Animator anim; //llamando a animator

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        
        if (hit) return; // si hizo hit retornara y ya no ejecutara el siguiente codigo
        float movementSpeed = speed *Time.deltaTime * direction; //la velocidad y direccion
        transform.Translate(movementSpeed,0,0); // añadiendo velocidad en el eje

        lifetime += Time.deltaTime;
        if(lifetime > 5) gameObject.SetActive(false);
    }

    //si entra en colicion con algo
    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        boxCollider.enabled = false; 
        anim.SetTrigger("explode"); //activa el trigger de animacion

        if (collision.tag == "Enemy")
            collision.GetComponent<Health>().TakeDamage(1); //Hara 1 de daño si colisiona con objetos con el tag de enemy
    }

    public void SetDirection(float _direction) //le dice al fireball si va derecha o izquierda
    {
        lifetime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false; //reincia el estado de la fireball
        boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction) // compruba si esta en la correcta direccion
            localScaleX = -localScaleX;
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false); //Desactiva cuando la animacion termina
    }

}