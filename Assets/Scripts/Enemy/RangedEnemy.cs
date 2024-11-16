using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private float damage;

    [Header("Ranged Attack")]
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject[] fireballs;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity; //Hace que el primer ataque no tenga tiempo de espera


    [Header("Fireball Sound")]
    [SerializeField] private AudioClip fireballSound;
    

    //Referencias
    private Animator anim;
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
            if (cooldownTimer >= attackCooldown )
            {
                cooldownTimer = 0; //reinicia el contado luego de dar el ataque
                anim.SetTrigger("rangedAttack");

            }
        }
        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight(); //Si ve al jugador dejara de patrullar y si no lo ve seguira patrullando
    }

    private void RangedAttack() //Permite lanzar los proyectiles
    {
        SoundManager.instance.PlaySound(fireballSound);
        cooldownTimer = 0;
        //Disparar el proyectil
        fireballs[FindFireball()].transform.position = firepoint.position;
        fireballs[FindFireball()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }

    private int FindFireball()
    {
        for (int i = 0; i  < fireballs.Length; i++)
        { 
         if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, //Se define origen  , raneg aumenat el rango, y transform local.x ayuda a apuntar el box dodne mira el enemigo
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), //se define el tamaño del box collider //esto esta comenzando a confundir 0_o 
            0, Vector2.left, 0, playerLayer); //Distancia y se define que layer mask donde se tomara en cuenta



        return hit.collider != null;  //Retorna true cuando no es null, y false cuando es null
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
}
