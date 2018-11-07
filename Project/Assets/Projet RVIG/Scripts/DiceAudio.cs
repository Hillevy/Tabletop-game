using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceAudio : MonoBehaviour {

    public GameObject tray;

    private AudioSource audio;
    private Rigidbody rigidBody;
    private bool touch;

    // Use this for initialization
    void Start ()
    {
        audio = GetComponent<AudioSource>();
        rigidBody = GetComponent<Rigidbody>();
        touch = false;
    }
    
    // Update is called once per frame
    void Update ()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!touch)
        {
            audio.Play();
            audio.volume *= rigidBody.velocity.magnitude * 10f;
            touch = true;
        }
        else
        {
            touch = false;
        }
    }
}
