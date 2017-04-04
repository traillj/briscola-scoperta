﻿// Briscola Scoperta
// Author: traillj

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public Text scoreDisplay;
    public Vector3 trumpPosition;

    // For positioning hands
    public Vector3[] playerCardPositions;
    public Vector3[] compCardPositions;

    // Order in layer behind the background
    private const int HIDDEN_ORDER = -2;

    private Hand playerHand;
    private Hand compHand;
    private Strategy compStrategy = new RandomStrategy();

    private Deck deck;
    private char trumpSuit;
    private Points pointsRef = new BriscolaPoints();

    private bool playerTurn = true;

    // Time computer waits after player chooses a card, in seconds
    private int compWaitTime = 1;
    private bool compWaiting = false;

    private int endTrickWaitTime = 2;
    private bool trickEnding = false;
    private bool trickWaiting = false;

    void Start()
    {
        deck = new Deck();
        playerHand = new Hand(deck, playerCardPositions, pointsRef);
        compHand = new Hand(deck, compCardPositions, pointsRef);
        InitBottomCard();

        playerHand.EnableTouch();
    }

    private void InitBottomCard()
    {
        GameObject trump = deck.PeekBottomCard();
        trumpSuit = trump.name[1];
        trump.GetComponent<Transform>().position = trumpPosition;
    }

    void Update()
    {
        if (trickEnding && !trickWaiting)
        {
            trickWaiting = true;
            StartCoroutine(EndTrick());
        }
        if (trickEnding)
        {
            return;
        }

        playerTurn = playerHand.UpdateTouch();
        if (!playerTurn && !compWaiting)
        {
            compWaiting = true;
            StartCoroutine(CompTurn());
        }
    }

    private IEnumerator CompTurn()
    {
        yield return new WaitForSeconds(compWaitTime);
        GameObject topCard = deck.PeekTopCard();
        Card topCardScript = CardFactory.AddCardScript(topCard, pointsRef);
        Card[] compCards = compHand.GetCardScripts();
        Card[] playerCards = playerHand.GetCardScripts();

        Card chosenCard = compStrategy.ChooseCard(compCards, playerCards,
            topCardScript, trumpSuit);
        chosenCard.Move(true);

        trickEnding = true;
    }

    private IEnumerator EndTrick()
    {
        yield return new WaitForSeconds(endTrickWaitTime);

        GameObject playerCard = playerHand.GetMovedCard();
        GameObject compCard = compHand.GetMovedCard();
        playerCard.GetComponent<Renderer>().sortingOrder = HIDDEN_ORDER;
        compCard.GetComponent<Renderer>().sortingOrder = HIDDEN_ORDER;

        int trickPoints = GetTrickPoints(true);
        scoreDisplay.text = trickPoints.ToString();

        DebugWinningCard();
        //trickEnding = false;
    }

    private int GetTrickPoints(bool playerMovedFirst)
    {
        string playerCard = playerHand.GetMovedCard().name;
        string compCard = compHand.GetMovedCard().name;

        int points;
        if (playerMovedFirst)
        {
            points = pointsRef.PointsWon(playerCard, compCard, trumpSuit);
        }
        else
        {
            points = pointsRef.PointsWon(compCard, playerCard, trumpSuit);
        }
        return points;
    }

    private void DebugWinningCard()
    {
        string playerCard = playerHand.GetMovedCard().name;
        string compCard = compHand.GetMovedCard().name;
        string card = pointsRef.GetWinningCard(playerCard,
            compCard, trumpSuit);
        if (card == playerCard)
        {
            Debug.Log("WIN: " + playerCard + " " + compCard);
        }
        else
        {
            Debug.Log("LOSE: " + playerCard + " " + compCard);
        }
    }

}
