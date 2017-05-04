// Briscola Scoperta
// Author: traillj

using System;
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

    public Vector3 deckPos;

    // Order in layer behind the background
    private const int HIDDEN_ORDER = -3;
    private const int MIN_VISIBLE_ORDER = -1;
    private const string CARD_TAG = "Card";

    private Hand playerHand;
    private Hand compHand;
    private Strategy compStrategy = new RandomStrategy();

    private GameObject[] cards;
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

    private Turn playerState;
    private Turn compState;
    private Turn refState;

    private bool playerTurn;
    private bool playerFirst;

    void Start()
    {
        cards = GameObject.FindGameObjectsWithTag(CARD_TAG);
        CardFactory.AddCardScripts(cards, pointsRef);
        NewGame();
    }

    private void NewGame()
    {
        // Reset previous game state
        StopAllCoroutines();
        InitStates();
        InitDeckPos();

        // Before deal hand
        deck = new Deck(cards);
        ResetCardScripts();

        playerHand = new Hand(deck, playerCardPositions, pointsRef);
        compHand = new Hand(deck, compCardPositions, pointsRef);
        InitBottomCard();
        scoreDisplay.text = "0";
    }

    private void InitStates()
    {
        playerState = Turn.Unstarted;
        compState = Turn.Start;
        refState = Turn.Unstarted;
        playerTurn = false;
        playerFirst = false;
    }

    private void InitDeckPos()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            Transform transform = cards[i].GetComponent<Transform>();
            transform.position = new Vector3(deckPos.x, deckPos.y);
        }
    }

    // Resets cards in the player's hand to
    // the default.
    private void ResetCardScripts()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            Card cardScript = cards[i].GetComponent<Card>();
            cardScript.SetClickable(false);
            cardScript.SetMoved(false);
        }
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
                    MoveToMinOrder(playerHand.GetMovedCard());
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
                MoveToMinOrder(compHand.GetMovedCard());
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

    private void MoveToMinOrder(GameObject card)
    {
        card.GetComponent<Renderer>().sortingOrder = MIN_VISIBLE_ORDER;
    }

    private IEnumerator CompTurn()
    {
        yield return new WaitForSeconds(compWaitTime);

        GameObject topCard = deck.PeekTopCard();
        Card topCardScript = null;
        if (topCard != null)
        {
            topCardScript = topCard.GetComponent<Card>();
        }
        Card[] compCards = compHand.GetCardScripts();
        Card[] playerCards = playerHand.GetCardScripts();

        Card chosenCard = UseStrategy(compCards, playerCards, topCardScript);
        chosenCard.Move(true);

        compState = Turn.Finish;
    }

    private Card UseStrategy(Card[] compCards, Card[] playerCards,
        Card topCardScript)
    {
        Card chosenCard;
        GameObject playedCard = playerHand.GetMovedCard();
        if (playedCard != null)
        {
            Card playedCardScript = playedCard.GetComponent<Card>();
            chosenCard = compStrategy.ChooseCard(compCards, playerCards,
                topCardScript, trumpSuit, playedCardScript);
        }
        else
        {
            chosenCard = compStrategy.ChooseCard(compCards, playerCards,
                topCardScript, trumpSuit);
        }
        return chosenCard;
    }

    private IEnumerator EndTrick()
    {
        yield return new WaitForSeconds(endTrickWaitTime);

        int trickPoints = GetTrickPoints();
        int newScore = int.Parse(scoreDisplay.text) + trickPoints;
        scoreDisplay.text = newScore.ToString();

        bool playerWon = DidPlayerWin();
        HideMovedCards();
        if (!deck.IsEmpty())
        {
            DealCardEach(playerWon);
        }

        if (!playerHand.IsEmpty())
        {
            SetTurnOrder(playerWon);
        }
        refState = Turn.Finish;
    }

    // Removes the moved cards from the player's hands
    private void HideMovedCards()
    {
        GameObject playerCard = playerHand.GetMovedCard();
        GameObject compCard = compHand.GetMovedCard();
        playerCard.GetComponent<Renderer>().sortingOrder = HIDDEN_ORDER;
        compCard.GetComponent<Renderer>().sortingOrder = HIDDEN_ORDER;
        playerHand.RemoveMovedCard();
        compHand.RemoveMovedCard();
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

        if (String.Equals(card, playerCard))
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

    public void Restart()
    {
        NewGame();
    }
}
