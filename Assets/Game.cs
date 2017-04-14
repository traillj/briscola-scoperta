// Briscola Scoperta
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

    // Time computer waits after player chooses a card, in seconds
    private int compWaitTime = 1;
    // Wait time after trick ends, in seconds
    private int endTrickWaitTime = 2;

    enum Turn
    {
        Unstarted,
        Start,
        Currently,
        Finish,
        Ended
    };

    private Turn playerState = Turn.Unstarted;
    private Turn compState = Turn.Start;
    private Turn refState = Turn.Unstarted;

    private bool playerTurn = false;
    private bool playerFirst = false;

    void Start()
    {
        deck = new Deck();
        playerHand = new Hand(deck, playerCardPositions, pointsRef);
        compHand = new Hand(deck, compCardPositions, pointsRef);
        InitBottomCard();
    }

    private void InitBottomCard()
    {
        GameObject trump = deck.PeekBottomCard();
        trumpSuit = trump.name[1];
        trump.GetComponent<Transform>().position = trumpPosition;
    }

    void Update()
    {
        CheckPlayerTurn();
        CheckCompTurn();
        CheckRefTurn();
    }

    private void CheckPlayerTurn()
    {
        if (playerState == Turn.Start)
        {
            playerState = Turn.Currently;
            playerHand.EnableTouch();
        }

        if (playerState == Turn.Currently)
        {
            playerTurn = playerHand.UpdateTouch();
            if (!playerTurn)
            {
                playerState = Turn.Ended;
                if (compState == Turn.Unstarted)
                {
                    compState = Turn.Start;
                }
                else
                {
                    refState = Turn.Start;
                }
            }
        }
    }

    private void CheckCompTurn()
    {
        if (compState == Turn.Start)
        {
            compState = Turn.Currently;
            StartCoroutine(CompTurn());
        }
        else if (compState == Turn.Finish)
        {
            compState = Turn.Ended;
            if (playerState == Turn.Unstarted)
            {
                playerState = Turn.Start;
            }
            else
            {
                refState = Turn.Start;
            }
        }
    }

    private void CheckRefTurn()
    {
        if (refState == Turn.Start)
        {
            refState = Turn.Currently;
            StartCoroutine(EndTrick());
        }
        else if (refState == Turn.Finish)
        {
            refState = Turn.Unstarted;
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

        compState = Turn.Finish;
    }

    private IEnumerator EndTrick()
    {
        yield return new WaitForSeconds(endTrickWaitTime);
        HideMovedCards();

        int trickPoints = GetTrickPoints();
        int newScore = int.Parse(scoreDisplay.text) + trickPoints;
        scoreDisplay.text = newScore.ToString();

        bool playerWon = DidPlayerWin();
        DealCardEach(playerWon);
        SetTurnOrder(playerWon);

        refState = Turn.Finish;
    }

    private void HideMovedCards()
    {
        GameObject playerCard = playerHand.GetMovedCard();
        GameObject compCard = compHand.GetMovedCard();
        playerCard.GetComponent<Renderer>().sortingOrder = HIDDEN_ORDER;
        compCard.GetComponent<Renderer>().sortingOrder = HIDDEN_ORDER;
    }

    private int GetTrickPoints()
    {
        string playerCard = playerHand.GetMovedCard().name;
        string compCard = compHand.GetMovedCard().name;

        int points;
        if (playerFirst)
        {
            points = pointsRef.PointsWon(playerCard, compCard, trumpSuit);
        }
        else
        {
            points = -pointsRef.PointsWon(compCard, playerCard, trumpSuit);
        }
        return points;
    }

    private bool DidPlayerWin()
    {
        string playerCard = playerHand.GetMovedCard().name;
        string compCard = compHand.GetMovedCard().name;
        string card;

        if (playerFirst)
        {
            card = pointsRef.GetWinningCard(playerCard,
                compCard, trumpSuit);
        }
        else
        {
            card = pointsRef.GetWinningCard(compCard,
                playerCard, trumpSuit);
        }

        if (card == playerCard)
        {
            playerFirst = true;
            Debug.Log("WIN  " + playerCard + " " + compCard);
            return true;
        }

        playerFirst = false;
        Debug.Log("LOSE " + playerCard + " " + compCard);
        return false;
    }

    // Trick winner draws first
    private void DealCardEach(bool playerWon)
    {
        if (playerWon)
        {
            playerHand.AddCard(deck, pointsRef);
            compHand.AddCard(deck, pointsRef);
        }
        else
        {
            compHand.AddCard(deck, pointsRef);
            playerHand.AddCard(deck, pointsRef);
        }
    }

    // Trick winner plays first
    private void SetTurnOrder(bool playerWon)
    {
        if (playerWon)
        {
            playerState = Turn.Start;
            compState = Turn.Unstarted;
        }
        else
        {
            playerState = Turn.Unstarted;
            compState = Turn.Start;
        }
    }

}
