using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : EnemyDamage //will damage the player eveytime they touch
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    private float lifetime;
    private Animator anim;
    private BoxCollider2D coll;

    private bool hit;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
    }
    public void ActivateProjectile()
    {
        hit = false;
        lifetime = 0;
        gameObject.SetActive(true);
        coll.enabled = true; 

    }

    private void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate (movementSpeed,0,0);

        lifetime += Time.deltaTime; //tiempo de vida de la fecha

        if(lifetime > resetTime) //expira el tiempo 
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;

        base.OnTriggerEnter2D(collision);
        coll.enabled = false; //evita que la fireball manatenga su collider y golpee al jugador cuando no debe
        gameObject.SetActive(false); // quitara la fecha

        if (anim != null)
            anim.SetTrigger("explode");//cuando el objeto sea una fireball explotara
        else
            gameObject.SetActive(false); 
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
