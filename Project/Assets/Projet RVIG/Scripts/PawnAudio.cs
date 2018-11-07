using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnAudio : MonoBehaviour {

    private AudioSource audio;
    private bool touch;

    // Use this for initialization
    void Start ()
    {
        audio = GetComponent<AudioSource>();
        touch = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.name == "Tile(Clone)" && !touch) //Tiles
        {
            audio.Play();
            touch = true;
            //collision.collider.gameObject.layer == 8 && 
        }
        else
        {
            touch = false;
        }
    }
}
