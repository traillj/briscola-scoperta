// Briscola Scoperta
// Author: traillj

using UnityEngine;

public class Deck
{
    private GameObject[] cards;

    // Initial number of cards in the deck
    private int size;

    public Deck(GameObject[] cards)
    {
        this.cards = cards;
        this.size = cards.Length;
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

    public GameObject PeekTopCard()
    {
        GameObject topCard = null;
        if (!IsEmpty())
        {
            topCard = cards[size - 1];
        }
        return topCard;
    }

    public GameObject PeekBottomCard()
    {
        return cards[0];
    }

    public bool IsEmpty()
    {
        return (size <= 0);
    }
}
