// Briscola Scoperta
// Author: traillj

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Acts as referee in game. Keeps track of the current
// game state and updates the game accordingly.
// Controls all objects on the main scene. 
// Starts and can restart the game.
public class Game : MonoBehaviour
{
    // Order in layer behind the background
    private const int HIDDEN_ORDER = -3;
    // Minimum order visible
    private const int MIN_VISIBLE_ORDER = -1;

    // Tag to access all card objects
    private const string CARD_TAG = "Card";

    // Position of the trump card
    public Vector3 trumpPosition;
    // Position of cards in hands
    public Vector3[] playerCardPositions;
    public Vector3[] compCardPositions;
    // Position of the deck
    public Vector3 deckPos;

    // The current score differential
    public Text scoreDisplay;

    // Contains the card objects currently in the player's hand
    private Hand playerHand;
    // Contains the card objects currently in the computer's hand
    private Hand compHand;

    // All cards
    private GameObject[] cards;
    // Contains all undrawn cards
    private Deck deck;
    // The suit of the trump card
    private char trumpSuit;

    // The referee that determines the points won in a trick
    private Points pointsRef = new BriscolaPoints();

    // The computer's card choosing strategy 
    private Strategy compStrategy = new RandomStrategy();

    // Time computer waits after player chooses a card, in seconds
    private int compWaitTime = 1;
    // Wait time after trick ends, in seconds
    private int endTrickWaitTime = 2;

    // Type to store state of the game
    enum Turn
    {
        Unstarted,
        Start,
        Currently,
        Finish,
        Ended
    };

    // State of the players and referee
    private Turn playerState;
    private Turn compState;
    private Turn refState;

    // True if the player had the first move of the game
    private bool playerFirst;


    // Initialises the game.
    void Start()
    {
        cards = GameObject.FindGameObjectsWithTag(CARD_TAG);
        CardFactory.AddCardScripts(cards, pointsRef);
        NewGame();
    }

    // Resets the state, card scripts, deck, hands and score.
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
        InitTrumpCard();
        scoreDisplay.text = "0";
    }

    // Initialises the states of the players and referee.
    private void InitStates()
    {
        playerState = Turn.Unstarted;
        compState = Turn.Start;
        refState = Turn.Unstarted;
        playerFirst = false;
    }

    // Initialises the position of the deck.
    private void InitDeckPos()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            Transform transform = cards[i].GetComponent<Transform>();
            transform.position = new Vector3(deckPos.x, deckPos.y);
        }
    }

    // Resets cards in the player's hand to the default.
    private void ResetCardScripts()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            Card cardScript = cards[i].GetComponent<Card>();
            cardScript.SetClickable(false);
            cardScript.SetMoved(false);
        }
    }

    // Initialises the trump card's position and suit information.
    private void InitTrumpCard()
    {
        GameObject trump = deck.PeekBottomCard();
        trumpSuit = trump.name[1];
        trump.GetComponent<Transform>().position = trumpPosition;
    }


    // Updates the game according to the current state.
    void Update()
    {
        CheckPlayerTurn();
        CheckCompTurn();
        CheckRefTurn();
    }

    // If the player's state is
    //   Start - Enable touch on the player's hand
    //   Currently - Determine whether a card has been chosen
    //               and update the states if true.
    private void CheckPlayerTurn()
    {
        if (playerState == Turn.Start)
        {
            playerState = Turn.Currently;
            playerHand.EnableTouch();
        }

        if (playerState == Turn.Currently)
        {
            bool playerTurn = playerHand.UpdateTouch();
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

    // If the computer's state is
    //   Start - Begin the coroutine waiting a predefined time
    //           before the computer chooses a card
    //   Finish - Update the states.
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

    // If the referee's state is
    //   Start - Begin the coroutine waiting a predefined time
    //           before the score is updated, the trick ends and
    //           the next trick begins.
    //   Finish - Update the referee's state.
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

    // Moves the card to the lowest order still visible in the scene.
    private void MoveToMinOrder(GameObject card)
    {
        card.GetComponent<Renderer>().sortingOrder = MIN_VISIBLE_ORDER;
    }

    // After a pause, chooses and moves a card from the computer's hand.
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

    // Uses the strategy to choose a card.
    private Card UseStrategy(Card[] cards, Card[] opponentCards,
        Card topCardScript)
    {
        Card chosenCard;
        GameObject playedCard = playerHand.GetMovedCard();
        if (playedCard != null)
        {
            Card playedCardScript = playedCard.GetComponent<Card>();
            chosenCard = compStrategy.ChooseCard(cards, opponentCards,
                topCardScript, trumpSuit, playedCardScript);
        }
        else
        {
            chosenCard = compStrategy.ChooseCard(cards, opponentCards,
                topCardScript, trumpSuit);
        }
        return chosenCard;
    }


    // Updates the score, determines the trick winner, updates
    // the states for the next trick, hides the played cards and
    // deals a new card each.
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

    // Hides the moved cards in the scene.
    // Removes the moved cards from the player's hands.
    private void HideMovedCards()
    {
        GameObject playerCard = playerHand.GetMovedCard();
        GameObject compCard = compHand.GetMovedCard();
        playerCard.GetComponent<Renderer>().sortingOrder = HIDDEN_ORDER;
        compCard.GetComponent<Renderer>().sortingOrder = HIDDEN_ORDER;
        playerHand.RemoveMovedCard();
        compHand.RemoveMovedCard();
    }

    // Returns the trick points won by the player.
    // If the computer won points, the result will be negative.
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

    // Returns true if the player won the trick.
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

    // Draws a card for each player. Previous trick winner draws first.
    // playerWon - True if the player won the last trick.
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

    // Updates the state for the next trick.
    // Previous trick winner plays first.
    // playerWon - True if the player won the last trick.
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

    // Restarts the game.
    public void Restart()
    {
        NewGame();
    }
}
