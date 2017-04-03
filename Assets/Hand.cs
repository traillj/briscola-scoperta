// Briscola Scoperta
// Author: traillj

using System;
using UnityEngine;

public class Hand
{
    private const int HAND_SIZE = 3;
    private GameObject[] cards;
    private Vector3[] cardPositions;

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
            cards[i] = deck.DrawTopCard();
            cards[i].GetComponent<Transform>().position = cardPositions[i];
            CardFactory.AddCardScript(cards[i], pointsRef);
        }
    }

    public Card[] GetCardScripts()
    {
        Card[] cardScripts = new Card[cards.Length];
        for (int i = 0; i < cards.Length; i++)
        {
            cardScripts[i] = cards[i].GetComponent<Card>();
        }
        return cardScripts;
    }

    public void EnableTouch()
    {
        canTouch = true;
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].AddComponent<BoxCollider2D>();
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

    // Disables touch if any card in the hand has moved
    private void CheckMovement()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            if (Math.Abs(cards[i].transform.position.y
                - cardPositions[i].y) > 0.1)
            {
                DisableTouch();
                break;
            }
        }
    }

    private void DisableTouch()
    {
        canTouch = false;
        for (int i = 0; i < cards.Length; i++)
        {
            Card card = cards[i].GetComponent<Card>();
            card.DisableTouch();
        }
    }

    // Returns the first card found that has moved.
    // Returns null if no cards in the hand have moved.
    public Card GetMovedCard()
    {
        Card[] cards = GetCardScripts();
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i].HasMoved())
            {
                return cards[i];
            }
        }
        return null;
    }

}
