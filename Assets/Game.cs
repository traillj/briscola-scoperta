﻿// Briscola Scoperta
// Author: traillj

using UnityEngine;

public class Game : MonoBehaviour
{
    // For positioning hands
    public Vector3[] playerCardPositions;
    public Vector3[] compCardPositions;

    private Hand playerHand;
    private Hand compHand;

    void Start()
    {
        Deck deck = new Deck();
        playerHand = new Hand(deck, playerCardPositions, true);
        compHand = new Hand(deck, compCardPositions, false);
    }

}