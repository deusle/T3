
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed; //pone variable para modificar en UI de Unity
    [SerializeField] private float jumpPower; //pone variable para modificar en UI de Unity
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D body;  // crea variable "body"
    private Animator anim;    //llamando a Animation que fue creado en el proyecto, se llama para controlar las animaciones, el general para darle logica a los parametros
    private BoxCollider2D boxCollider; // llama al box collider para la caja 
    private float wallJumpCooldown; // responsable de crear delays entre jumps
    private float horizontalInput; // vale entre -1 y 1 y se define en Update
    private float botones;
    [Header("SFX")]
    [SerializeField] private AudioClip jumpSound;
    

    private void Awake() //Inicializa
    {
        body = GetComponent<Rigidbody2D>();  //referencia el "Rigidbody2d" y lo guarda en "body"
        anim = GetComponent<Animator>();    //referenciar el "Animation" y lo guarda en anim
        boxCollider = GetComponent<BoxCollider2D>(); //referencia al "boxCollider2D"
    }

    private void Update() //Corre en cada frame
    {
        // Input.GetAxis("Horizontal") es una varaible definida por unity que dara -1 si se presioan la izquierda y 1 con la derecha
        horizontalInput = Input.GetAxis("Horizontal");
        
        // Determina el movimiento, dando prioridad a los botones táctiles
        if (botones !=0)
            MoveTactil(botones);
        else
            Move(horizontalInput);
        // if (horizontalInput > 0.01f)
        //     transform.localScale = Vector3.one;
        // else if (horizontalInput < -0.01f)
        // transform.localScale = new Vector3(-1,1,1);
        



        //los parametros del animator
        //esta observando si se presiona las teclas para moverse derecha o inzquierda
        // si 0 no es igual a 0 entonces si se esta presionando por lo tanto debe activar la animacion de run
        //Se pasa el parametro boolean para comprobar si se esta en el suelo con "grounded"
        // anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        //wall jump logic
        if(wallJumpCooldown > 0.2f)
        {
          
            //velocidad del jugador
            body.velocity = new Vector2((horizontalInput+botones) * speed, body.velocity.y);

            // si esta en la pared y no esta en el suelo
            if (onWall() && !isGrounded())
            {
                body.gravityScale = 0; // quita la gravedad
                body.velocity = Vector2.zero; // para la velocidad
            }
            else body.gravityScale = 3; // devuelve la gravedad cuando ya no esta en la pared o toca suelo

            if (Input.GetKey(KeyCode.Space))//verifica que space sea presionado 
            {
                Jump();
                if(Input.GetKeyDown(KeyCode.Space) && isGrounded()) // para envitar que el audio se repita muchas veces, solo se activa cuando se empieza a presioanr la tecla
                {
                    SoundManager.instance.PlaySound(jumpSound);
                }
            }
                
        }
        else
            wallJumpCooldown += Time.deltaTime; // para los cooldown entre jumps
    }

    //Se determina que es contituye jump
    public void Jump()
    {
        // el comprobador para saber si esta en el suelo se encuentr aqui de tal forma que se pueda manejar el jump de diferentes formas
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            // activa el trigger jump 
            anim.SetTrigger("jump");
        }
        // el salto de la pared deberia empujar hacia el lado opuesto y para arriba
        else if (onWall() && !isGrounded())
        {
            if(horizontalInput == 0)
            {
                // Mathf.Sign retorma 1 si esta a la derecha y -1 a la izquierda, lo usamos en negativo para obtener que debemos empujarloa lado opuesto
                // se pone 0 pq no piensa empujarlo arriba
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
                // Mathf.Sign retorma 1 si esta a la derecha y -1 a la izquierda, lo usamos en negativo para obtener que debemos empujarloa lado opuesto
                // el 6 es la furza que lo empuja para arriba
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);

            wallJumpCooldown = 0;
            
            
        }

    }

    // Se determina si se entra en colision con algun objeto
    //Se usa tag del unity para que se referencie el suelo
   

    //dira si el personaje esta o no esta tocando suelo, se necesita esto para diferenciarlo de otras superficies (pared)
    private bool isGrounded()
    {
        // invoca una caja que si colisiona con algo en su area lo detecta y envia una señal
        //Los parametros son los siguientes:
        // - Origen de la box
        // - the size of the box
        // - el angulo , es 0 pq no queremos rotar la box
        // - direccion
        // - Distacia, que significa que tan abajo del player quieres pocicionar la caja
        // Layer mask , determina que esta buscando el box y que debe ignorar
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center,boxCollider.bounds.size,0, Vector2.down,0.1f, groundLayer);
        return raycastHit.collider != null;
    }


    private bool onWall()
    {
        // invoca una caja que si colisiona con algo en su area lo detecta y envia una señal
        //Los parametros son los siguientes:
        // - Origen de la box
        // - the size of the box
        // - el angulo , es 0 pq no queremos rotar la box
        // - direccion, esta vez en vez de ir para abajo para detectar suelo va a la derecha o izquierda 
        // - Distacia, que significa que tan abajo del player quieres pocicionar la caja
        // Layer mask , determina que esta buscando el box y que debe ignorar
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x,0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    
    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }

    public void Move(float direction)
    {
        // Actualiza la velocidad del Rigidbody2D
        body.velocity = new Vector2(direction * speed, body.velocity.y);

        // Cambia la dirección del sprite
        if (direction > 0.01f)
            transform.localScale = Vector3.one; // Mira hacia la derecha
        else if (direction < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1); // Mira hacia la izquierda

        // Actualiza la animación de correr
        anim.SetBool("run", direction != 0);
    }

    public void MoveTactil(float direction)
    {
        // Actualiza la velocidad del Rigidbody2D
        body.velocity = new Vector2(direction * speed, body.velocity.y);

        // Cambia la dirección del sprite
        if (direction > 0.01f)
            transform.localScale = Vector3.one; // Mira hacia la derecha
        else if (direction < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1); // Mira hacia la izquierda

        // Actualiza la animación de correr
        anim.SetBool("run", direction != 0);
    }
    public void SetHorizontalInput(float value)
    {
        botones = value; // Asigna el valor desde los botones táctiles
    }

    public void StopHorizontalInput()
    {
        botones = 0; // Detiene el movimiento al soltar el botón
    }
}
