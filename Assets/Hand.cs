// Briscola Scoperta
// Author: traillj

using System;
using UnityEngine;

public class Hand
{
    private const int HAND_SIZE = 3;
    private GameObject[] cards;
    private Vector3[] cardPositions;
    private Deck deck;

    private bool canTouch;

    // Draws cards from the deck and moves them to
    // the specified card positions.
    public Hand(Deck deck, Vector3[] cardPositions)
    {
        canTouch = false;
        this.deck = deck;
        this.cardPositions = cardPositions;
        cards = new GameObject[HAND_SIZE];
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i] = deck.DrawTopCard();
            cards[i].GetComponent<Transform>().position = cardPositions[i];
            cards[i].AddComponent<Card>();
        }
    }

    public void EnableTouch()
    {
        canTouch = true;
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].AddComponent<BoxCollider2D>();
        }
    }

    public void UpdateTouch()
    {
        if (canTouch)
        {
            CheckMovement();
        }
    }

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

}
