using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firetrap : MonoBehaviour
{
    [SerializeField] private float damage;

    [Header("Firetrap Timers")]
    [SerializeField] private float activationDelay; // cuanto tiempo tiene que pasar desde que el player lo activa, (se activara en :)
    [SerializeField] private float activeTime; // cuanto tiempo esta activo
    private Animator anim;
    private SpriteRenderer spriteRend;

    [Header("SFX")]
    [SerializeField] private AudioClip firetrapSound;

    private bool triggered; // que activa la trmpa
    private bool active; // dicta que la trampa esta activa y puede dañar al jugador

    private void Awake()
    {
        anim= GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") // si coliciona con el player
        {
            if (!triggered) // si no esta activa
            {
                StartCoroutine(ActivateFiretrap());
            }
            if (active) // si esta activa dañar al player
                collision.GetComponent<Health>().TakeDamage(damage);
            

            
        }
    }

    private IEnumerator ActivateFiretrap()
    {
        triggered = true; // se debe especificar que la trampa a sido activado y que ya no puede aver multiples activaciones
        spriteRend.color = Color.red; // se vuelve rojo

        yield  return new WaitForSeconds(activationDelay); //espera un momento
        SoundManager.instance.PlaySound(firetrapSound);
        spriteRend.color = Color.white; /// vuelve a su color natural
        active = true;
        anim.SetBool("activated", true);
        yield return new WaitForSeconds(activeTime); // se activa durante un evento
        active = false;
        triggered = false;
        anim.SetBool("activated", false);
    }


}
