using UnityEngine;

public class CamaraControler : MonoBehaviour
{
    [SerializeField] private float speed; //velocidad de la camara
    private float currentPosX;  //pocicion actual
    private Vector3 velocity = Vector3.zero;

    private void Update()
    {
        //SmoothDamp graduall change a vector towards the desired goal
        //parametros:
        // 1 La posicion actual de la camara
        // 2  el destino
        // 3 la velocidad vectorial del cambio
        // 4 speed of a movement 
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(currentPosX, transform.position.y, transform.position.z),ref velocity, speed);
    }

    public void MoveToNewRoom(Transform _newRoom )
    {
        currentPosX = _newRoom.position.x;
    }

}
