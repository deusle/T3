using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header ("Health")] // aparece como emcabeza, funciona para orden 
    [SerializeField] private float startingHealth; //vida inicial
    public float currentHealth { get; private set; } //puedes obtener la vida desde cualquier lugar pero solo la puedes set en este script
    public Animator anim;
    private bool dead; // se asegua que no se repita la animacion de muerte

    [Header("iFrames")] // aparece como emcabeza, funciona para orden 
    [SerializeField] private float iFramesDuration; //duraciom de la inmunidad
    [SerializeField] private int numberOfflashes;   // cuanto va a brillar rojo
    private SpriteRenderer spriteRend; //cambiar el color del jugador cuando es inmune

    [Header("Components")]
    [SerializeField] private Behaviour[] components;
    private bool invunerable;

    [Header("Death")]
    [SerializeField] private AudioClip deathSound; // audio de muerte
    [SerializeField] private AudioClip hurtSound;

    private int points = 0;
    [SerializeField] public Text ScoreText;

    //llamar al score
    private GameObject gameManager;

    private void Awake()
    {
        currentHealth = startingHealth; // la vida del inicio
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>(); // llamando al sprite renderer para cambiar de color
        gameManager = GameObject.Find("GameManager");
    }

    public void TakeDamage(float _damage)
    {
        // la vida no va below 0 or above max
        //parametro
        //1 valor normal cuando se quita vida
        //2 minimo valor
        //3 maximo valor
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            //iframes
            StartCoroutine(Invulnerability()); // no puedes llamar a un IEnumeration solo, tienes que llamarlo con startCoroutine para que funcione
            //sonido de recibir daño
            SoundManager.instance.PlaySound(hurtSound);
        }
        else
        {
            if (!dead) //se asegura de que no estaba ya muerto
            {
                anim.SetTrigger("die");
                gameManager.GetComponent<Score>().AddScore(10);

                //Desactiva todo lo agregado a componente

                foreach (Behaviour component in components)
                     component.enabled = false;
                

                dead = true;
                SoundManager.instance.PlaySound(deathSound); // cuando muere sale el audio
                
            }
            
        }

    }

    public void AddHealth(float _value)
    {
        // la vida no va below 0 or above max
        //parametro
        //1 valor normal cuando gana vida
        //2 minimo valor
        //3 maximo valor
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }
    private IEnumerator Invulnerability()
    {
        // Parametros:
        // layer
        // layer
        // true significa que las coliciones seran ignoradas
        Physics2D.IgnoreLayerCollision(10,11,true);
        for(int i = 0; i < numberOfflashes; i++)
        {
            // 3 parametros para el color RGB 0 to 1
            // ultimo parametro para hacerlo medio transparente

            spriteRend.color = new Color(1,0,0,0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfflashes * 2 ));  // espera un momento antes de ejecutar lo siguiente
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration/ (numberOfflashes * 2 )); // Se debe dividir para saber cuanto durar entre flashes y * 2 pq estamos pausando 2 veces
        }
        // invunerability duration
        Physics2D.IgnoreLayerCollision(10,11,false);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false); // disable the game object
        points++;
        ScoreText.text = points.ToString();
    }
    
}
