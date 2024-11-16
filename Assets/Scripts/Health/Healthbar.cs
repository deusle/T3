
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Health playerHealth; //vida actual
    [SerializeField] private UnityEngine.UI.Image totalhealthBar; //corazon en negro
    [SerializeField] private UnityEngine.UI.Image currenthealthBar; //corazones en rojo

    private void Start()
    {
        totalhealthBar.fillAmount = playerHealth.currentHealth / 10; //corazones negros, el maximo de vida al inicio
    }

    private void Update()
    {
        currenthealthBar.fillAmount = playerHealth.currentHealth / 10; //divide entre 10 para poder  mover correctamente el fill
    }
}
