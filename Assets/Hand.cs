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
            AddCard(deck, pointsRef, i);
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
    public GameObject GetMovedCard()
    {
        Card[] cardScripts = GetCardScripts();
        for (int i = 0; i < cardScripts.Length; i++)
        {
            if (cardScripts[i].HasMoved())
            {
                return cards[i];
            }
        }
        return null;
    }

    // Returns true if a card was added to the hand,
    // false otherwise.
    public bool AddCard(Deck deck, Points pointsRef)
    {
        RemoveMovedCard();
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i] == null)
            {
                AddCard(deck, pointsRef, i);
                return true;
            }
        }
        return false;
    }

    // pos = Position of the card from the left, left most is at pos 0.
    private void AddCard(Deck deck, Points pointsRef, int pos)
    {
        cards[pos] = deck.DrawTopCard();
        cards[pos].GetComponent<Transform>().position = cardPositions[pos];
        CardFactory.AddCardScript(cards[pos], pointsRef);
    }

    // Returns true if a card was removed,
    // false otherwise.
    private bool RemoveMovedCard()
    {
        Card[] cardScripts = GetCardScripts();
        for (int i = 0; i < cardScripts.Length; i++)
        {
            if (cardScripts[i].HasMoved())
            {
                cards[i] = null;
                cardScripts[i] = null;
                return true;
            }
        }
        return false;
    }

}
