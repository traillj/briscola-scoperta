// Briscola Scoperta
// Author: traillj

using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
    // For positioning hands
    public Vector3[] playerCardPositions;
    public Vector3[] compCardPositions;

    private Hand playerHand;
    private Hand compHand;
    private Strategy compStrategy = new RandomStrategy();

    private Deck deck;
    private char trumpSuit;
    private Points pointsRef = new BriscolaPoints();

    private bool playerTurn = true;
    private bool trickOver = false;

    // Time computer waits after player chooses a card, in seconds
    private int compWaitTime = 1;
    private bool compWaiting = false;

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
        GameObject bottomCard = deck.PeekBottomCard();
        trumpSuit = bottomCard.name[1];
    }

    void Update()
    {
        if (trickOver)
        {
            return;
        }

        playerTurn = playerHand.UpdateTouch();
        if (!playerTurn && !compWaiting)
        {
            compWaiting = true;
            StartCoroutine(CompTurn());
            trickOver = true;
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
    }
}
