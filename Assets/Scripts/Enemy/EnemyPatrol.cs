using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;

    [Header("Movement paramaeters")]
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft; // para determianr la direccion

    [Header("Idle Behaviour")]
    [SerializeField] private float idleDuration;
    private float idleTimer;


    [Header("Enemy Animator")]
    [SerializeField] private Animator anim;


    private void Awake()
    {
        initScale = enemy.localScale; //el local scale del enemigo al inicio del juego
    }

    private void OnDisable()
    {
         anim.SetBool("moving",false); // cuando el enemy patrol es disable o destruido, el movimiento se cancelara
    }

    private void Update() 
    {
        if (movingLeft)
        {
            if (enemy.position.x >= leftEdge.position.x) // Aseguramos que el enemigo no haya legado al limite 
            MoveInDirection(-1);
            else
            {
                //si no es el caso cambia de dirrecion
                DirectionChange();
            }
        }
        else
        {
            if (enemy.position.x <= rightEdge.position.x)// Aseguramos que el enemigo no haya legado al limite 
            MoveInDirection(1);
            else
            {
                //si no es el caso cambia de dirrecion
                DirectionChange();
            }
        }
    }

    private void DirectionChange() // cambia el valor al contrario
    {
        anim.SetBool("moving", false); // cancelamos la animacion
                                       // 
        idleTimer += Time.deltaTime; //el idlle tiemr sera usado paa darle un break en la patrulla al llegar al limite

        if(idleTimer > idleDuration)  //Solo cuando el idle tiemr se ha cumplido cambia de direccion
        movingLeft = !movingLeft;
    }

    private void MoveInDirection(int _direction)
    {
        idleTimer = 0; //reinicia el idletimer
        anim.SetBool("moving", true); //activa la animacion de Run

        //Make enemy face direction
        enemy.localScale =  new Vector3(Mathf.Abs(initScale.x) * _direction,  // se usa valor absoluto para evitar problemas cuando x sea negativo
            initScale.y,initScale.z);

        //Mover en dicha direccion
        enemy.position = new Vector3(enemy.position.x+ Time.deltaTime * _direction * speed, 
            enemy.position.y, enemy.position.z);
    }
}
