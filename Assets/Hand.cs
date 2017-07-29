// Briscola Scoperta
// Author: traillj

using System;
using UnityEngine;

// Represents a hand of cards.
// Controls whether cards can be moved.
public class Hand
{
    // Number of cards in a hand
    private const int HAND_SIZE = 3;

    // Some indexes may be null
    private GameObject[] cards;
    // Position of cards in hand
    private Vector3[] cardPositions;

    // Number of cards currently in hand
    private int numCards = 0;
    // True if a card can be moved
    private bool canTouch;

    // Draws cards from the deck and moves them to
    // the specified card positions.
    public Hand(Deck deck, Vector3[] cardPositions, Points pointsRef)
    {
        canTouch = false;
        this.cardPositions = cardPositions;
        cards = new GameObject[HAND_SIZE];
        for (int i = 0; i < cards.Length; i++)
        {
            AddCard(deck, i);
        }
    }

    // Returns the card scripts ignoring indexes without cards.
    public Card[] GetCardScripts()
    {
        Card[] cardScripts = new Card[numCards];
        int j = 0;
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i] != null)
            {
                cardScripts[j] = cards[i].GetComponent<Card>();
                j++;
            }
        }
        return cardScripts;
    }

    // Allows cards in hand to be moved when clicked.
    public void EnableTouch()
    {
        canTouch = true;
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i] != null)
            {
                Card cardScript = cards[i].GetComponent<Card>();
                cardScript.SetClickable(true);
            }
        }
    }

    // Returns false when a card has been touched,
    // otherwise returns true.
    public bool UpdateTouch()
    {
        bool playerTurn = true;
        if (canTouch)
        {
            CheckMovement();
        }
        else
        {
            playerTurn = false;
        }
        return playerTurn;
    }

    // Disables touch if any card in the hand has moved.
    private void CheckMovement()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i] != null && Math.Abs(cards[i].transform.position.y
                - cardPositions[i].y) > 0.1)
            {
                DisableTouch();
                break;
            }
        }
    }

    // Prevents cards in hand from moving when clicked.
    private void DisableTouch()
    {
        canTouch = false;
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i] != null)
            {
                Card cardScript = cards[i].GetComponent<Card>();
                cardScript.SetClickable(false);
            }
        }
    }

    // Returns the first card found that has moved.
    // Returns null if no cards in the hand have moved.
    public GameObject GetMovedCard()
    {
        GameObject movedCard = null;
        int i = GetMovedCardIndex();
        if (i >= 0)
        {
            movedCard = cards[i];
        }
        return movedCard;
    }

    // Returns the index of the moved card.
    // This index considers indexes with no cards.
    // If no cards have moved, returns -1.
    private int GetMovedCardIndex()
    {
        Card[] cardScripts = GetCardScripts();
        int j = 0;
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i] != null)
            {
                if (cardScripts[j].HasMoved())
                {
                    return i;
                }
                j++;
            }
        }
        return -1;
    }

    // Returns true if a card was added to the hand,
    // false otherwise.
    public bool AddCard(Deck deck)
    {
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i] == null)
            {
                AddCard(deck, i);
                return true;
            }
        }
        return false;
    }

    // Moves the top card from the deck to specified position in the hand.
    // pos - position of the card from the left, left most is at pos 0.
    private void AddCard(Deck deck, int pos)
    {
        numCards++;
        cards[pos] = deck.DrawTopCard();
        cards[pos].GetComponent<Transform>().position = cardPositions[pos];
    }

    // Returns true if a card was removed, false otherwise.
    public bool RemoveMovedCard()
    {
        bool cardRemoved = false;
        int i = GetMovedCardIndex();
        if (i >= 0)
        {
            cards[i] = null;
            numCards--;
            cardRemoved = true;
        }
        return cardRemoved;
    }

    // True if there are no cards in the hand.
    public bool IsEmpty()
    {
        return (numCards == 0);
    }
}
