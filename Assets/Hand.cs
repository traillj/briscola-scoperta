// Briscola Scoperta
// Author: traillj

using UnityEngine;

public class Hand
{
    private const int HAND_SIZE = 3;
    private GameObject[] cards;
    private Vector3[] cardPositions;

    private Deck deck;

    // Draws cards from the deck and moves them to
    // the specified card positions.
    // If it's a player's hand, cards can be played by clicking.
    public Hand(Deck deck, Vector3[] cardPositions, bool player)
    {
        this.deck = deck;
        cards = new GameObject[HAND_SIZE];
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i] = deck.DrawTopCard();
            cards[i].GetComponent<Transform>().position = cardPositions[i];

            if (player)
            {
                cards[i].AddComponent<BoxCollider2D>();
            }
            cards[i].AddComponent<Card>();
        }
    }

}
