using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float damage;
    [SerializeField] private float range;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity; //Hace que el primer ataque no tenga tiempo de espera

    [Header("Attack Sound")]
    [SerializeField] private AudioClip attackSound; //clip de sonido
    

    //Referencias
    private Animator anim;
    private Health playerHealth;
    private EnemyPatrol enemyPatrol;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>(); // obtiene el componente del padre no del objeto en si
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime; //incrementa el cooldownTimer en cada frame

        //atacar cuando el enemigo ve al player
        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown && playerHealth.currentHealth>0) 
            {
                cooldownTimer = 0; //reinicia el contado luego de dar el ataque
                anim.SetTrigger("meleeAttack");
                SoundManager.instance.PlaySound(attackSound); // sonido del ataque
              
            }
        }
        if (enemyPatrol != null) 
            enemyPatrol.enabled = !PlayerInSight(); //Si ve al jugador dejara de patrullar y si no lo ve seguira patrullando
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, //Se define origen  , raneg aumenat el rango, y transform local.x ayuda a apuntar el box dodne mira el enemigo
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), //se define el tamaño del box collider //esto esta comenzando a confundir 0_o 
            0, Vector2.left, 0, playerLayer); //Distancia y se define que layer mask donde se tomara en cuenta

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<Health>();// Usamos esto ya que el enemigo siempre vera al player y nadie mas
        
        return hit.collider !=null;  //Retorna true cuando no es null, y false cuando es null
    }

    //Visualizar la vision del enemigo
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DamagePlayer()
    {
        if(PlayerInSight()) //Asegura que el jugador este en la zona del ataque para que el daño cuente
        {
            playerHealth.TakeDamage(damage);            
        }
    }
}
