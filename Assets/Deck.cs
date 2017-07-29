// Briscola Scoperta
// Author: traillj

using UnityEngine;

// A container for card game objects.
public class Deck
{
    // Game objects representing cards
    private GameObject[] cards;

    // Number of cards currently in the deck
    private int size;

    // Creates a new Deck and shuffles.
    public Deck(GameObject[] cards)
    {
        this.cards = cards;
        this.size = cards.Length;
        Shuffle();
    }

    // Shuffles the cards by sorting randomly and
    // reassigning the layer order of each card.
    private void Shuffle()
    {
        System.Array.Sort(cards, RandomSort);
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].GetComponent<Renderer>().sortingOrder = i;
        }
    }

    // Returns a random int between -1 and 1.
    private int RandomSort(GameObject a, GameObject b)
    {
        return Random.Range(-1, 2);
    }

    // Returns the card with the highest sorting order.
    // Removes the card from the deck.
    public GameObject DrawTopCard()
    {
        size--;
        return cards[size];
    }

    // Returns the card with the highest sorting order.
    // Does not remove the card from the deck.
    public GameObject PeekTopCard()
    {
        GameObject topCard = null;
        if (!IsEmpty())
        {
            topCard = cards[size - 1];
        }
        return topCard;
    }

    // Returns the card with the lowest sorting order.
    // Does not remove the card from the deck.
    public GameObject PeekBottomCard()
    {
        return cards[0];
    }

    // True if no cards are in the deck.
    public bool IsEmpty()
    {
        return (size <= 0);
    }
}
