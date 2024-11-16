using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikehead : EnemyDamage
{
    [Header("Spikehead attributes")]
    [SerializeField] private float speed; //velocidad
    [SerializeField] private float range; // la distancia en la que puede observar
    [SerializeField] private float checkDelay; // delay entre ataques
    [SerializeField] private LayerMask playerLayer; // delay entre ataques
    private Vector3[] directions = new Vector3[4]; // 4 direcciones a las que mira the spikhead
    private float checkTimer;
    private Vector3 destination;
    private bool attacking; // cuando esta atacando al jugador y cuando no

    [Header("SFX")]
    [SerializeField] private AudioClip spikeheadSound; //audio

 

    private void OnEnable() // es llamado cada vez que es activado  
    {
        Stop();
    }
    private void Update()
    {
        //si esta atacando, ve a su destino final, si no, no hagas nada
        if (attacking)
            transform.Translate(destination * Time.deltaTime * speed);
        else
        {
            checkTimer += Time.deltaTime;
            if (checkTimer > checkDelay)
                CheckForPlayer();
        }

    }

    private void CheckForPlayer()
    {
        //comprobar si esta viendo al jugador
        CalculateDirections();
        for (int i = 0; i < directions.Length; i++)
        {
            //muestra lineas rojas para debugear
            Debug.DrawRay(transform.position, directions[i], Color.red);
            //origen
            // que direccion
            // que distancia
            // que leyer debe detectar
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i],range, playerLayer);

            if(hit.collider != null && !attacking) // si detecta algo y no esta atacando comenzara a atacar
            {

                SoundManager.instance.PlaySound(spikeheadSound);
                attacking = true;
                destination = directions[i];
                checkTimer = 0;
            }
        }
    }

    private void CalculateDirections()
    {
        directions[0] = transform.right * range; //derecha
        directions[1] = -transform.right * range; // izquierda
        directions[2] = transform.up * range; // arriba
        directions[3] = -transform.up * range; //abajo
    }

    private void Stop()
    {
        destination = transform.position; // el destino es la pocicion actual asi que ya no debe moverse
        attacking = false;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        //debe parar despues de dañarlo
        Stop();
    }
}
