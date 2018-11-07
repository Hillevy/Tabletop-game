using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckDisplayer : MonoBehaviour {

    public Texture emptyDeckTex;
    public Texture deckTex;

    private Renderer rend;

    // Use this for initialization
    void Start () {
        rend = GetComponent<Renderer>();
        rend.material.mainTexture = deckTex;
    }

    //Changes the texture of the deck place to indicate that the deck is empty
    public void changeTexture()
    {
        rend.material.mainTexture = emptyDeckTex;
    }

}
