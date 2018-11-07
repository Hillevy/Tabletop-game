using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour {

    public GameObject cardBoard;

    private static Stack<GameObject> deck = new Stack<GameObject>();
    private GameObject[] allCards; //all the cards in the deck at the beginning
    private GameObject[] cardsPositions; //Spots where the cards can be placed
    private int count; //total number of spots where the cards can be placed
    private int nbCards; //total number of cards

    // Use this for initialization
    void Start ()
    {
        createDeck();
        nbCards = deck.Count;
        allCards = new GameObject[nbCards];
        getAllCards();
        deck.Peek().gameObject.SetActive(true); //Only the card at the top of the deck is visible
        cardBoardPositions();
    }
    
    // Update is called once per frame
    void Update ()
    {
        if (deck.Count >= 1)
        {
            checkFreePlaces();
            /* If the card at the top is placed, it is removed from the stack.
             * The next card in the deck becomes visible once the previous visible card is placed */
            if (checkCardPosition()) 
            {
                deck.Pop();
                if (deck.Count > 0)
                {
                    deck.Peek().gameObject.SetActive(true);
                    getAllCards();
                }
            }
        }
        blockDiscard();
        if (deck.Count == 0)
        {
            //We change the texture on the deck from "Deck" to "Deck (empty)" 
            GameObject.Find("CardSnapDeck").GetComponent<DeckDisplayer>().changeTexture();
        }
    }

    //Creates the deck and makes all cards invisible
    void createDeck()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
            deck.Push(child.gameObject);
        }
    }

    //Makes an array containing all the cards present in the deck at the beginning
    void getAllCards()
    {
        for (int i = 0; i < nbCards; i++)
        {
            allCards[i] = transform.GetChild(i).gameObject;
        }
    }

    //Get all the positions where the cards can be placed
    void cardBoardPositions()
    {
        count = cardBoard.transform.childCount;
        cardsPositions = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            cardsPositions[i] = cardBoard.transform.GetChild(i).gameObject;
        }
    }

    //Checks if the card at the top of the deck has been placed
    bool checkCardPosition()
    {
        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j < deck.Count; j++)
            {
                float distX = Mathf.Abs(allCards[j].transform.position.x - cardsPositions[i].transform.position.x);
                float distZ = Mathf.Abs(allCards[j].transform.position.z - cardsPositions[i].transform.position.z);
                if (distX <= 0.05f && distZ <= 0.5f)
                {
                    if(cardsPositions[i].tag == "Free")
                        cardsPositions[i].layer = 0;
                    return true;
                }
            }
        }
        return false;
    }

    //Makes a place available if no card is placed on it, and unavailable if a card is on it.
    void checkFreePlaces()
    {
        
        foreach (GameObject go in cardsPositions)
        {
            if (go.layer == 0 && go.name != "Discard")
            {
                foreach (GameObject card in allCards)
                {
                    /* For each place for cards, we check if a card is placed on it or not. 
                     * If it is, the place remains not snappable : layer = 0
                     * If it is not, we make the place snappable again : layer = 9 */
                    float distX = Mathf.Abs(card.transform.position.x - go.transform.position.x);
                    float distZ = Mathf.Abs(card.transform.position.z - go.transform.position.z);
                    if (!(distX <= 0.05f && distZ <= 0.05f))
                    {
                        return;
                    }
                    go.layer = 9;
                }
                
            }
            else if (go.layer == 9 && go.name != "Discard")
            {
                foreach (GameObject card in allCards)
                {
                    /* If a card is placed, we make sure that its place is no longer available : layer = 0 */
                    float distX = Mathf.Abs(card.transform.position.x - go.transform.position.x);
                    float distZ = Mathf.Abs(card.transform.position.z - go.transform.position.z);
                    if (distX <= 0.05f && distZ <= 0.05f)
                    {
                        go.layer = 0;
                        return;
                    }
                }
            }
        }
    }

    // Disables the script which allows to grab cards when a card is in the discard
    void blockDiscard()
    {
        foreach(GameObject go in allCards)
        {
            if (Vector3.Distance(go.transform.position, cardsPositions[0].transform.position) <= 0.05f)
            {
                go.GetComponent<GrabCard>().enabled = false;
            }
        }
    }
}