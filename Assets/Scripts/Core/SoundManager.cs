using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; } // un singleton, unica variable en todo lugar
    private AudioSource source; //llama al audio

    private void Awake()
    {
        
        source = GetComponent<AudioSource>(); //instancia del audio

        
        

        if(instance == null) // evitar el peligro de varias instancias
        {
            instance = this;
            //No destruye un objeto
            DontDestroyOnLoad(gameObject);
        }
        else if  (instance != null && instance!= this) // significa que hay 2 instancias, debe destruir una
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip _sound)
    {
        source.PlayOneShot(_sound); // play a audio clip only once
    }
}
