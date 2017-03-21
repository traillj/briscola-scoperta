// Briscola Scoperta
// Author: traillj

using UnityEngine;

public class Deck
{
    private GameObject[] cards;
    private int size = 40;

    public Deck()
    {
        cards = GameObject.FindGameObjectsWithTag("Card");
        ShuffleCards();
    }

    // Shuffles the cards by sorting randomly and
    // reassigning the layer order of each card.
    private void ShuffleCards()
    {
        System.Array.Sort(cards, RandomSort);
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].GetComponent<Renderer>().sortingOrder = i;
        }
    }

    private int RandomSort(GameObject a, GameObject b)
    {
        return Random.Range(-1, 2);
    }

    public GameObject DrawTopCard()
    {
        size--;
        return cards[size];
    }
}
