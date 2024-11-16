
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown; //poder modificar en la UI el cooldown entre ataques
    [SerializeField] private Transform firePoint; //pocicion de donde las fireball se disparan
    [SerializeField] private GameObject[] fireballs; //donde se estan guardando las fireballs
    [SerializeField] private AudioClip fireballSound;// enviar un audio clip al sound manager

    private Animator anim;
    private PlayerMovement playerMovement; //llamando al anterior script
    private float cooldownTimer = Mathf.Infinity; // debe comenzar con un valor alto o no podra dar su primer ataque

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        // si hace clic activa esto
        //comprobar si suficiente tiempo a pasado desde el anterior ataque
        if (Input.GetKeyDown(KeyCode.Z) && cooldownTimer > attackCooldown && playerMovement.canAttack()) // va a ver si puede atacar con la funcion del script "playerMovement"
            Attack();

        // aumenta conforme pasa el tiempo
        cooldownTimer += Time.deltaTime;

    }

    //metodo para el ataque
    public void Attack()
    {
        SoundManager.instance.PlaySound(fireballSound);
        //activa el trigger
        anim.SetTrigger("attack");
        //reinicia el cooldown timer
        cooldownTimer = 0;

        fireballs[FindFireball()].transform.position = firePoint.position;  // cada vez que ataques tomaremos una fireball y renuevaremos su pocicion al firepoint
        fireballs[FindFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x)); //le estamos pasando al scprit "proyectile" la direccion  que mira el jugador
    }

    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if(!fireballs[i].activeInHierarchy)//busca si esta activa en el array, si no esta entonces retorna el index de la no activa
                return i;
        }
        return 0;
    }


}
