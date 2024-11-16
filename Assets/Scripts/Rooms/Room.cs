using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies; // aqui estan todo los enemigos
    private Vector3[] initialPosition; // inicial postion of all enemies

    private void Awake()
    {
        //Save the initial positions of enemies
        initialPosition = new Vector3[enemies.Length];
        for (int i = 0; i < enemies.Length; i++)
        {
            if(enemies[i] != null) 
            initialPosition[i] = enemies[i].transform.position;
        }
    }
    public void ActivateRoom ( bool _status)
    {
        //activar y desactivar una sala
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                enemies[i].SetActive(_status);
                enemies[i].transform.position = initialPosition[i];
            }
                
        }
    }
}
